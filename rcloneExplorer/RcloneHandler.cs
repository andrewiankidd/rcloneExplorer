using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rcloneExplorer
{
    public class rcloneObject
    {
        public string Name { get; set; } = string.Empty;
        // public string Path { get; set; }      
        public long Size { get; set; } = 0;
        public DateTime ModTime { get; set; } = DateTime.MinValue;
        public bool IsDir { get; set; } = false;
        public string BaseDir { get; set; } = string.Empty;
        public string Progress { get; set; } = string.Empty;
    }
    class RcloneHandler
    {
        private static string configPath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rclone.conf");
        private static Dictionary<int, List<string>> stdOut = new Dictionary<int, List<string>>();
        private static Dictionary<int, List<string>> stdErr = new Dictionary<int, List<string>>();
        private static Dictionary<string, string> remotesDict = new Dictionary<string, string>();
        private static Dictionary<string, List<Tuple<int, string>>> activeTransfers = new Dictionary<string, List<Tuple<int, string>>>();

        public void CheckUpdates(Func<string, int> updateStatus)
        {

            updateStatus("Checking for rclone Updates...");
            using (WebClient webClient = new WebClient())
            {
                // Use the appsetting to locate and check the current rclone version string
                string onlineVersionStr = webClient.DownloadString(Program.AppSettings["versionURL"]);

                // Get the Regex'd version number
                decimal onlineVersion = (decimal)Convert.ToDecimal(Regex.Match(onlineVersionStr, "([0-9].[0-9])").Value);

                // If rclone exists locally, grab the version number. Otherwise set version to 0 to force a download.
                decimal currentVersion = File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rclone.exe")) ? GetRcloneVersion() : 0;

                // If there is an update available, grab it
                if (onlineVersion > currentVersion)
                {
                    updateStatus($"Updating rclone v{currentVersion} to v{onlineVersion}");
                    webClient.DownloadProgressChanged += (s, e) =>
                    {
                        updateStatus($"Downloading: {e.ProgressPercentage}");
                    };
                    using (MemoryStream data = new MemoryStream(webClient.DownloadData(Program.AppSettings["updateURL"])))
                    {
                        ZipArchive archive = new ZipArchive(data);
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            if (entry.Name.Equals("rclone.exe", StringComparison.OrdinalIgnoreCase))
                            {
                                var stream = entry.Open();
                                byte[] bytes;
                                using (var ms = new MemoryStream())
                                {
                                    stream.CopyTo(ms);
                                    bytes = ms.ToArray();
                                }
                                File.WriteAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rclone.exe"), bytes);
                            }
                        }
                    }
                }
                else
                {
                    // Versions match so okay to continue
                    updateStatus($"Running latest version (rclone v{currentVersion})");
                }
            }
        }

        public Dictionary<string, string> CheckConfig(Func<string, int> updateStatus)
        {
            // Run rclone config, save output to output list
            var rclone = RcloneExec("config", true);

            // No remotes set up
            if (stdOut[rclone.Id].Any(str => str.Contains("No remotes found")))
            {
                updateStatus("Starting rclone remote setup");

                rcloneInput(ref rclone, "n");
                while (!rclone.HasExited)
                {
                    lock (stdErr)
                    {
                        string lastMsg = String.Join(Environment.NewLine, stdOut[rclone.Id].ToArray());
                        string answer = rclonePrompt(lastMsg);
                        rcloneInput(ref rclone, answer);
                    }
                }
            }
            else
            {
                updateStatus("Parsing remotes..");

                // Parse remotes from rclone menu
                var stdOutRemotes = stdOut[rclone.Id].Where(s => (stdOut[rclone.Id].IndexOf(s) > stdOut[rclone.Id].IndexOf("====                 ====")) && (stdOut[rclone.Id].IndexOf(s) < (stdOut[rclone.Id].IndexOf(string.Empty, stdOut[rclone.Id].IndexOf(string.Empty) + 1))));

                // Iterate the assumed remotes and index them
                foreach (string remote in stdOutRemotes)
                {
                    string longestRun = new string(remote.Select((c, index) => remote.Substring(index).TakeWhile(e => e == ' '))
                                                   .OrderByDescending(e => e.Count())
                                                   .First().ToArray());

                    var r = remote.Split(new[] { longestRun }, StringSplitOptions.None);
                    remotesDict.Add(r[0], r[1]);
                }

                updateStatus($"Found {remotesDict.Count} remotes");
                rcloneInput(ref rclone, "q");
            }

            return remotesDict;
        }

        public Dictionary<string, List<rcloneObject>> ListDirectory(string dir = "")
        {
            // Run rclone lsjson
            dir = (dir.Equals(string.Empty) ? $@"{getSelectedRemote()}:" : $"{dir}");
            var rclone = RcloneExec($"lsjson \"{dir.Replace(@"\", @"\\")}\" -L");

            // Parse output to string
            var outputString = string.Join(Environment.NewLine, stdOut[rclone.Id]);

            // parse output string to list of rcloneObjects
            var unsortedResults = JsonConvert.DeserializeObject<List<rcloneObject>>(outputString);

            // Add current transfers
            unsortedResults.AddRange(ListTransfers(dir));

            // Order by filename
            List<rcloneObject> tDirs = unsortedResults.Where(o => o.IsDir).OrderBy(p => p.Name).ToList();
            List<rcloneObject> tFiles = unsortedResults.Where(o => !o.IsDir).OrderBy(p => p.Name).ToList();

            // Join directory and file results, showing directories first
            var result = new List<rcloneObject>();
            result.AddRange(tDirs);
            result.AddRange(tFiles);

            // Set baseDir property on elements
            result.ForEach(a => a.BaseDir = dir);

            // Create dictionary with directory and elements
            var dict = new Dictionary<string, List<rcloneObject>>();
            dict.Add(dir, result);

            return dict;
        }

        public string dumpTransfers()
        {
            var x = string.Empty;
            activeTransfers.ToList().ForEach(t => {
                if (t.Value.Count>0)
                {
                    var objectPID = t.Value.First().Item1;
                    var path = $@"{t.Key}\{t.Value.First().Item2}";
                    var progress = parseProgress(objectPID);
                    x += $@"[{objectPID}] {path} - {progress}{Environment.NewLine}";
                }      
            });
            return x;
        }

        private string parseProgress(int objectPID, bool percentOnly = false)
        {
            var progress = string.Empty;
            lock (stdOut)
            {
                progress = stdOut.ContainsKey(objectPID) ? (stdOut[objectPID].Where(w => w.Contains('%')).Count() > 0 ? stdOut[objectPID].LastOrDefault(w => w.Contains('%')).Split(':')[1] : string.Empty) : string.Empty;
            }
            if (percentOnly && progress.Contains('%'))
            {
                return $"{progress.Split(new[] { '%' }, 2).ToList().First()}%";
            }
            return progress;
        }

        public List<rcloneObject> ListTransfers(string dir)
        {
            List<rcloneObject> results = new List<rcloneObject>();
            var toRemove = new List<Tuple<int, string>>();
            // Check for files in progress
            if (activeTransfers.ContainsKey(dir))
            {
                activeTransfers[dir].ForEach(obj =>
                {
                    var objectName = obj.Item2;
                    var objectPID = obj.Item1;
                    var objectPrg = "0";
                    if (Process.GetProcesses().Any(x => x.Id == objectPID))
                    {
                        objectPrg = parseProgress(objectPID, true);

                        var x = new rcloneObject();
                        x.Name = objectName;
                        x.IsDir = false;
                        x.ModTime = DateTime.UtcNow;
                        x.Progress = objectPrg;
                        x.Size = 0;
                        results.Add(x);
                    }
                    else
                    {
                        toRemove.Add(obj);
                    }
                });
            }
            if (activeTransfers.ContainsKey(dir))
            {
                lock (activeTransfers[dir])
                {
                    toRemove.ForEach(s => {
                        activeTransfers[dir].Remove(s);
                    });
                }
            }
              
            return results;
        }

        public Process Cat(string remotePath)
        {
            var rclone = RcloneExec($"cat \"{remotePath}\"", false, false, false, false, false);
            return rclone;
        }

        public string CacheFile(string remotePath, string sizeString)
        {
            string tempPath = Path.Combine("temp", Path.GetFileName(remotePath));
            long remoteSize = Convert.ToInt64(sizeString);

            // save file to temp path
            var rclone = RcloneExec($"copyto \"{remotePath}\" \"{tempPath}\"", false, true, false);

            while (!File.Exists(tempPath))
            {
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            var openIncompleteFiles = false;
            if (openIncompleteFiles)
            {
                // once file is over 10% or 50mb (whatevers smaller)
                long localSize = new FileInfo(tempPath).Length;
                while (localSize < ((50 * 1024) * 1024) && localSize < (remoteSize * 0.10))
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    localSize = new FileInfo(tempPath).Length;
                }
            }
            else
            {
                while (Process.GetProcesses().Any(x => x.Id == rclone.Id))
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }

            return tempPath;
        }

        internal void Download(string dir, string name, string targetFilename)
        {
            var remotePath = $@"{dir}\{name}";

            targetFilename = targetFilename.Replace(@"\", @"\\");

            // Seems to be a bug or something where rclone copyto C:\ creates a folder named 'C' in the current directory.. 
            // Only seems to happen if you're running on the same drive you're transferring to
            // Additionally if you try \\DRIVE:\\ it creates a folder on DRIVE: called DRIVE...?
            // to mitigate check if we're running on DRIVE: and copying to DRIVE:, then convert to \\DRIVE:\\..\\ which sems to fix it
            if (Path.GetPathRoot(targetFilename).Equals(Path.GetPathRoot((AppDomain.CurrentDomain.BaseDirectory))))
            {
                targetFilename = $"{targetFilename.Replace(Path.GetPathRoot(targetFilename), $@"\\{Path.GetPathRoot(targetFilename)}\\..\\")}";
            }


            var rclone = RcloneExec($"copyto \"{remotePath}\" \"{targetFilename}\"", false, true, false);
            AddActiveTransfer(dir, name, rclone.Id);
        }

        internal void Delete(string dir, string name)
        {
            var remotePath = $@"{dir}\{name}";

            RcloneExec($"delete \"{remotePath}\"");
        }

        internal void Purge(string dir, string name)
        {
            var remotePath = $@"{dir}\{name}";

            RcloneExec($"purge \"{remotePath}\"");
        }

        internal void Rename(string dir, string name, string newName)
        {
            var remotePath = $@"{dir}\{name}";
            var newRemotePath = $@"{dir}\{newName}";

            RcloneExec($"moveto \"{remotePath}\" \"{newRemotePath}\"");
        }

        internal void Move(string targetDir, string remotePath)
        {
            var rclone = RcloneExec($"move \"{remotePath}\" \"{targetDir}\"", false, true, false);
            AddActiveTransfer(targetDir, remotePath, rclone.Id);
        }

        internal void Copy(string targetDir, string remotePath)
        {
            var rclone = RcloneExec($"copy \"{remotePath}\" \"{targetDir}\"", false, true, false);
            AddActiveTransfer(targetDir, remotePath, rclone.Id);
        }

        internal void KillTransfer(string dir, string name)
        {
            var transfer = activeTransfers[dir].Where(o => o.Item2.Equals(name)).FirstOrDefault();
            int transferPID = transfer.Item1;
            Program.KillProcessAndChildren(transferPID);
            activeTransfers[dir].Remove(transfer);
        }

        public void Upload(string targetDir, string filePath)
        {
            // Prevent double slashes
            targetDir = targetDir.Replace("//", "/");

            // Run rclone copyto
            var rclone = RcloneExec($"copy \"{filePath}\" \"{targetDir}\"", false, true, false);

            AddActiveTransfer(targetDir, filePath, rclone.Id);
        }

        private void AddActiveTransfer(string dir, string path, int pid)
        {
            // add uploading files somewhere (dictionary, basedrive, filelist)
            // when pulling filelist, add files which are on the uploading list for that basedir
            // potentially add a progress column

            if (!activeTransfers.ContainsKey(dir))
            {
                activeTransfers.Add(dir, new List<Tuple<int, string>>());
            }
            activeTransfers[dir].Add(new Tuple<int, string>(pid, Path.GetFileName(path)));
        }

        public void rcloneInput(ref Process rclone, string text)
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            rclone.StandardInput.WriteLine(text);
            getStdOut(ref rclone);
        }

        public string rclonePrompt(string text)
        {
            return Microsoft.VisualBasic.Interaction.InputBox(text, "rcloneExplorer Prompt", "", -1, -1);
        }

        public decimal GetRcloneVersion()
        {
            // Run rclone version
            var rclone = RcloneExec("version");

            // Get output as a string
            string versionString = stdOut[rclone.Id].FirstOrDefault(str => str.Contains("rclone v"));

            // Return Regex'd version number
            return (decimal)Convert.ToDecimal(Regex.Match(versionString, "([0-9].[0-9])").Value);
        }

        /// <summary>
        /// Executes rclone with required parameters and methods for parsing.
        /// </summary>
        /// <param name="command">The rclone command to run, with arguments</param> 
        /// <param name="interactive">If the command is interactive</param> 
        /// <param name="verbose">Enable verbosity parameter</param> 
        /// <param name="WaitForExit">Should wait for the rclone command to fully complete before returning.</param> 
        public Process RcloneExec(string command, bool interactive = false, bool verbose = false, bool WaitForExit = true, bool execute = true, bool captureOutput = true)
        {
            Process rclone = new Process();
            rclone.StartInfo.FileName = "cmd.exe";
            rclone.StartInfo.Arguments = $"/c rclone.exe {(interactive | verbose ? "-v" : "")} --stats 1s --config \"{configPath}\" {command}";
            rclone.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            rclone.StartInfo.UseShellExecute = false;
            rclone.StartInfo.CreateNoWindow = true;

            if (captureOutput)
            {
                rclone.StartInfo.RedirectStandardError = true;
                rclone.StartInfo.StandardErrorEncoding = Encoding.UTF8;
                rclone.StartInfo.RedirectStandardOutput = true;
                rclone.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                rclone.StartInfo.RedirectStandardInput = true;
            }

            if (!interactive)
            {
                // Non interactive process we can just dump the results out into the relevant lists.
                if (execute) { rclone.Start(); }
                if (rclone.StartInfo.RedirectStandardOutput) { rclone.OutputDataReceived += (sender, args) => proc_OutputDataReceived(sender, args, rclone.Id); rclone.BeginOutputReadLine(); }
                if (rclone.StartInfo.RedirectStandardError) { rclone.ErrorDataReceived += (sender, args) => proc_OutputDataReceived(sender, args, rclone.Id); rclone.BeginErrorReadLine(); }
                if (WaitForExit) { rclone.WaitForExit(); }
            }
            else
            {
                // For interactive processes we need to step through whole window, not just line by line.

                // Start rclone
                if (execute) { rclone.Start(); }

                // Store output to relevant list
                if (rclone.StartInfo.RedirectStandardOutput) { getStdOut(ref rclone); }
                if (rclone.StartInfo.RedirectStandardError) { rclone.ErrorDataReceived += (sender, args) => proc_OutputDataReceived(sender, args, rclone.Id); }
            }

            if (execute){Console.WriteLine($"[{rclone.Id}] {rclone.StartInfo.FileName} {rclone.StartInfo.Arguments}"); }
  
            return rclone;
        }

        /// <summary>
        /// Returns the currently selected remote.
        /// </summary>
        public string getSelectedRemote()
        {
            return Program.selectedRemote;
        }
        
        public Dictionary<string, string> getRemoteList()
        {
            return remotesDict;
        }

        public string GetRemoteFromString(string input)
        {
            string remoteName = (input.Split(':').First());
            return remoteName;
        }

        /// <summary>
        /// Read getStdOut character by character, even incomplete lines (prompts).
        /// </summary>
        /// <param name="rclone_ref">The process to monitor.</param>
        private string getStdOut(ref Process rclone_ref)
        {
            // Iterate through the window, copy each letter into the charbuffer
            // kill the process if it takes more than two seconds. (assume waiting for input)
            // after each successful copy, convert the charbuffer into a string
            string x = string.Empty;

            Process rclone = rclone_ref;
            bool timedOut = false;

            var task = Task.Run(() =>
            {
                char[] charBuffer = new char[1000];
                int index = 0;
                while (index < charBuffer.Length && !timedOut)
                {
                    lock (stdOut)
                    {
                        rclone.StandardOutput.ReadBlock(charBuffer, index, 1);
                        Console.WriteLine($"copying char:{charBuffer[index]} - index: {index}");
                        x = new string(charBuffer).TrimEnd('\0');
                        index++;
                    }                  
                };
            });
            if (!task.Wait(TimeSpan.FromSeconds(5))) { Console.WriteLine($"[{rclone.StartInfo.Arguments}] Timed out!"); timedOut = true; }

            storeOutput(ref stdOut, rclone.Id, x, true);
            return x;
        }

        /// <summary>
        /// Stores text send by program in relevant list. Splits the text into lines for readability.
        /// </summary>
        /// <param name="list">The Dictionary to write to, is indexed by process ID and value is list of output.</param>
        /// <param name="id">Process ID, used as index for dictionary.</param>
        /// <param name="text">String to store.</param>
        /// <param name="clear">Should previous stored data be removed?</param>
        private void storeOutput(ref Dictionary<int, List<string>> list, int id, string text, bool clear = false)
        {
            if (!list.ContainsKey(id)) { list.Add(id, new List<string>()); };
            string[] lines = text.Split( new[] { '\n' }, StringSplitOptions.None);
            if (clear) { list[id].Clear(); }
            list[id].AddRange(lines);
            Console.WriteLine($"[{id}] - {text}");
        }

        /// <summary>
        /// Triggered when process writes to stdout, validates the data and sends it to be stored for processing.
        /// </summary>
        /// <param name="id">Process ID, for reference when looking up output later..</param>
        private void proc_OutputDataReceived(object sender, DataReceivedEventArgs e, int id)
        {
            if (e.Data != null)
            {
                storeOutput(ref stdOut, id, e.Data.ToString());
            }
        }

        /// <summary>
        /// Triggered when process writes to stderr, validates the data and sends it to be stored for processing.
        /// </summary>
        /// <param name="id">Process ID, for reference when looking up output later..</param>
        private void proc_ErrorDataReceived(object sender, DataReceivedEventArgs e, int id)
        {
            if (e.Data != null)
            {
                storeOutput(ref stdErr, id, e.Data.ToString());
            }
        }
    }
}

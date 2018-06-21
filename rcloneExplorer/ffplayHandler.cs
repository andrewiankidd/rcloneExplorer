using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace rcloneExplorer
{
    class FfplayHandler
    {
        public void CheckUpdates(Func<string, int> updateStatus)
        {

            updateStatus("Checking for ffplay Updates...");
            using (WebClient webClient = new WebClient())
            {
                // default online version to 10
                decimal onlineVersion = 1;

                // only really care if it exists already, so 1 for installed, 0 if not.
                decimal currentVersion = File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffplay.exe")) ? 1 : 0;

                // If there is an update available, grab it
                if (onlineVersion > currentVersion)
                {
                    updateStatus($"Updating ffplay v{currentVersion} to v{onlineVersion}");
                    webClient.DownloadProgressChanged += (s, e) =>
                    {
                        updateStatus($"Downloading: {e.ProgressPercentage}");
                    };
                    using (MemoryStream data = new MemoryStream(webClient.DownloadData(Program.AppSettings["ffUpdateURL"])))
                    {
                        ZipArchive archive = new ZipArchive(data);
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            if (entry.Name.Equals("ffplay.exe", StringComparison.OrdinalIgnoreCase))
                            {
                                var stream = entry.Open();
                                byte[] bytes;
                                using (var ms = new MemoryStream())
                                {
                                    stream.CopyTo(ms);
                                    bytes = ms.ToArray();
                                }
                                File.WriteAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffplay.exe"), bytes);
                            }
                        }
                    }
                }
                else
                {
                    // Versions match so okay to continue
                    updateStatus($"Running latest version (ffplay v{currentVersion})");
                }
            }
        }

        private Process FfplayExec(string command, string windowTitle = "ffplayrcloneExplorer", bool execute = true)
        {
            Process ffplay = new Process();
            ffplay.StartInfo.FileName = "ffplay.exe";
            ffplay.StartInfo.Arguments = $" -loglevel quiet -window_title {windowTitle} {command}";
            ffplay.StartInfo.UseShellExecute = false;
            ffplay.StartInfo.CreateNoWindow = true;
            ffplay.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            ffplay.Start();

            Console.WriteLine($"[{ffplay.Id}] {ffplay.StartInfo.FileName} {ffplay.StartInfo.Arguments}");
            return ffplay;
        }

        public Process Stream(Process rclone, string windowTitle)
        {
            var ffplay = FfplayExec("-", windowTitle, false);

            Process pipeProcess = new Process();
            pipeProcess.StartInfo.FileName = "cmd.exe";
            pipeProcess.StartInfo.Arguments = $"/c {rclone.StartInfo.FileName}{rclone.StartInfo.Arguments}|{ffplay.StartInfo.FileName}{ffplay.StartInfo.Arguments}";
            pipeProcess.StartInfo.UseShellExecute = false;
            pipeProcess.StartInfo.CreateNoWindow = true;
            pipeProcess.Start();

            Console.WriteLine($"[{pipeProcess.Id}] {pipeProcess.StartInfo.FileName} {pipeProcess.StartInfo.Arguments}");
            return pipeProcess;
        }

        public Process Play(string file)
        {
            var ffplay = FfplayExec($"\"{file}\"");
            return ffplay;
        }
    }
}

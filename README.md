#rcloneExplorer ![](http://i.imgur.com/T4We4ZK.png) 

Crappy rclone GUI for Windows
Runs rclone commands and displays output based on the results

##Screenshot:
<p align="center">
  <img src="http://i.imgur.com/rA0vdht.gif"/>
</p>

##Usage
1. Have a working rclone config on machine
2. have rclone.exe in working directory
3. run rcloneExplorer.exe

##Features/How it works
###listing files
1. Sends an rclone lsl command to a hidden cmd window and redirects output back to itself
2. processes the input into files and directories
3. displays them in a list view

###downloading files
1. when a file is selected it sends an rclone copy command to copy the file from remote to local
2. periodically checks the filesize of the saved file, compares it against the remote file and shows a percentage

####cancelling files
1. when the download is started the process ID is stored
2. when the cancel is requested the download process is killed
3. after the process is killed the local file is deleted as it would be broken

###quit (continue transfers)
1. simply closes the GUI application
2. all background processes will still run

###quit
1. scans through all stored process ID's
2. checks if they are still active
3. kills them if they are
4. this method doesnt cleanup broken files (yet)

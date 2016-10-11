#rcloneExplorer ![](http://i.imgur.com/T4We4ZK.png) 

Crappy rclone GUI for Windows
Runs rclone commands and displays output based on the results

##Screenshot:
<p align="center">
  <img src="https://i.imgur.com/ebJq30W.gif"/>
</p>

##Usage
1. Have a working rclone config on machine
2. have rclone.exe in working directory
3. run rcloneExplorer.exe

##Features
###Browse encrypted remotes transparently
rcloneExplorer merely takes output from rclone, which does all the heavy lifting. If you have an encrypted remote set up in rclone and point rcloneExplorer to it, it will work just as well as with a non-encrypted remote. rcloneExplorer can only access rclone configured remotes.

###Streaming
Making use of ['rclone cat'](http://rclone.org/commands/rclone_cat/) and [ffplay](https://ffmpeg.org/ffplay.html), media files can be streamed directly from remote.

###Syncing
rcloneExplorer provides a simple GUI to set up and schedule a regular sync option, to keep your files backed up and safe on a regular basis. [Currently a WIP]

###Downloading Files
You can download files from the remote directly to your machine, with a simple click-and-save GUI.

###Uploading Files
To quickly upload some files to your remote, drag and drop them onto the remote folder and the upload will start automatically. If you're uploading to an encrypted remote, the file will be encrypted on the fly courtesy of rclone.

###Cancelling Transfers
You can cancel uploads and downloads just as easily as starting them, just right click and cancel. Any partial files downloaded will be deleted automatically.

###Quit (continue transfers)
simply closes the GUI application, all background processes will still run

###Quit
quits the GUI application, and also kills any rclone transfers in process. NOTE: this method does not delete partly transferred files.

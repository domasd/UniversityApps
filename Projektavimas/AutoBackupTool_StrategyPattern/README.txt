Auto backup tool v0.1 Domas Dziaugys 2015-10-15

This program is used form automatic scheduling and backuping files to cloud and local storage
Warning: It just uses local cloud folders, whereas the cloud app which is doing the big job of working directly with cloud.

Solution has possibilities to work with either google drive or dropbox and could easily scale with other clouds. 
For a local storage it is possible to work with filesystem or ftp.

If you want to use it, provide real credentials in App.config

DROPBOX
I dont have a lot of space in dropbox so I'm not keeping any old backups there, so before any backup upload
I delete everything and compress new files.

FileSystem and Ftp
Because of having a lot of space, I do not delete old backups, instead for each day backup occus - it creates new folder 
and backups there



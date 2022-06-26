# Remote Media Control for Windows

This is a server for [Remote Media Control system](https://github.com/Remote-Media-Control/core) implemented C# for ASP.NET.

Due to achitecture of Windows, the only normal way to implement RMC server for Windows was using C#.

But, from the other side - this server integrates with `SystemMediaTransportControls` mechanism - this is windows-only
> built-in system UI to display media metadata such as artist, album title, or chapter title. The system transport control also allows a user to control the playback of a media app using the built-in system UI, such as pausing playback and skipping forward and backward in a playlist.

![smtc example](https://docs.microsoft.com/en-us/uwp/api/windows.media/images/smtc.png?view=winrt-22621)

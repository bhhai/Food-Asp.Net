# CKFinder 3 GetFileInfo Plugin Sample

This sample plugin illustrates [how to create custom CKFinder commands](http://docs.cksource.com/ckfinder3-net/howto.html#howto_custom_commands).

If this plugin is enabled, you can call an additional `GetFileInfo` command that returns some very basic
information about a file, like the size and the last modification timestamp. This behavior can be simply altered to return any 
other information about the file (for example EXIF data for images or ID3 tags for mp3 files).

## Sample Request (HTTP GET Method)

Get basic information about the `foo.png` file located in the `sub1` directory of the `Files` resource type.

```
ckfinder/connector?command=GetFileInfo&type=Files&currentFolder=/sub1/&fileName=foo.png
```

## Sample Response

```
{
    "type": "file",
    "path":"files\/sub1\/1.png",
    "timestamp":1425909932,
    "size":1336
}
```

For more detailed information about commands, please refer to [Commands section](http://docs.cksource.com/ckfinder3-net/commands.html) of CKFinder 3 .NET connector documentation.

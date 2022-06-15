# CKFinder 3 ImageWatermark Plugin

This sample plugin illustrates the modification of stream in events.

## Configuration Options

To set a custom image to use as a watermark add the following option to the main CKFinder configuration file (usually `Web.config`):

```xml
<plugin name="Watermark">
  <option name="watermarkPath" value="/custom/image/path/stamp.png" />
</plugin>
```

**Note:** Make sure that your image path is absolute, and use a transparent `png` image for best results.

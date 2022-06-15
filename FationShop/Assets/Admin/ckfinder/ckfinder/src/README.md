# CKFinder 3 - .Net Samples

This repository contains ready-to-use code samples created for the [CKFinder .Net connector documentation](http://docs.cksource.com/ckfinder3-net/).

## Installation

1. Clone this repository (or download ZIP).
2. Compile the solution
2. Move compiled plugins to the CKFinder `plugins` directory, so the structure looks like below:

```
plugins
+-- DiskQuota
¦   +-- CKSource.CKFinder.Connector.Plugin.DiskQuota.dll
+-- GetFileInfo
¦   +-- CKSource.CKFinder.Connector.Plugin.GetFileInfo.dll
+-- UserActionsLogger
¦   +-- CKSource.CKFinder.Connector.Plugin.UserActionsLogger.dll
+-- Watermark
    +-- CKSource.CKFinder.Connector.Plugin.Watermark.dll
```

To enable plugins, add their names to the [`plugins`](http://docs.cksource.com/ckfinder3-net/configuration.html#configuration_options_plugins) configuration option in the connector configuration file (by default `Web.config`):

```xml
<plugins folder="plugins">
  <plugin name="DiskQuota">
    <option name="userQuota" value="209715200" /> <!-- 209715200 = 200MB -->
  </plugin>
  <plugin name="GetFileInfo" />
  <plugin name="UserActionsLogger" />
  <plugin name="Watermark">
    <option name="watermarkPath" value="/custom/image/path/stamp.png" />
  </plugin>
</plugins>
```

## License

Copyright (c) 2016, CKSource - Frederico Knabben. All rights reserved.
For license details see: [LICENSE.md](https://github.com/ckfinder/ckfinder-docs-samples-net/blob/master/LICENSE.md).

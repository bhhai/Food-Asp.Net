# CKFinder 3 DiskQuota Plugin Sample

This sample plugin [illustrates the usage of CKFinder events](http://docs.cksource.com/ckfinder3-net/howto.html#howto_disk_quota) to set disk storage limit per user.

Please notice this is **not** a fully functional plugin. The quota checking method `IsQuotaAvailable()` needs custom implementation.

## Configuration Options

To set the quota, add the following option to main CKFinder configuration file (usually `Web.config`):

```xml
<plugin name="DiskQuota">
  <option name="userQuota" value="209715200" /> <!-- 209715200 = 200MB -->
</plugin>
```

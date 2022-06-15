namespace CKSource.CKFinder.Connector.WebApp
{
    using System.Configuration;

    using CKSource.CKFinder.Connector.Config;
    using CKSource.CKFinder.Connector.Core.Builders;
    using CKSource.CKFinder.Connector.Host.Owin;
    using CKSource.CKFinder.Connector.KeyValue.FileSystem;
    using CKSource.FileSystem.Amazon;
    using CKSource.FileSystem.Azure;
    using CKSource.FileSystem.Dropbox;
    using CKSource.FileSystem.Ftp;
    using CKSource.FileSystem.Local;

    using Owin;

    public class ConnectorConfig
    {
        public static void RegisterFileSystems()
        {
            FileSystemFactory.RegisterFileSystem<LocalStorage>();
            FileSystemFactory.RegisterFileSystem<DropboxStorage>();
            FileSystemFactory.RegisterFileSystem<AmazonStorage>();
            FileSystemFactory.RegisterFileSystem<AzureStorage>();
            FileSystemFactory.RegisterFileSystem<FtpStorage>();
        }

        public static void SetupConnector(IAppBuilder builder)
        {
            var allowedRoleMatcherTemplate = ConfigurationManager.AppSettings["ckfinderAllowedRole"];
            var authenticator = new RoleBasedAuthenticator(allowedRoleMatcherTemplate);

            var connectorFactory = new OwinConnectorFactory();
            var connectorBuilder = new ConnectorBuilder();
            var connector = connectorBuilder
                .LoadConfig()
                .SetAuthenticator(authenticator)
                .SetRequestConfiguration(
                    (request, config) =>
                    {
                        config.LoadConfig();

                        var defaultBackend = config.GetBackend("default");
                        var keyValueStoreProvider = new FileSystemKeyValueStoreProvider(defaultBackend);
                        config.SetKeyValueStoreProvider(keyValueStoreProvider);
                    })
                .Build(connectorFactory);

            builder.UseConnector(connector);
        }
    }
}
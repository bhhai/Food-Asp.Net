/*
 * CKFinder
 * ========
 * http://cksource.com/ckfinder
 * Copyright (C) 2007-2016, CKSource - Frederico Knabben. All rights reserved.
 *
 * The software, this file and its contents are subject to the MIT License.
 * Please read the LICENSE.md file before using, installing, copying,
 * modifying or distribute this file or part of its contents.
 */

[assembly: Microsoft.Owin.OwinStartup(typeof(CKSource.CKFinder.Connector.WebApp.Startup))]

namespace CKSource.CKFinder.Connector.WebApp
{
    using System.Configuration;
    using System.Reflection;

    using CKSource.CKFinder.Connector.Core.Logs;
    using CKSource.CKFinder.Connector.Logs.NLog;

    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;

    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            LoggerManager.LoggerAdapterFactory = new NLogLoggerAdapterFactory();

            var assembly = Assembly.Load("App_Code");
            var connectorConfigType = assembly.GetType("CKSource.CKFinder.Connector.WebApp.ConnectorConfig");
            var registerFileSystemsMethod = connectorConfigType.GetMethod("RegisterFileSystems", BindingFlags.Public | BindingFlags.Static);
            registerFileSystemsMethod.Invoke(null, new object[] {});

            builder.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                AuthenticationMode = AuthenticationMode.Active
            });

            var setupConnectorMethod = connectorConfigType.GetMethod("SetupConnector", BindingFlags.Public | BindingFlags.Static);

            var route = ConfigurationManager.AppSettings["ckfinderRoute"];
            builder.Map(route, x => setupConnectorMethod.Invoke(null, new object[] { x }));
        }
    }
}

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

namespace CKSource.CKFinder.Connector.Plugin.GetFileInfo
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    using CKSource.CKFinder.Connector.Core;
    using CKSource.CKFinder.Connector.Core.Commands;
    using CKSource.CKFinder.Connector.Core.Plugins;

    [Export(typeof(IPlugin))]
    public class GetFileInfoPlugin : IPlugin
    {
        public void Initialize(IComponentResolver componentResolver, IReadOnlyDictionary<string, IReadOnlyCollection<string>> options)
        {
            var commandRepository = componentResolver.Resolve<ICommandRepository>();

            commandRepository.Add<GetFileInfoCommand>("GetFileInfo");
        }
    }
}

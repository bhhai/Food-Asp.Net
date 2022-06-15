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

namespace CKSource.CKFinder.Connector.Plugin.DiskQuota
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Threading.Tasks;

    using CKSource.CKFinder.Connector.Core;
    using CKSource.CKFinder.Connector.Core.Commands;
    using CKSource.CKFinder.Connector.Core.Events;
    using CKSource.CKFinder.Connector.Core.Events.Messages;
    using CKSource.CKFinder.Connector.Core.Exceptions;
    using CKSource.CKFinder.Connector.Core.Plugins;

    [Export(typeof(IPlugin))]
    public class DiskQuotaPlugin : IPlugin, IDisposable
    {
        private const long DefaultQuotaLimit = 100 * 1024 * 1024;

        private long _quotaLimit;

        private IEventAggregator _eventAggregator;

        private object _beforeCommandSubscription;

        public void Initialize(IComponentResolver componentResolver, IReadOnlyDictionary<string, IReadOnlyCollection<string>> options)
        {
            _quotaLimit = long.Parse(options["userQuota"].FirstOrDefault() ?? DefaultQuotaLimit.ToString());

            _eventAggregator = componentResolver.Resolve<IEventAggregator>();
            _beforeCommandSubscription = _eventAggregator.Subscribe<BeforeCommandEvent>(next => async messageContext => await OnBeforeCommand(messageContext, next));
        }

        public void Dispose()
        {
            _eventAggregator?.Unsubscribe(_beforeCommandSubscription);
        }

        private bool IsQuotaAvailable(ICommandRequest commandRequest)
        {
            /* TODO: Add custom implementation */

            return true;
        }

        private async Task OnBeforeCommand(MessageContext<BeforeCommandEvent> messageContext, EventHandlerFunc<BeforeCommandEvent> next)
        {
            if (messageContext.Message.CommandInstance is FileUploadCommand ||
                messageContext.Message.CommandInstance is CopyFilesCommand ||
                messageContext.Message.CommandInstance is ImageResizeCommand ||
                messageContext.Message.CommandInstance is CreateFolderCommand)
            {
                var commandRequest = messageContext.ComponentResolver.Resolve<ICommandRequest>();

                if (!IsQuotaAvailable(commandRequest))
                {
                    throw new CustomErrorException("Storage quota exceeded");
                }
            }

            await next(messageContext);
        }
    }
}

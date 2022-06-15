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

namespace CKSource.CKFinder.Connector.Plugin.UserActionsLogger
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;

    using CKSource.CKFinder.Connector.Core;
    using CKSource.CKFinder.Connector.Core.Events;
    using CKSource.CKFinder.Connector.Core.Events.Messages;
    using CKSource.CKFinder.Connector.Core.Logs;
    using CKSource.CKFinder.Connector.Core.Plugins;

    [Export(typeof(IPlugin))]
    public class UserActionsLoggerPlugin : IPlugin, IDisposable
    {
        private static readonly Logger Log = LoggerManager.GetLoggerForCurrentClass();

        private readonly List<object> _subscriptions = new List<object>();

        private IEventAggregator _eventAggregator;

        public void Initialize(IComponentResolver componentResolver, IReadOnlyDictionary<string, IReadOnlyCollection<string>> options)
        {
            _eventAggregator = componentResolver.Resolve<IEventAggregator>();

            _subscriptions.AddRange(new[]
            {
                _eventAggregator.Subscribe<CopyFileEvent>(next => async messageContext => await OnCopyFile(messageContext, next)),
                _eventAggregator.Subscribe<CreateFolderEvent>(next => async messageContext => await OnCreateFolder(messageContext, next)),
                _eventAggregator.Subscribe<DeleteFileEvent>(next => async messageContext => await OnDeleteFile(messageContext, next)),
                _eventAggregator.Subscribe<DeleteFolderEvent>(next => async messageContext => await OnDeleteFolder(messageContext, next)),
                _eventAggregator.Subscribe<DownloadFileEvent>(next => async messageContext => await OnDownloadFile(messageContext, next)),
                _eventAggregator.Subscribe<FileUploadEvent>(next => async messageContext => await OnFileUpload(messageContext, next)),
                _eventAggregator.Subscribe<MoveFileEvent>(next => async messageContext => await OnMoveFile(messageContext, next)),
                _eventAggregator.Subscribe<RenameFileEvent>(next => async messageContext => await OnRenameFile(messageContext, next)),
                _eventAggregator.Subscribe<RenameFolderEvent>(next => async messageContext => await OnRenameFolder(messageContext, next)),
                _eventAggregator.Subscribe<SaveImageEvent>(next => async messageContext => await OnSaveImage(messageContext, next)),
                _eventAggregator.Subscribe<ResizeImageEvent>(next => async messageContext => await OnResizeImage(messageContext, next)),
            });
        }

        public void Dispose()
        {
            if (_eventAggregator == null)
            {
                return;
            }

            foreach (var subscription in _subscriptions)
            {
                _eventAggregator.Unsubscribe(subscription);
            }

            _subscriptions.Clear();
        }

        private static async Task OnCopyFile(MessageContext<CopyFileEvent> messageContext, EventHandlerFunc<CopyFileEvent> next)
        {
            var userInfo = GetUserLogInfo(messageContext);

            Log.Info($"{userInfo} - File copy: {messageContext.Message.SourceFile} -> {messageContext.Message.DestinationFile}");

            await next(messageContext);
        }

        private static async Task OnCreateFolder(MessageContext<CreateFolderEvent> messageContext, EventHandlerFunc<CreateFolderEvent> next)
        {
            var userInfo = GetUserLogInfo(messageContext);

            Log.Info($"{userInfo} - Folder create: {messageContext.Message.Folder}");

            await next(messageContext);
        }

        private static async Task OnDeleteFile(MessageContext<DeleteFileEvent> messageContext, EventHandlerFunc<DeleteFileEvent> next)
        {
            var userInfo = GetUserLogInfo(messageContext);

            Log.Info($"{userInfo} - File delete: {messageContext.Message.File}");

            await next(messageContext);
        }

        private static async Task OnDeleteFolder(MessageContext<DeleteFolderEvent> messageContext, EventHandlerFunc<DeleteFolderEvent> next)
        {
            var userInfo = GetUserLogInfo(messageContext);

            Log.Info($"{userInfo} - Folder delete: {messageContext.Message.Folder}");

            await next(messageContext);
        }

        private static async Task OnDownloadFile(MessageContext<DownloadFileEvent> messageContext, EventHandlerFunc<DownloadFileEvent> next)
        {
            var userInfo = GetUserLogInfo(messageContext);

            Log.Info($"{userInfo} - File download: {messageContext.Message.File}");

            await next(messageContext);
        }

        private static async Task OnFileUpload(MessageContext<FileUploadEvent> messageContext, EventHandlerFunc<FileUploadEvent> next)
        {
            var userInfo = GetUserLogInfo(messageContext);

            Log.Info($"{userInfo} - File upload: {messageContext.Message.File}");

            await next(messageContext);
        }

        private static async Task OnMoveFile(MessageContext<MoveFileEvent> messageContext, EventHandlerFunc<MoveFileEvent> next)
        {
            var userInfo = GetUserLogInfo(messageContext);

            Log.Info($"{userInfo} - File move: {messageContext.Message.SourceFile} -> {messageContext.Message.DestinationFile}");

            await next(messageContext);
        }

        private static async Task OnRenameFile(MessageContext<RenameFileEvent> messageContext, EventHandlerFunc<RenameFileEvent> next)
        {
            var userInfo = GetUserLogInfo(messageContext);

            Log.Info($"{userInfo} - File rename: {messageContext.Message.SourceFile} -> {messageContext.Message.DestinationFile}");

            await next(messageContext);
        }

        private static async Task OnRenameFolder(MessageContext<RenameFolderEvent> messageContext, EventHandlerFunc<RenameFolderEvent> next)
        {
            var userInfo = GetUserLogInfo(messageContext);

            Log.Info($"{userInfo} - Folder rename: {messageContext.Message.SourceFolder} -> {messageContext.Message.DestinationFolder}");

            await next(messageContext);
        }

        private static async Task OnSaveImage(MessageContext<SaveImageEvent> messageContext, EventHandlerFunc<SaveImageEvent> next)
        {
            var userInfo = GetUserLogInfo(messageContext);

            Log.Info($"{userInfo} - Image save: {messageContext.Message.File}");

            await next(messageContext);
        }

        private static async Task OnResizeImage(MessageContext<ResizeImageEvent> messageContext, EventHandlerFunc<ResizeImageEvent> next)
        {
            var userInfo = GetUserLogInfo(messageContext);

            Log.Info($"{userInfo} - Image resize: {messageContext.Message.File}");

            await next(messageContext);
        }

        private static string GetUserLogInfo<T>(MessageContext<T> messageContext) where T : EventBase
        {
            var commandRequest = messageContext.Message.CommandRequest;
            return commandRequest.Principal?.Identity?.Name ?? "Not logged in";
        }
    }
}

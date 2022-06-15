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
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using CKSource.CKFinder.Connector.Core;
    using CKSource.CKFinder.Connector.Core.Commands;
    using CKSource.CKFinder.Connector.Core.Exceptions;
    using CKSource.CKFinder.Connector.Core.Nodes;
    using CKSource.CKFinder.Connector.Core.Resources;
    using CKSource.FileSystem;

    public class GetFileInfoCommand : ICommand
    {
        private readonly ICommandRequest _commandRequest;

        private readonly IResourceTypeRepository _resourceTypeRepository;

        private readonly INodeFactory _nodeFactory;

        private readonly INodeValidator _nodeValidator;

        public GetFileInfoCommand(
            ICommandRequest commandRequest,
            IResourceTypeRepository resourceTypeRepository,
            INodeFactory nodeFactory,
            INodeValidator nodeValidator)
        {
            _commandRequest = commandRequest;
            _resourceTypeRepository = resourceTypeRepository;
            _nodeFactory = nodeFactory;
            _nodeValidator = nodeValidator;
        }

        public async Task<CommandResponse> ExecuteAsync(CancellationToken cancellationToken)
        {
            var currentFolderName = _commandRequest.QueryParameters["currentFolder"].FirstOrDefault() ?? "/";

            var resourceTypeName = _commandRequest.QueryParameters["type"].FirstOrDefault();
            if (string.IsNullOrEmpty(resourceTypeName))
            {
                throw new InvalidRequestException();
            }

            var fileName = _commandRequest.QueryParameters["fileName"].FirstOrDefault();
            if (string.IsNullOrEmpty(fileName))
            {
                throw new InvalidRequestException();
            }

            var resourceType = _resourceTypeRepository.GetByName(resourceTypeName);

            var file = _nodeFactory.CreateFile(resourceType, Path.Combine(currentFolderName, fileName));
            await _nodeValidator.ThrowIfReadDeniedAsync(file, cancellationToken);

            var fileInfo = await file.GetFileInfoAsync(cancellationToken);

            return new JsonCommandResponse(fileInfo);
        }
    }
}
using AutoMapper;
using Cmas.BusinessLayers.TimeSheets.CommandsContexts;
using Cmas.DataLayers.Infrastructure;
using Cmas.Infrastructure.Domain.Commands;
using System;
using System.Threading.Tasks;
using Cmas.DataLayers.CouchDb.TimeSheets.Dtos;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Commands
{
    public class CreateTempAttachmentTokenCommand : ICommand<CreateTempAttachmentTokenContext>
    {
        private IMapper _autoMapper;
        private readonly CouchWrapper _couchWrapper;

        public CreateTempAttachmentTokenCommand(IServiceProvider serviceProvider)
        {
            _autoMapper = (IMapper) serviceProvider.GetService(typeof(IMapper));

            _couchWrapper = new CouchWrapper(serviceProvider, DbConsts.ServiceName);
        }

        public async Task<CreateTempAttachmentTokenContext> Execute(CreateTempAttachmentTokenContext commandContext)
        {
            var doc = new AttachmentTokenDto();

            doc.CreatedAt = DateTime.UtcNow;

            doc.FileName = commandContext.FileName;
            doc.TimeSheetId = commandContext.TimeSheetId;
            doc.Token = commandContext.Token;
            doc._id = null;

            var result = await _couchWrapper.GetResponseAsync(async (client) =>
            {
                return await client.Entities.PostAsync(doc);
            });

            return commandContext;
        }
    }
}
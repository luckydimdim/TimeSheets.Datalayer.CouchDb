using AutoMapper;
using Cmas.BusinessLayers.TimeSheets.CommandsContexts;
using Cmas.DataLayers.Infrastructure;
using Cmas.Infrastructure.Domain.Commands;
using System;
using System.Threading.Tasks;
using MyCouch.Requests;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Commands
{
    public class AddAttachmentCommand : ICommand<AddAttachmentCommandContext>
    {
        private IMapper _autoMapper;
        private readonly CouchWrapper _couchWrapper;

        public AddAttachmentCommand(IServiceProvider serviceProvider)
        {
            _autoMapper = (IMapper) serviceProvider.GetService(typeof(IMapper));

            _couchWrapper = new CouchWrapper(serviceProvider, DbConsts.ServiceName);
        }

        public async Task<AddAttachmentCommandContext> Execute(AddAttachmentCommandContext commandContext)
        {
            var result = await _couchWrapper.GetResponseAsync(async (client) =>
            {
                var request = new PutAttachmentRequest(commandContext.Id, commandContext.RevId, commandContext.Name,
                    commandContext.ContentType, commandContext.Content);

                return await client.Attachments.PutAsync(request);
            });

            commandContext.Id = result.Id;

            return commandContext;
        }
    }
}
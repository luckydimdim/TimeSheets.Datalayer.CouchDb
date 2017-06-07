using Cmas.BusinessLayers.TimeSheets.CommandsContexts;
using Cmas.DataLayers.Infrastructure;
using Cmas.Infrastructure.Domain.Commands;
using System;
using System.Threading.Tasks;
using MyCouch.Requests;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Commands
{
    public class DeleteAttachmentCommand : ICommand<DeleteAttachmentCommandContext>
    {
     
        private readonly CouchWrapper _couchWrapper;

        public DeleteAttachmentCommand(IServiceProvider serviceProvider)
        {
            _couchWrapper = new CouchWrapper(serviceProvider, DbConsts.ServiceName);
        }

        public async Task<DeleteAttachmentCommandContext> Execute(DeleteAttachmentCommandContext commandContext)
        {
            var result = await _couchWrapper.GetResponseAsync(async (client) =>
            {
                var request = new DeleteAttachmentRequest(commandContext.Id, commandContext.RevId, commandContext.Name);

                return await client.Attachments.DeleteAsync(request);
            }); 

            return commandContext;
        }
    }
}
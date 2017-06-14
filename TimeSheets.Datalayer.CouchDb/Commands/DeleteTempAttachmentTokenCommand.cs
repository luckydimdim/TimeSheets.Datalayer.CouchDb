using System.Threading.Tasks;
using Cmas.Infrastructure.Domain.Commands;
using Cmas.BusinessLayers.TimeSheets.CommandsContexts;
using Cmas.DataLayers.Infrastructure;
using System;
using CouchRequest = MyCouch.Requests;
using Cmas.DataLayers.CouchDb.TimeSheets.Dtos;
using System.Linq;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Commands
{ 
    public class DeleteTempAttachmentTokenCommand : ICommand<DeleteTempAttachmentTokenContext>
    {
        private readonly CouchWrapper _couchWrapper;

        public DeleteTempAttachmentTokenCommand(IServiceProvider serviceProvider)
        {
            _couchWrapper = new CouchWrapper(serviceProvider, DbConsts.ServiceName);
        }

        public async Task<DeleteTempAttachmentTokenContext> Execute(DeleteTempAttachmentTokenContext commandContext)
        {

            var query =
                new CouchRequest.QueryViewRequest(DbConsts.DesignDocumentName, DbConsts.AttachmentTokensViewName)
                    .Configure(q => q.Key(commandContext.Token));

            var viewResult = await _couchWrapper.GetResponseAsync(async (client) =>
            {
                return await client.Views.QueryAsync<AttachmentTokenViewDto>(query);
            });

            if (viewResult.Rows.Length == 0)
                return commandContext;

            var row = viewResult.Rows.FirstOrDefault();
            
            var header = await _couchWrapper.GetHeaderAsync(row.Id);
            
            var result = await _couchWrapper.GetResponseAsync(async (client) =>
            {
                return await client.Documents.DeleteAsync(header.Id, header.Rev);
            });

            return commandContext;
        }
    }

}

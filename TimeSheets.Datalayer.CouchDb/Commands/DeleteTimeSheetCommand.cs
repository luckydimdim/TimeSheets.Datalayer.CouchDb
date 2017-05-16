using System.Threading.Tasks;
using Cmas.Infrastructure.Domain.Commands;
using Cmas.BusinessLayers.TimeSheets.CommandsContexts;
using Cmas.DataLayers.Infrastructure;
using System;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Commands
{
    public class DeleteTimeSheetCommand : ICommand<DeleteTimeSheetCommandContext>
    { 
        private readonly CouchWrapper _couchWrapper;

        public DeleteTimeSheetCommand(IServiceProvider serviceProvider)
        { 

            _couchWrapper = new CouchWrapper(serviceProvider, DbConsts.ServiceName);
        }

        public async Task<DeleteTimeSheetCommandContext> Execute(DeleteTimeSheetCommandContext commandContext)
        {
            var header = await _couchWrapper.GetHeaderAsync(commandContext.Id);

            var result = await _couchWrapper.GetResponseAsync(async (client) =>
            {
                return await client.Documents.DeleteAsync(commandContext.Id, header.Rev);
            });

            return commandContext;
        }
    }
}
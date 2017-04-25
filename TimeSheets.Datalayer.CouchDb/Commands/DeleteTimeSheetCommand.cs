using System.Threading.Tasks;
using Cmas.Infrastructure.Domain.Commands;
using Cmas.BusinessLayers.TimeSheets.CommandsContexts;
using Microsoft.Extensions.Logging;
using Cmas.DataLayers.Infrastructure;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Commands
{
    public class DeleteTimeSheetCommand : ICommand<DeleteTimeSheetCommandContext>
    {
        private readonly ILogger _logger;
        private readonly CouchWrapper _couchWrapper;

        public DeleteTimeSheetCommand(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DeleteTimeSheetCommand>();
            _couchWrapper = new CouchWrapper(DbConsts.DbConnectionString, DbConsts.DbName, _logger);
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
using System.Threading.Tasks;
using AutoMapper;
using Cmas.DataLayers.CouchDb.TimeSheets.Dtos;
using Cmas.Infrastructure.Domain.Commands;
using Cmas.BusinessLayers.TimeSheets.CommandsContexts;
using Cmas.DataLayers.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Commands
{
    public class UpdateTimeSheetCommand : ICommand<UpdateTimeSheetCommandContext>
    {
        private IMapper _autoMapper;
        private readonly ILogger _logger;
        private readonly CouchWrapper _couchWrapper;

        public UpdateTimeSheetCommand(IMapper autoMapper, ILoggerFactory loggerFactory)
        {
            _autoMapper = autoMapper;
            _logger = loggerFactory.CreateLogger<UpdateTimeSheetCommand>();
            _couchWrapper = new CouchWrapper(DbConsts.DbConnectionString, DbConsts.DbName, _logger);
        }

        public async Task<UpdateTimeSheetCommandContext> Execute(UpdateTimeSheetCommandContext commandContext)
        {
            // FIXME: нельзя так делать, надо от frontend получать Rev
            var header = await _couchWrapper.GetHeaderAsync(commandContext.TimeSheet.Id);

            var entity = _autoMapper.Map<TimeSheetDto>(commandContext.TimeSheet);

            entity._rev = header.Rev;

            var result = await _couchWrapper.GetResponseAsync(async (client) =>
            {
                return await client.Entities.PutAsync(entity._id, entity);
            });

            return commandContext; // TODO: возвращать _revid
        }
    }
}
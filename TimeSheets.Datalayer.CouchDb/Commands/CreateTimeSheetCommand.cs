using System.Threading.Tasks;
using AutoMapper;
using Cmas.DataLayers.CouchDb.TimeSheets.Dtos;
using Cmas.Infrastructure.Domain.Commands;
using Cmas.BusinessLayers.TimeSheets.CommandsContexts;
using Cmas.DataLayers.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Commands
{
    public class CreateTimeSheetCommand : ICommand<CreateTimeSheetCommandContext>
    {
        private IMapper _autoMapper;
        private readonly ILogger _logger;
        private readonly CouchWrapper _couchWrapper;

        public CreateTimeSheetCommand(IMapper autoMapper, ILoggerFactory loggerFactory)
        {
            _autoMapper = autoMapper;
            _logger = loggerFactory.CreateLogger<CreateTimeSheetCommand>();
            _couchWrapper = new CouchWrapper(DbConsts.DbConnectionString, DbConsts.DbName, _logger);
        }

        public async Task<CreateTimeSheetCommandContext> Execute(CreateTimeSheetCommandContext commandContext)
        {
            var doc = _autoMapper.Map<TimeSheetDto>(commandContext.TimeSheet);

            var result = await _couchWrapper.GetResponseAsync(async (client) =>
            {
                return await client.Entities.PostAsync(doc);
            });

            commandContext.Id = result.Id;

            return commandContext;
        }
    }
}
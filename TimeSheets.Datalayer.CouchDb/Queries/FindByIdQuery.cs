using System.Threading.Tasks;
using AutoMapper;
using Cmas.BusinessLayers.TimeSheets.Entities;
using Cmas.DataLayers.CouchDb.TimeSheets.Dtos;
using Cmas.DataLayers.Infrastructure;
using Cmas.Infrastructure.Domain.Criteria;
using Cmas.Infrastructure.Domain.Queries;
using Microsoft.Extensions.Logging;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Queries
{
    public class FindByIdQuery : IQuery<FindById, Task<TimeSheet>>
    {
        private readonly IMapper _autoMapper;
        private readonly ILogger _logger;
        private readonly CouchWrapper _couchWrapper;

        public FindByIdQuery(IMapper autoMapper, ILoggerFactory loggerFactory)
        {
            _autoMapper = autoMapper;
            _logger = loggerFactory.CreateLogger<FindByIdQuery>();
            _couchWrapper = new CouchWrapper(DbConsts.DbConnectionString, DbConsts.DbName, _logger);
        }

        public async Task<TimeSheet> Ask(FindById criterion)
        {
            var result = await _couchWrapper.GetResponseAsync(async (client) =>
            {
                return await client.Entities.GetAsync<TimeSheetDto>(criterion.Id);
            });

            return _autoMapper.Map<TimeSheet>(result.Content);
        }
    }
}
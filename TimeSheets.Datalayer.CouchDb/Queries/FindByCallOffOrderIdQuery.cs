using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cmas.DataLayers.CouchDb.TimeSheets.Dtos;
using Cmas.Infrastructure.Domain.Queries;
using CouchRequest = MyCouch.Requests;
using Cmas.BusinessLayers.TimeSheets.Criteria;
using Cmas.BusinessLayers.TimeSheets.Entities;
using Microsoft.Extensions.Logging;
using Cmas.DataLayers.Infrastructure;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Queries
{
    public class FindByCallOffOrderIdQuery : IQuery<FindByCallOffOrderId, Task<IEnumerable<TimeSheet>>>
    {
        private IMapper _autoMapper;
        private readonly ILogger _logger;
        private readonly CouchWrapper _couchWrapper;

        public FindByCallOffOrderIdQuery(IMapper autoMapper, ILoggerFactory loggerFactory)
        {
            _autoMapper = autoMapper;
            _logger = loggerFactory.CreateLogger<FindByCallOffOrderIdQuery>();
            _couchWrapper = new CouchWrapper(DbConsts.DbConnectionString, DbConsts.DbName, _logger);
        }

        public async Task<IEnumerable<TimeSheet>> Ask(FindByCallOffOrderId criterion)
        {
            var result = new List<TimeSheet>();
            var query =
                new CouchRequest.QueryViewRequest(DbConsts.DesignDocumentName, DbConsts.ByCallOffOrderDocsViewName)
                    .Configure(q => q.Key(criterion.CallOffOrderId));

            var viewResult = await _couchWrapper.GetResponseAsync(async (client) =>
            {
                return await client.Views.QueryAsync<TimeSheetDto>(query);
            });

            foreach (var row in viewResult.Rows.OrderByDescending(s => s.Value.CreatedAt))
            {
                result.Add(_autoMapper.Map<TimeSheet>(row.Value));
            }

            return result;
        }
    }
}
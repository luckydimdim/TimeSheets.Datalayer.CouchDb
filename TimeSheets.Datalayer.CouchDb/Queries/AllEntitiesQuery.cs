﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cmas.Infrastructure.Domain.Criteria;
using Cmas.Infrastructure.Domain.Queries;
using MyCouch.Requests;
using Cmas.BusinessLayers.TimeSheets.Entities;
using Cmas.DataLayers.CouchDb.TimeSheets.Dtos;
using Cmas.DataLayers.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Queries
{
    public class AllEntitiesQuery : IQuery<AllEntities, Task<IEnumerable<TimeSheet>>>
    {
        private IMapper _autoMapper;
        private readonly ILogger _logger;
        private readonly CouchWrapper _couchWrapper;

        public AllEntitiesQuery(IMapper autoMapper, ILoggerFactory loggerFactory)
        {
            _autoMapper = autoMapper;
            _logger = loggerFactory.CreateLogger<AllEntitiesQuery>();
            _couchWrapper = new CouchWrapper(DbConsts.DbConnectionString, DbConsts.DbName, _logger);
        }

        public async Task<IEnumerable<TimeSheet>> Ask(AllEntities criterion)
        {
            var result = new List<TimeSheet>();

            var query = new QueryViewRequest(DbConsts.DesignDocumentName, DbConsts.AllDocsViewName);

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

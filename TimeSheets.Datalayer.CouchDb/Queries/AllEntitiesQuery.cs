using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cmas.Infrastructure.Domain.Criteria;
using Cmas.Infrastructure.Domain.Queries;
using MyCouch;
using MyCouch.Requests;
using Cmas.BusinessLayers.TimeSheets.Entities;
using Cmas.DataLayers.CouchDb.TimeSheets.Dtos;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Queries
{
    public class AllEntitiesQuery : IQuery<AllEntities, Task<IEnumerable<TimeSheet>>>
    {
        private IMapper _autoMapper;

        public AllEntitiesQuery(IMapper autoMapper)
        {
            _autoMapper = autoMapper;
        }

        public async Task<IEnumerable<TimeSheet>> Ask(AllEntities criterion)
        {
            using (var client = new MyCouchClient(DbConsts.DbConnectionString, DbConsts.DbName))
            {
                var result = new List<TimeSheet>();

                var query = new QueryViewRequest(DbConsts.DesignDocumentName, DbConsts.AllDocsViewName);

                var viewResult = await client.Views.QueryAsync<TimeSheetDto>(query);

                if (!viewResult.IsSuccess)
                {
                    throw new Exception(viewResult.Error);
                }

                foreach (var row in viewResult.Rows.OrderByDescending(s => s.Value.CreatedAt))
                {
                    result.Add(_autoMapper.Map<TimeSheet>(row.Value));
                }

                return result;
            }
        }
    }
}

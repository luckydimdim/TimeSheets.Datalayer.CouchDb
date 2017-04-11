using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper; 
using Cmas.DataLayers.CouchDb.TimeSheets.Dtos;
using Cmas.Infrastructure.Domain.Queries;
using MyCouch;
using CouchRequest = MyCouch.Requests;
using Cmas.BusinessLayers.TimeSheets.Criteria;
using Cmas.BusinessLayers.TimeSheets.Entities;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Queries
{
    public class FindByCallOffOrderIdQuery : IQuery<FindByCallOffOrderId, Task<IEnumerable<TimeSheet>>>
    {
        private IMapper _autoMapper;

        public FindByCallOffOrderIdQuery(IMapper autoMapper)
        {
            _autoMapper = autoMapper;
        }

        public async Task<IEnumerable<TimeSheet>> Ask(FindByCallOffOrderId criterion)
        {
            using (var client = new MyCouchClient(DbConsts.DbConnectionString, DbConsts.DbName))
            {

                var result = new List<TimeSheet>();

                var query = new CouchRequest.QueryViewRequest(DbConsts.DesignDocumentName, DbConsts.ByCallOffOrderDocsViewName).Configure(q => q.Key(criterion.CallOffOrderId));

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

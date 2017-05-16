using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cmas.DataLayers.CouchDb.TimeSheets.Dtos;
using Cmas.Infrastructure.Domain.Queries;
using CouchRequest = MyCouch.Requests;
using Cmas.BusinessLayers.TimeSheets.Criteria;
using Cmas.BusinessLayers.TimeSheets.Entities;
using Cmas.DataLayers.Infrastructure;
using System;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Queries
{
    public class FindByCallOffOrderIdQuery : IQuery<FindByCallOffOrderId, Task<IEnumerable<TimeSheet>>>
    {
        private readonly IMapper _autoMapper;
        private readonly CouchWrapper _couchWrapper;

        public FindByCallOffOrderIdQuery(IServiceProvider serviceProvider)
        {
            _autoMapper = (IMapper)serviceProvider.GetService(typeof(IMapper));

            _couchWrapper = new CouchWrapper(serviceProvider, DbConsts.ServiceName);
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
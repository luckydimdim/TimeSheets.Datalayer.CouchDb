using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cmas.Infrastructure.Domain.Queries;
using CouchRequest = MyCouch.Requests;
using Cmas.BusinessLayers.TimeSheets.Criteria;
using Cmas.DataLayers.Infrastructure;
using Cmas.BusinessLayers.TimeSheets.Entities;
using Cmas.DataLayers.CouchDb.TimeSheets.Dtos;
using System;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Queries
{
    public class FindByRequestIdQuery : IQuery<FindByRequestId, Task<IEnumerable<TimeSheet>>>
    {
        private readonly IMapper _autoMapper;
        private readonly CouchWrapper _couchWrapper;

        public FindByRequestIdQuery(IServiceProvider serviceProvider)
        {
            _autoMapper = (IMapper)serviceProvider.GetService(typeof(IMapper));

            _couchWrapper = new CouchWrapper(serviceProvider, DbConsts.ServiceName);
        }

        public async Task<IEnumerable<TimeSheet>> Ask(FindByRequestId criterion)
        {
            var result = new List<TimeSheet>();

            var query =
                new CouchRequest.QueryViewRequest(DbConsts.DesignDocumentName, DbConsts.ByRequestIdDocsViewName)
                    .Configure(q => q.Key(criterion.RequestId));

            var viewResult = await _couchWrapper.GetResponseAsync(async (client) =>
            {
                return await client.Views.QueryAsync<TimeSheetDto>(query);
            });

            foreach (var row in viewResult.Rows.OrderBy(t => t.Value._id))
            {
                result.Add(_autoMapper.Map<TimeSheet>(row.Value));
            }

            return result;
        }
    }
}
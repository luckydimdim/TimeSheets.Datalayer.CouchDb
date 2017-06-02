using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cmas.Infrastructure.Domain.Queries;
using CouchRequest = MyCouch.Requests;
using Cmas.BusinessLayers.TimeSheets.Criteria;
using Cmas.BusinessLayers.TimeSheets.Entities;
using Cmas.DataLayers.Infrastructure;
using System;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Queries
{
    public class CountByCallOffOrderIdQuery : IQuery<FindByCallOffOrderId, Task<int>>
    {
        private readonly CouchWrapper _couchWrapper;

        public CountByCallOffOrderIdQuery(IServiceProvider serviceProvider)
        {
            _couchWrapper = new CouchWrapper(serviceProvider, DbConsts.ServiceName);
        }

        public async Task<int> Ask(FindByCallOffOrderId criterion)
        {
            var result = new List<TimeSheet>();
            var query =
                new CouchRequest.QueryViewRequest(DbConsts.DesignDocumentName, DbConsts.ByCallOffOrderDocsViewName)
                    .Configure(q => q.Key(criterion.CallOffOrderId).Reduce(true));

            var viewResult = await _couchWrapper.GetResponseAsync(async (client) =>
            {
                return await client.Views.QueryAsync<int>(query);
            });

            var row = viewResult.Rows.FirstOrDefault();

            if (row == null)
                return 0;
            else
            {
                return row.Value;
            }
        }
    }
}
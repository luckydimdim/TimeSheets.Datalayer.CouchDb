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
    /// <summary>
    /// Получить ID табелей по заявке
    /// </summary>
    public class IdsByRequest : IQuery<FindByRequestId, Task<IEnumerable<string>>>
    {
        private readonly IMapper _autoMapper;
        private readonly CouchWrapper _couchWrapper;

        public IdsByRequest(IServiceProvider serviceProvider)
        {
            _autoMapper = (IMapper)serviceProvider.GetService(typeof(IMapper));

            _couchWrapper = new CouchWrapper(serviceProvider, DbConsts.ServiceName);
        }

        public async Task<IEnumerable<string>> Ask(FindByRequestId criterion)
        {
            var result = new List<string>();

            var query =
                new CouchRequest.QueryViewRequest(DbConsts.DesignDocumentName, DbConsts.IdsByRequestDocsViewName)
                    .Configure(q => q.Key(criterion.RequestId));

            query.IncludeDocs = false;

            var viewResult = await _couchWrapper.GetResponseAsync(async (client) =>
            {
                return await client.Views.QueryAsync(query);
            });

            foreach (var row in viewResult.Rows)
            {
                result.Add(row.Id);
            }

            return result;
        }
    }
}
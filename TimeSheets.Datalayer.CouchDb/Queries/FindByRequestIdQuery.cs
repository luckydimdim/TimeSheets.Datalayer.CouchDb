using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Cmas.Infrastructure.Domain.Queries;
using MyCouch;
using CouchRequest = MyCouch.Requests;
using Cmas.BusinessLayers.TimeSheets.Criteria;
using Cmas.Infrastructure.ErrorHandler;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Queries
{
    public class FindByRequestIdQuery : IQuery<FindByRequestId, Task<IEnumerable<string>>>
    {
        private IMapper _autoMapper;

        public FindByRequestIdQuery(IMapper autoMapper)
        {
            _autoMapper = autoMapper;
        }

        public async Task<IEnumerable<string>> Ask(FindByRequestId criterion)
        {
            var result = new List<string>();

            using (var client = new MyCouchClient(DbConsts.DbConnectionString, DbConsts.DbName))
            {
                var query =
                    new CouchRequest.QueryViewRequest(DbConsts.DesignDocumentName, DbConsts.ByRequestIdDocsViewName)
                        .Configure(q => q.Key(criterion.RequestId));

                var viewResult = await client.Views.QueryAsync(query);

                if (!viewResult.IsSuccess)
                {
                    if (viewResult.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new NotFoundErrorException(viewResult.ToStringDebugVersion());
                    }

                    throw new Exception(viewResult.ToStringDebugVersion());
                }


                foreach (var row in viewResult.Rows)
                {
                    result.Add(row.Id);
                }
            }

            return result;
        }
    }
}

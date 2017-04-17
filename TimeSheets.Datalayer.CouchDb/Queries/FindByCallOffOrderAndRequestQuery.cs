using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Cmas.DataLayers.CouchDb.TimeSheets.Dtos;
using Cmas.Infrastructure.Domain.Queries;
using MyCouch;
using CouchRequest = MyCouch.Requests;
using Cmas.BusinessLayers.TimeSheets.Criteria;
using Cmas.BusinessLayers.TimeSheets.Entities;
using Cmas.Infrastructure.ErrorHandler;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Queries
{
    public class FindByCallOffOrderAndRequestQuery : IQuery<FindByCallOffOrderAndRequest, Task<TimeSheet>>
    {
        private IMapper _autoMapper;

        public FindByCallOffOrderAndRequestQuery(IMapper autoMapper)
        {
            _autoMapper = autoMapper;
        }

        public async Task<TimeSheet> Ask(FindByCallOffOrderAndRequest criterion)
        {
            using (var client = new MyCouchClient(DbConsts.DbConnectionString, DbConsts.DbName))
            {
                var query =
                    new CouchRequest.QueryViewRequest(DbConsts.DesignDocumentName, DbConsts.ByCallOffOrderDocsViewName)
                        .Configure(q => q.Keys(new string[] {criterion.CallOffOrderId, criterion.RequestId}));

                var viewResult = await client.Views.QueryAsync<TimeSheetDto>(query);

                if (!viewResult.IsSuccess)
                {
                    if (viewResult.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new NotFoundErrorException(viewResult.ToStringDebugVersion());
                    }

                    throw new Exception(viewResult.ToStringDebugVersion());
                }

                if (viewResult.Rows.Length > 1)
                    throw new Exception("One request and call-off order can not have more than one time sheet " + viewResult.ToStringDebugVersion());

                if (viewResult.Rows.Length <= 0)
                    throw new NotFoundErrorException(viewResult.ToStringDebugVersion());

                return _autoMapper.Map<TimeSheet>(viewResult.Rows.First().Value);
            }
        }
    }
}
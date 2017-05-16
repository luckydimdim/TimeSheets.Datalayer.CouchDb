using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cmas.DataLayers.CouchDb.TimeSheets.Dtos;
using Cmas.Infrastructure.Domain.Queries;
using CouchRequest = MyCouch.Requests;
using Cmas.BusinessLayers.TimeSheets.Criteria;
using Cmas.BusinessLayers.TimeSheets.Entities;
using Cmas.DataLayers.Infrastructure;
using Cmas.Infrastructure.ErrorHandler;
using Microsoft.Extensions.Logging;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Queries
{
    public class FindByCallOffOrderAndRequestQuery : IQuery<FindByCallOffOrderAndRequest, Task<TimeSheet>>
    {
        private readonly IMapper _autoMapper;
        private readonly CouchWrapper _couchWrapper;

        public FindByCallOffOrderAndRequestQuery(IServiceProvider serviceProvider)
        {
            _autoMapper = (IMapper)serviceProvider.GetService(typeof(IMapper));

            _couchWrapper = new CouchWrapper(serviceProvider, DbConsts.ServiceName);
        }

        public async Task<TimeSheet> Ask(FindByCallOffOrderAndRequest criterion)
        {
            var query =
                new CouchRequest.QueryViewRequest(DbConsts.DesignDocumentName,
                        DbConsts.ByCallOffOrderAndRequestDocsViewName)
                    .Configure(q => q.Key(new string[] {criterion.CallOffOrderId, criterion.RequestId}));

            var viewResult = await _couchWrapper.GetResponseAsync(async (client) =>
            {
                return await client.Views.QueryAsync<TimeSheetDto>(query);
            });

            if (viewResult.Rows.Length > 1)
                throw new Exception(String.Format(
                    "One request {0} and call-off order {1} can not have more than one time sheet", criterion.RequestId,
                    criterion.CallOffOrderId));

            if (viewResult.Rows.Length <= 0)
                throw new NotFoundErrorException();

            return _autoMapper.Map<TimeSheet>(viewResult.Rows.First().Value);
        }
    }
}
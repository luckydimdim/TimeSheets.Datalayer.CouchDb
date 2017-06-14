using System.Threading.Tasks;
using AutoMapper;
using Cmas.DataLayers.CouchDb.TimeSheets.Dtos;
using Cmas.DataLayers.Infrastructure;
using Cmas.Infrastructure.Domain.Queries;
using System;
using System.Linq;
using Cmas.BusinessLayers.TimeSheets.Criteria;
using CouchRequest = MyCouch.Requests;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Queries
{
    public class IsTempAttachmentTokenValidQuery : IQuery<IsTempAttachmentTokenValid, Task<bool>>
    {
        private readonly IMapper _autoMapper;
        private readonly CouchWrapper _couchWrapper;

        public IsTempAttachmentTokenValidQuery(IServiceProvider serviceProvider)
        {
            _autoMapper = (IMapper) serviceProvider.GetService(typeof(IMapper));

            _couchWrapper = new CouchWrapper(serviceProvider, DbConsts.ServiceName);
        }

        public async Task<bool> Ask(IsTempAttachmentTokenValid criterion)
        {
            var query =
                new CouchRequest.QueryViewRequest(DbConsts.DesignDocumentName, DbConsts.AttachmentTokensViewName)
                    .Configure(q => q.Key(criterion.Token));

            var viewResult = await _couchWrapper.GetResponseAsync(async (client) =>
            {
                return await client.Views.QueryAsync<AttachmentTokenViewDto>(query);
            });

            if (viewResult.Rows.Length == 0)
                return false;

            var value = viewResult.Rows.FirstOrDefault().Value;

            if (value.FileName.Equals(criterion.FileName, StringComparison.OrdinalIgnoreCase) &&
                value.TimeSheetId.Equals(criterion.TimeSheetId, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }
    }
}
using System.Threading.Tasks;
using Cmas.DataLayers.Infrastructure;
using Cmas.Infrastructure.Domain.Queries;
using System;
using Cmas.BusinessLayers.TimeSheets.Criteria;
using Cmas.BusinessLayers.TimeSheets.Entities;
using CouchRequest = MyCouch.Requests;
using System.Collections.Generic;
using System.Linq;
using Cmas.DataLayers.CouchDb.TimeSheets.Dtos;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Queries
{
    public class GetAttachmentsQuery : IQuery<GetAttachments, Task<Attachment[]>>
    {
        private readonly CouchWrapper _couchWrapper;

        public GetAttachmentsQuery(IServiceProvider serviceProvider)
        {
            _couchWrapper = new CouchWrapper(serviceProvider, DbConsts.ServiceName);
        }

        public async Task<Attachment[]> Ask(GetAttachments criterion)
        {
            var result = new List<Attachment>();

            var query =
                new CouchRequest.QueryViewRequest(DbConsts.DesignDocumentName, DbConsts.AttachmentsViewName)
                    .Configure(q => q.Key(criterion.Id));

            var viewResult = await _couchWrapper.GetResponseAsync(async (client) =>
            {
                return await client.Views.QueryAsync<Dictionary<string, AttachmentDto>>(query);
            });

            if (viewResult == null || viewResult.RowCount <= 0)
                return result.ToArray();

            Dictionary<string, AttachmentDto> attachmentsDto = viewResult.Rows.First().Value;

            foreach (var fileName in attachmentsDto.Keys)
            {
                var attachment = attachmentsDto[fileName];

                result.Add(new Attachment
                {
                    Name = fileName,
                    Length = attachment.Length,
                    Content_type = attachment.Content_type
                });
            }

            return result.ToArray();
        }
    }
}
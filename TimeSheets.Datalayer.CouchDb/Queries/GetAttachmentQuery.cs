using System.Threading.Tasks;
using Cmas.DataLayers.Infrastructure;
using Cmas.Infrastructure.Domain.Queries;
using System;
using Cmas.BusinessLayers.TimeSheets.Criteria;
using Cmas.BusinessLayers.TimeSheets.Entities;
using MyCouch.Requests;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Queries
{
    public class GetAttachmentQuery : IQuery<GetAttachment, Task<Attachment>>
    {
        private readonly CouchWrapper _couchWrapper;

        public GetAttachmentQuery(IServiceProvider serviceProvider)
        {
            _couchWrapper = new CouchWrapper(serviceProvider, DbConsts.ServiceName);
        }

        public async Task<Attachment> Ask(GetAttachment criterion)
        {
            var result = await _couchWrapper.GetResponseAsync(async (client) =>
            {
                var request = new GetAttachmentRequest(criterion.Id, criterion.RevId, criterion.Name);

                return await client.Attachments.GetAsync(request);
            });

            if (result == null)
                return null;

            var attachment = new Attachment();

            attachment.Content_type = result.ContentType;
            attachment.Data = result.Content;
            attachment.Name = criterion.Name;

            return attachment;
        }
    }
}
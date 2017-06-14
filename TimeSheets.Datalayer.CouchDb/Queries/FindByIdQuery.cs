using System.Threading.Tasks;
using AutoMapper;
using Cmas.BusinessLayers.TimeSheets.Entities;
using Cmas.DataLayers.CouchDb.TimeSheets.Dtos;
using Cmas.DataLayers.Infrastructure;
using Cmas.Infrastructure.Domain.Criteria;
using Cmas.Infrastructure.Domain.Queries;
using System;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Queries
{
    public class FindByIdQuery : IQuery<FindById, Task<TimeSheet>>
    {
        private readonly IMapper _autoMapper;
        private readonly CouchWrapper _couchWrapper;

        public FindByIdQuery(IServiceProvider serviceProvider)
        {
            _autoMapper = (IMapper)serviceProvider.GetService(typeof(IMapper));

            _couchWrapper = new CouchWrapper(serviceProvider, DbConsts.ServiceName);
        }

        public async Task<TimeSheet> Ask(FindById criterion)
        {
            var result = await _couchWrapper.GetResponseAsync(async (client) =>
            {
                return await client.Entities.GetAsync<TimeSheetDto>(criterion.Id);
            });

            if (result == null)
                return null;

            return _autoMapper.Map<TimeSheet>(result.Content);
        }
    }
}
using System;
using System.Threading.Tasks;
using AutoMapper;
using Cmas.BusinessLayers.TimeSheets.Entities;
using Cmas.DataLayers.CouchDb.TimeSheets.Dtos;
using Cmas.Infrastructure.Domain.Criteria;
using Cmas.Infrastructure.Domain.Queries;
using MyCouch;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Queries
{
    public class FindByIdQuery : IQuery<FindById, Task<TimeSheet>>
    {
        private IMapper _autoMapper;

        public FindByIdQuery(IMapper autoMapper)
        {
            _autoMapper = autoMapper;
        }

        public async Task<TimeSheet> Ask(FindById criterion)
        {
            using (var client = new MyCouchClient(DbConsts.DbConnectionString, DbConsts.DbName))
            {
                var result = await client.Entities.GetAsync<TimeSheetDto>(criterion.Id);

                if (!result.IsSuccess)
                {
                    throw new Exception(result.Error);
                }

                return _autoMapper.Map<TimeSheet>(result.Content);
            }

        }
    }
}

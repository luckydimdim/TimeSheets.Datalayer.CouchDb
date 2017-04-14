using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Cmas.BusinessLayers.TimeSheets.Entities;
using Cmas.DataLayers.CouchDb.TimeSheets.Dtos;
using Cmas.Infrastructure.Domain.Criteria;
using Cmas.Infrastructure.Domain.Queries;
using Cmas.Infrastructure.ErrorHandler;
using MyCouch;
using MyCouch.Responses;

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
                GetEntityResponse<TimeSheetDto> result = await client.Entities.GetAsync<TimeSheetDto>(criterion.Id);
            
                if (!result.IsSuccess)
                {
                    if (result.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new NotFoundErrorException(result.ToStringDebugVersion());
                    }

                    throw new Exception(result.ToStringDebugVersion());
                }

                return _autoMapper.Map<TimeSheet>(result.Content);
            }

        }
    }
}

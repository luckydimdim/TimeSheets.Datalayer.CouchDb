using System;
using System.Threading.Tasks;
using AutoMapper;
using Cmas.DataLayers.CouchDb.TimeSheets.Dtos;
using Cmas.Infrastructure.Domain.Commands;
using MyCouch;
using Cmas.BusinessLayers.TimeSheets.CommandsContexts;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Commands
{
 
    public class UpdateTimeSheetCommand : ICommand<UpdateTimeSheetCommandContext>
    {

        private IMapper _autoMapper;

        public UpdateTimeSheetCommand(IMapper autoMapper)
        {
            _autoMapper = autoMapper;
        }

        public async Task<UpdateTimeSheetCommandContext> Execute(UpdateTimeSheetCommandContext commandContext)
        {
            using (var client = new MyCouchClient(DbConsts.DbConnectionString, DbConsts.DbName))
            {
                // FIXME: нельзя так делать, надо от frontend получать
                var existingDoc = (await client.Entities.GetAsync<TimeSheetDto>(commandContext.TimeSheet.Id)).Content;
 
                var newDto = _autoMapper.Map<TimeSheetDto>(commandContext.TimeSheet);
                newDto._id = existingDoc._id;
                newDto._rev = existingDoc._rev;

                var result = await client.Entities.PutAsync(newDto._id, newDto);

                if (!result.IsSuccess)
                {
                    throw new Exception(result.Error);
                }

                // TODO: возвращать _revid

                return commandContext;
            }

        }
    }
}

using System;
using System.Threading.Tasks;
using AutoMapper;
using Cmas.DataLayers.CouchDb.TimeSheets.Dtos;
using Cmas.Infrastructure.Domain.Commands;
using MyCouch;
using Cmas.BusinessLayers.TimeSheets.CommandsContexts;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Commands
{
    public class CreateTimeSheetCommand : ICommand<CreateTimeSheetCommandContext>
    {
        private IMapper _autoMapper;

        public CreateTimeSheetCommand(IMapper autoMapper)
        {
            _autoMapper = autoMapper;
        }

        public async Task<CreateTimeSheetCommandContext> Execute(CreateTimeSheetCommandContext commandContext)
        {
            using (var store = new MyCouchStore(DbConsts.DbConnectionString, DbConsts.DbName))
            {
                var doc = _autoMapper.Map<TimeSheetDto>(commandContext.TimeSheet);

                doc._id = null;
                doc._rev = null;

                var result = await store.Client.Entities.PostAsync(doc);

                if (!result.IsSuccess)
                {
                    throw new Exception(result.Error);
                }

                commandContext.Id = result.Id;


                return commandContext;
            }

        }
    }
}

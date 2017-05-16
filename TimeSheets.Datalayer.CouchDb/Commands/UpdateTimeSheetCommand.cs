using System.Threading.Tasks;
using AutoMapper;
using Cmas.DataLayers.CouchDb.TimeSheets.Dtos;
using Cmas.Infrastructure.Domain.Commands;
using Cmas.BusinessLayers.TimeSheets.CommandsContexts;
using Cmas.DataLayers.Infrastructure;
using System;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Commands
{
    public class UpdateTimeSheetCommand : ICommand<UpdateTimeSheetCommandContext>
    {
        private readonly IMapper _autoMapper;
        private readonly CouchWrapper _couchWrapper;

        public UpdateTimeSheetCommand(IServiceProvider serviceProvider)
        {
            _autoMapper = (IMapper)serviceProvider.GetService(typeof(IMapper));

            _couchWrapper = new CouchWrapper(serviceProvider, DbConsts.ServiceName);
        }

        public async Task<UpdateTimeSheetCommandContext> Execute(UpdateTimeSheetCommandContext commandContext)
        {
            // FIXME: нельзя так делать, надо от frontend получать Rev
            var header = await _couchWrapper.GetHeaderAsync(commandContext.TimeSheet.Id);

            var entity = _autoMapper.Map<TimeSheetDto>(commandContext.TimeSheet);

            entity._rev = header.Rev;

            var result = await _couchWrapper.GetResponseAsync(async (client) =>
            {
                return await client.Entities.PutAsync(entity._id, entity);
            });

            return commandContext; // TODO: возвращать _revid
        }
    }
}
using System.Threading.Tasks;
using AutoMapper;
using Cmas.DataLayers.CouchDb.TimeSheets.Dtos;
using Cmas.Infrastructure.Domain.Commands;
using Cmas.BusinessLayers.TimeSheets.CommandsContexts;
using Cmas.DataLayers.Infrastructure;
using System;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Commands
{
    public class CreateTimeSheetCommand : ICommand<CreateTimeSheetCommandContext>
    {
        private IMapper _autoMapper;
        private readonly CouchWrapper _couchWrapper;

        public CreateTimeSheetCommand(IServiceProvider serviceProvider)
        {
            _autoMapper = (IMapper)serviceProvider.GetService(typeof(IMapper));

            _couchWrapper = new CouchWrapper(serviceProvider, DbConsts.ServiceName);
        }

        public async Task<CreateTimeSheetCommandContext> Execute(CreateTimeSheetCommandContext commandContext)
        {
            var doc = _autoMapper.Map<TimeSheetDto>(commandContext.TimeSheet);

            var result = await _couchWrapper.GetResponseAsync(async (client) =>
            {
                return await client.Entities.PostAsync(doc);
            });

            commandContext.Id = result.Id;

            return commandContext;
        }
    }
}
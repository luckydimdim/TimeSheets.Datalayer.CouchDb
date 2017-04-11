using System;
using System.Threading.Tasks;
using Cmas.Infrastructure.Domain.Commands;
using MyCouch;
using Cmas.BusinessLayers.TimeSheets.CommandsContexts;

namespace Cmas.DataLayers.CouchDb.TimeSheets.Commands
{
    public class DeleteTimeSheetCommand : ICommand<DeleteTimeSheetCommandContext>
    {
        public async Task<DeleteTimeSheetCommandContext> Execute(DeleteTimeSheetCommandContext commandContext)
        {
            using (var store = new MyCouchStore(DbConsts.DbConnectionString, DbConsts.DbName))
            {

                bool success = await store.DeleteAsync(commandContext.Id);

                if (!success)
                {
                    throw new Exception("error while deleting");
                }

                return commandContext;
            }

        }
    }
}

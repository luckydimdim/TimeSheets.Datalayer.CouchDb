using System;
using AutoMapper;
using System.Threading.Tasks;
using Cmas.Infrastructure.Domain.Criteria;
using Cmas.DataLayers.CouchDb.TimeSheets.Queries;
using Cmas.DataLayers.CouchDb.TimeSheets;
using Cmas.BusinessLayers.TimeSheets.Entities;

namespace ConsoleTests
{
    class Program
    {
        private static IMapper _mapper;

        static void Main(string[] args)
        {
            try
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.AddProfile<AutoMapperProfile>();
                });

                _mapper = config.CreateMapper();


                FindByIdQueryTest().Wait();


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            Console.ReadKey();
        }

        static async Task<bool> FindByIdQueryTest()
        {
            FindByIdQuery findByIdQuery = new FindByIdQuery(_mapper);
            FindById criterion = new FindById("26270cfa2422b2c4ebf158285e0ccb73");
            TimeSheet result = null;

            try
            {
                result = await findByIdQuery.Ask(criterion);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            Console.WriteLine(result.Id);

            return true;
        }



    }
}
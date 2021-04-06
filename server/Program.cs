using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace server
{
    public static class Program
    {
        public static void Main(string[] args)
        {

            // try
            // {
            //     var settings = new MongoDbSettings()
            //     {
            //         ConnectionString = "mongodb://admin:admin@db.mongo-dev",
            //         DatabaseName = "demo"
            //     };
            //     Data.Repositories.MongoEvents events = new Data.Repositories.MongoEvents(settings);
            //     Data.Repositories.MongoRepository<Person> repo = new(settings);



            //     List<Person> people = new List<Person>();
            //     var a = repo.AsQueryable().Where(c=> c.LastName.Contains("ccc")).Select(c=> c.Id).ToList();
            //     people.AddRange(repo.AsQueryable().Take(39999).ToList());
            //     repo.DeleteById(a);

            //     if(people.Count < 100000)
            //     {
            //         for(int i=0;i<=1000000;i++)
            //         {
            //             people.Add(new Person(){
            //                 LastName = Guid.NewGuid().ToString(),
            //                 FirstName = "Michael" + i
            //             });
            //         }
            //         repo.InsertMany(people);
            //     }
            // }
            // catch (Exception ex)
            // {


            // }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}

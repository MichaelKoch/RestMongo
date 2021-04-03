using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models;

namespace server
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Data.Repositories.MongoRepository<Person> repo = new(
                new MongoDbSettings(){
                    ConnectionString="mongodb://admin:admin@db.mongo-dev",
                    DatabaseName="demo"
                }
            );
            var count = repo.AsQueryable().Skip(100).Take(500).ToList();
            repo.InsertOne(new Person(){FirstName="Michael",
            LastName="koch"});
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}

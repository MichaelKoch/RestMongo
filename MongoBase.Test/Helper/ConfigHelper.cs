using MongoBase.Interfaces;
using MongoBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoBase.Test
{
    public class ConfigHelper
    {
        public static IConnectionSettings GetMongoConfig()
        {
            return new ConnectionSettings()
            {
                ConnectionString = "mongodb://admin:admin@vehicle:27017",
                DatabaseName = "test"
            };
        }

    }
}

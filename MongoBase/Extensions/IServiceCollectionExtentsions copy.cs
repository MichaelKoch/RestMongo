using System.Linq;

using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using MongoBase.Interfaces;
using MongoBase.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoBase
{
    public static class IApplicationBuilderExtentsions
    {
        public static void UseMongoBase(this IApplicationBuilder application)
        {
           
        }
    }

}
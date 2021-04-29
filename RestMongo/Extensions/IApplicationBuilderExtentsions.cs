using System.Linq;

using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using RestMongo.Interfaces;
using RestMongo.Models;
using RestMongo.Repositories;
using RestMongo.Utils;
using MongoDB.Bson;
using MongoDB.Driver;

namespace RestMongo
{
    public static class IApplicationBuilderExtentions
    {
        public static void AddRestMongo(this IApplicationBuilder app)
        {

        }
    }

}
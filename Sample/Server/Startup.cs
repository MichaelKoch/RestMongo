using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using MongoBase;
using MongoBase.Interfaces;
using MongoBase.Models;
using MongoBase.Repositories;
using MongoBase.Utils;
using Sample.Domain;
using Sample.Domain.Interfaces;
using Sample.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Swashbuckle.AspNetCore.Newtonsoft;
using System.Threading.Tasks;

namespace SampleServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddControllers();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddResponseCompression();
            services.AddSwaggerGen(c =>
            {
                c.ResolveConflictingActions(apiDescriptions => {
                    return apiDescriptions.First();
                });
            });

            services.AddMongoBase(Configuration);
            var mongoSettings = new ConnectionSettings();
            Configuration.GetSection("mongo").Bind(mongoSettings);
            services.AddSingleton<IConnectionSettings>(mongoSettings);
            SchemaInitializer.Run(mongoSettings, typeof(ProductContext).Assembly);
            services.AddScoped<ProductContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();
            app.UseMvc(routeBuilder => routeBuilder.EnableDependencyInjection());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SampleServer v1")
                );
            }
            app.UseMongoBase();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}

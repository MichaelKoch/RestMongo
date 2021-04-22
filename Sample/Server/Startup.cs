using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoBase;
using MongoBase.Interfaces;
using MongoBase.Models;
using MongoBase.Utils;
using Sample.Domain;
using System.Linq;

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
            services.AddMongoBase(Configuration);
            services.AddScoped<ProductContext>();
            services.AddControllers();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddResponseCompression();
            services.AddSwaggerGen(c =>
            {
                c.ResolveConflictingActions(apiDescriptions => {
                    return apiDescriptions.First();
                });
            });
            InitSchema();
         
        }

        protected void InitSchema()
        {
            ConnectionSettings mongoSettings = new ConnectionSettings();
            Configuration.GetSection("mongo").Bind(mongoSettings);
            SchemaInitializer.Run(mongoSettings, typeof(ProductContext).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            app.UseResponseCompression();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SampleServer v1")
                );
            }
            app.UseMvc(routeBuilder =>
            {
                routeBuilder.EnableDependencyInjection();
            });
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

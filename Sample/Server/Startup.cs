using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestMongo;
using RestMongo.Interfaces;
using RestMongo.Models;
using RestMongo.Utils;
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
            services.AddRestMongo<ProductContext>(Configuration);
            services.AddScoped<ProductContext>();
            services.AddControllers();
            services.AddMvcCore(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().ToList())
                {
                    options.OutputFormatters.Remove(outputFormatter);
                    //outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
                }
                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().ToList())
                {
                    options.InputFormatters.Remove(inputFormatter);
                    //inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
                }
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
            app.UseRouting();
            app.UseAuthorization();
            app.AddRestMongo();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.EnableDependencyInjection();
            });
        }
    }
}

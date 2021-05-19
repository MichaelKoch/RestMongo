using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using RestMongo.Extensions;
using SimpleCartService.Controllers.Cart;
using SimpleCartService.DomainServices;
using SimpleCartService.Entities;
using SimpleCartService.Models.Cart;
using SimpleCartService.Models.CartItem;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SimpleCartService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddRestMongo<SimpleCartService.Startup>(Configuration);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Domain.CartService", Version = "v1" });
            });

            // showcase simple instance injection
            services.AddDomainService<CartDomainService>();
            services.AddDomainService<CartItemDomainService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.AddRestMongo(env);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.DisplayOperationId();
                    c.DefaultModelRendering(ModelRendering.Example);
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Domain.CartService v1");
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}

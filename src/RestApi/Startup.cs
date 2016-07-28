using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using RestApi.Models;
using RestApi.Models.Repositories;
using Swashbuckle.Swagger.Model;
using Microsoft.Extensions.Configuration;

namespace RestApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            IConfigurationRoot connectionStringConfig = ReadConfiguration();

            services.AddOptions();

            services
               .AddMvc()
               .AddJsonOptions(options =>
               {
                   options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
               });

            services.AddSwaggerGen(c =>
            {
                c.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "Super cool service API",
                    Description = "A sample API for demoing super cool service thingies!",
                });
            });

            services.AddSingleton(typeof(IPersonRepository), typeof(InMemoryPersonRepository));

        }

        private static IConfigurationRoot ReadConfiguration()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var connectionStringConfig = builder.Build();
            return connectionStringConfig;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            app.UseDeveloperExceptionPage();
            app.UseMvc();

            app.UseSwagger((httpRequest, swaggerDoc) =>
            {
                swaggerDoc.Host = httpRequest.Host.Value;
            });

            app.UseSwaggerUi();
        }
    }
}

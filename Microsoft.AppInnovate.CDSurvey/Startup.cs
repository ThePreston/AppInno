using Azure.Core;
using Azure.Identity;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AppInnovate.CDSurvey.Service;
using Microsoft.AppInnovate.CDSurvey.Service.EF;
using Microsoft.AppInnovate.CDSurvey.Service.Models;
using StackExchange.Profiling.Storage;
using System;
using System.Linq;

namespace PowerOData
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
            services.AddLogging();
            services.AddControllers().AddNewtonsoftJson();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Survey", Version = "v1" });
            });

            services.AddTransient<IRepoService<SurveyEntityModel>, EFSurveyService>();
            
            services.AddOData();

            services.AddScoped(Provider => {

                var conn = new SqlConnection(Configuration.GetConnectionString("AppInnoConn"))
                {
                    AccessToken = new DefaultAzureCredential(false).GetToken(new TokenRequestContext(new[] { "https://database.windows.net/" })).Token
                };

                return conn;

            });

            services.AddDbContext<SurveyContext>();

            ////Remove Profiler 
            //services.AddMiniProfiler(options =>
            //{
            //    options.RouteBasePath = "/profiler";
            //    (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(5);
            //    options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();  // SqlServerFormatter
            //}).AddEntityFramework();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            //app.UseMiniProfiler();

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Survey v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.EnableDependencyInjection();
                endpoints.Select().Count().Filter().OrderBy().MaxTop(100).SkipToken().Expand();
                endpoints.MapControllers();

            });
        }
    }
}

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Tfs.Api.Infrastructure.DbContexts;
using Tfs.Api.WebApi.AutofacModules;
using Tfs.Api.WebApi.Extensions;
namespace Tfs.Api.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }
        //public ILifetimeScope AutofacContainer { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomMvc()
                .AddCustomDbContext(Configuration)
                .AddCustomSwagger()
                .AddCustomAutoMapper()
            //.AddFluentValidation()
            //.AddHttpLogging()
            //.AddProblemDetails()
            //.AddHttpClient()
            ;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {

            builder.RegisterModule(new ApplicationModule());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            Log.Information("environment {environment}", Configuration["ASPNETCORE_ENVIRONMENT"]);
            //         app.UseSerilogRequestLogging();

            //         // Configure the HTTP request pipeline.
            //         //if (env.IsDevelopment())
            //         //{
            //         //app.UseProblemDetails();
            //         //app.UseHttpLogging();
            app.UseSwagger();
            app.UseSwaggerUI();
            ////}
            ///
            app.UseRequestMiddleware();// Colocando este middleware al principio del método da problemas de concurrencia.
            app.UseRouting();
            ////app.UseCors("_myAllowSpecificOrigins");
            ////app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            if (env.IsDevelopment())
            {                
                var container = app.ApplicationServices.GetAutofacRoot();
                using (var scope = container.BeginLifetimeScope())
                {
                    var context = scope.Resolve<TfsApiDbContext>();
                    var databaseName = context.Database.GetDbConnection().Database;
                    if (databaseName == "TfsApiDev")
                    {
                        context.Database.Migrate();
                    }
                }
            }
            //// The UseStaticFiles method in ASP.NET Core is used to enable serving static files, such as HTML, images, CSS, and JavaScript, directly from the application's root or a specified directory. This middleware is essential for handling static content in your web application.
            app.UseStaticFiles();
            //app.UseRequestMiddleware();// Colocando este middleware al principio del método da problemas de concurrencia.
                                       //         //app.UseSerilogRequestLogging();
                                       lifetime.ApplicationStopped.Register(() => Log.CloseAndFlush());
        }
        public string addnumbertostring(string cadena, int number) { return cadena + number; }
    }
}
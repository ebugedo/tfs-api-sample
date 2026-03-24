using Autofac;
using Autofac.Extensions.DependencyInjection;
using DataLoader.Application.Interfaces;
using DataLoader.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
namespace Tfs.Api.DataLoader
{
    public class Program
	{
		static async Task Main(string[] args)
		{

			Log.Logger = new LoggerConfiguration()
					.MinimumLevel.Information()
					.WriteTo.Console()
					.WriteTo.File("logfile.txt", rollingInterval: RollingInterval.Day) // Configuración para escribir en un archivo
					.CreateLogger();
			Log.Information("Starting up!");
			try
			{
				IServiceCollection services = new ServiceCollection();
				ConfigureServices(services);
				
				var container = ConfigureContainer();
				using (var scope = container.BeginLifetimeScope())
				{
					var dataLoaderService = scope.Resolve<IDataLoaderService>();
					//var httpClientFactory = scope.Resolve<IHttpClientFactory>();
					//var httpClient = httpClientFactory.CreateClient("TfsApiClient");
					await dataLoaderService.Load();
				}
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "An unhandled exception occurred during bootstrapping");
			}
			finally
			{
				Log.CloseAndFlush();
			}

			
		}
		static IContainer ConfigureContainer()
		{
			var serviceCollection = new ServiceCollection();
			ConfigureServices(serviceCollection);

			var containerBuilder = new ContainerBuilder();
			containerBuilder.Populate(serviceCollection);

			containerBuilder.RegisterType<DataLoaderService>().As<IDataLoaderService>();
			containerBuilder.RegisterType<DataLoaderClientsService>().As<IDataLoaderClientsService>();
			containerBuilder.RegisterType<DataLoaderProjetsService>().As<IDataLoaderProjectsService>();
			containerBuilder.RegisterType<DataLoaderSectorsService>().As<IDataLoaderSectorsService>();
			containerBuilder.RegisterType<DataLoaderTechnologiesService>().As<IDataLoaderTechnologiesService>();

			return containerBuilder.Build();
		}
		static void ConfigureServices(IServiceCollection services)
		{
			services.AddLogging(builder => builder.AddSerilog());
			var configuration = new ConfigurationBuilder()
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.json")
					.Build();
			services.AddHttpClient("MyApiClient", client =>
			{
				client.BaseAddress = new Uri(configuration["HttpClientConfig:MyApiClient:BaseAddress"]);
				client.Timeout = TimeSpan.FromSeconds(Convert.ToDouble(configuration["HttpClientConfig:MyApiClient:TimeoutSeconds"]));
			});
			IServiceProvider serviceProvider = services.BuildServiceProvider();
		}
	}
}
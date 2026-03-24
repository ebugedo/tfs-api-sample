using Autofac.Extensions.DependencyInjection;
using Serilog;
using Serilog.Templates;
namespace Tfs.Api.WebApi
{
	/// <summary>
	/// 
	/// </summary>
	public class Program
	{
		//public static readonly string AppName = Assembly.GetExecutingAssembly().GetName().Name;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		public static int Main(string[] args)
		{
			// Bootstrap logging with Serilog + ASP.NET Core https://nblumhardt.com/2020/10/bootstrap-logger/
			Log.Logger = new LoggerConfiguration()
								.WriteTo.Console()
								.WriteTo.File("logfile.txt", rollingInterval: RollingInterval.Day) // Configuración para escribir en un archivo
								.CreateBootstrapLogger();
			Log.Information("Starting up!");
			//Log.Logger = CreateSerilogLogger();
			try
			{
				CreateHostBuilder(args).Build().Run();
				Log.Information("Stopped cleanly");
				return 0;
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "An unhandled exception occurred during bootstrapping");
				return 1;
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}
		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseServiceProviderFactory(new AutofacServiceProviderFactory())
				.ConfigureAppConfiguration((hostingContext, config) =>
				{
					config.AddCustomConsul(hostingContext.HostingEnvironment.EnvironmentName);
				})
                .UseSerilog((context, services, configuration) => configuration
					.ReadFrom.Configuration(context.Configuration)
					.ReadFrom.Services(services)
					.Enrich.FromLogContext()
					.WriteTo.Console(new ExpressionTemplate(
					// Include trace and span ids when present.
					"[{@t:HH:mm:ss} {@l:u3}{#if @tr is not null} ({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}"))
					.WriteTo.File("logfile.txt", rollingInterval: RollingInterval.Day) // Configuración para escribir en un archivo
				)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				})
				.UseSerilog()
			;
		//private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
		//{
		//	var appName = Assembly.GetExecutingAssembly().GetName().Name;
		//	//var seqServerUrl = configuration["Serilog:SeqServerUrl"];
		//	var seqServerUrl = "http://seq";
		//	//var logstashUrl = configuration["Serilog:LogstashgUrl"];

		//	return new LoggerConfiguration()
		//		.MinimumLevel.Verbose()
		//		.Enrich.WithProperty("ApplicationContext", appName)
		//		.Enrich.WithProperty("API Version", Assembly.GetEntryAssembly().GetName().Version)
		//		.Enrich.FromLogContext()
		//		.WriteTo.Console()
		//		.WriteTo.File("logfile.txt", rollingInterval: RollingInterval.Day) // Configuración para escribir en un archivo
		//		.WriteTo.Debug()
		//		.WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
		//		.ReadFrom.Configuration(configuration)
		//		.CreateLogger();
		//}
	}

}




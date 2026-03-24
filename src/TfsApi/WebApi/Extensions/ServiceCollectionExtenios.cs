

using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using Tfs.Api.Application.Mappers;
using Tfs.Api.Infrastructure.DbContexts;
using TfsApi.WebApi.Extensions;
namespace Tfs.Api.WebApi.Extensions
{
	public static class ServiceCollectionExtenios
	{
		public static IServiceCollection AddCustomMvc(this IServiceCollection services)
		{
			services.AddControllers(options =>
			{
				options.Filters.Add(typeof(HttpGlobalExceptionFilter<Exception>));
				options.Filters.Add(typeof(HttpResponseOuputFilter));
			});
			//services.AddCors(options =>
			//{
			//	options.AddPolicy(name: "_myAllowSpecificOrigins",
			//					  policy =>
			//					  {
			//						  //policy.WithOrigins("http://localhost:3000", "http://localhost");
			//						  policy.AllowAnyOrigin();
			//						  policy.AllowAnyHeader();
			//						  policy.AllowAnyMethod();
			//					  });
			//});

			return services;
		}
		/// <remarks>
		/// https://github.com/FluentValidation/FluentValidation/issues/1965
		/// </remarks>
		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddFluentValidation(this IServiceCollection services)
		{
			services.AddFluentValidationAutoValidation(); //no se pueden meter validaciones asíncronas
			services.AddFluentValidationClientsideAdapters();
			//services.AddValidatorsFromAssemblyContaining<XXXXDtoValidator>();
			return services;
		}

		public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration["database:ConnectionString"];
			Log.Information("connectionString {connectionString}", connectionString);

            services.AddDbContext<TfsApiDbContext>(options =>
			{
				options.UseSqlServer(
					connectionString,
					sqlServerOptionsAction: sqlOptions =>
					{
						sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
					});
			},
			ServiceLifetime.Scoped);
			//ServiceLifetime.Transient);

			return services;
		}
		public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
		{

			var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1",
					new OpenApiInfo
					{
						Title = "TFS Project Info API",
						Version = "v1",
						Description = "API de información de proyectos de Time For Software"
                    });
				options.IncludeXmlComments(xmlPath);
				options.EnableAnnotations();
			});

			return services;
		}
		public static IServiceCollection AddCustomAutoMapper(this IServiceCollection services)
		{
			services.AddAutoMapper(typeof(ProjectProfile));
			return services;
		}

		public static IServiceCollection AddHttpLogging(this IServiceCollection services)
		{
			services.AddHttpLogging(logging =>
			{
				// Customize HTTP logging here.
				logging.LoggingFields = HttpLoggingFields.All;
				logging.RequestHeaders.Add("My-Request-Header");
				logging.ResponseHeaders.Add("My-Response-Header");
				logging.MediaTypeOptions.AddText("application/javascript");
				logging.RequestBodyLogLimit = 4096;
				logging.ResponseBodyLogLimit = 4096;
			});
			return services;
		}

	
	}
}

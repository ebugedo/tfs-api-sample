using Autofac;
using Domain.AggregateModels;
using Domain.AggregateModels.Projects;
using Domain.Interfaces;
using Infrastructure.Repositories.Projects;
using Tfs.Api.Application.Interfaces;
using Tfs.Api.Application.Services;
using Tfs.Api.Domain.AggretateModels;
using Tfs.Api.Domain.Interfaces;
using Tfs.Api.Infrastructure.Repositories;

namespace Tfs.Api.WebApi.AutofacModules
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationModule : Module
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="builder"></param>
		protected override void Load(ContainerBuilder builder)
		{
			// Repositorios
			builder.RegisterType<ProjectRepository>().As<IProjectRepository>();
			// Repositorios genéricos
			builder.RegisterType<EFRepository<RequestTrace, int>>().As<IRepository<RequestTrace, int>>().InstancePerLifetimeScope();
			builder.RegisterType<EFRepository<Client, int>>().As<IRepository<Client, int>>().InstancePerLifetimeScope();
			builder.RegisterType<EFRepository<Project, int>>().As<IRepository<Project, int>>().InstancePerLifetimeScope();
			builder.RegisterType<EFRepository<ProjectTechnology, int>>().As<IRepository<ProjectTechnology, int>>().InstancePerLifetimeScope();
			builder.RegisterType<EFRepository<Sector, int>>().As<IRepository<Sector, int>>().InstancePerLifetimeScope();
			builder.RegisterType<EFRepository<Technology, int>>().As<IRepository<Technology, int>>().InstancePerLifetimeScope();
			// Servicios de aplicación
			builder.RegisterType<RequestTraceService>().As<IRequestTraceService>();
			builder.RegisterType<ClientService>().As<IClientService>();
			builder.RegisterType<ProjectService>().As<IProjectService>();
			builder.RegisterType<SectorService>().As<ISectorService>();
			builder.RegisterType<TechnologyService>().As<ITechnologyService>();
			// Servicios para tests
			// Servicios genéricos de base de datos
			// Servicios genéricos de base de datos de sólo lectura
			// Validators
			// Otros
			builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
		}
	}
}

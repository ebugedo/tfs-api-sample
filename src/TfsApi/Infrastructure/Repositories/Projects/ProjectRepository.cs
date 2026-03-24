using Domain.AggregateModels.Projects;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tfs.Api.Infrastructure.DbContexts;
using Tfs.Api.Infrastructure.Repositories;
namespace Infrastructure.Repositories.Projects
{
    public class ProjectRepository : EFRepository<Project, int>, IProjectRepository
	{
		private readonly ILogger<ProjectRepository> _logger;
		public ProjectRepository(
					TfsApiDbContext dbContext,
					ILogger<ProjectRepository> logger
			) : base(dbContext)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<Project> RetrieveByKeyAsync(int id)
		{
			_logger.LogInformation("ProjectRepository - RetrieveByKeyAsync. Project id: {id}", id);
			var query = await base.DbContext.Project
						.Include(c => c.Client)
						.Include(c => c.Sector)
						.Include(c => c.ProjectTechnologies)
						.ThenInclude(projectTechnology => projectTechnology.Technology)
						.Where(c=>c.Id==id)
						.FirstOrDefaultAsync();
			return query;
		}
		public async Task<IList<Project>> RetrieveAllAsync()
		{
			_logger.LogInformation("ProjectRepository - RetrieveAllAsync.");
			var query = await base.DbContext.Project
						.Include(c => c.Client)
						.Include(c => c.Sector)
						.Include(c => c.ProjectTechnologies)
						.ThenInclude(projectTechnology => projectTechnology.Technology)
						.ToListAsync();
			return query;
		}
		public async Task<int> SaveChangesAsync()
		{
			_logger.LogInformation("ProjectRepository - SaveChangesAsync");
			int saved = 0;
			try
			{
				saved = await base.DbContext.SaveChangesAsync();
				base.DbContext.ChangeTracker.Entries().ToList().ForEach(entry => entry.State = EntityState.Detached);
			}
			catch (Exception exception)
			{
				throw GetDomainException(exception);
			}
			return saved;
		}
		private Exception GetDomainException(Exception exception)
		{
			if (exception.InnerException != null &&
				exception.InnerException.Message.Contains("FOREIGN KEY constraint"))
			{
				//if (exception.InnerException.Message.Contains("FK_VIS_VISITA_VIS_VISITA_ID_VISITA"))
				//{
				//	return new PreviousVisitDoesntExistException("La visita previa no está registrada en el sistema");
				//}
				//else if (exception.InnerException.Message.Contains("FK_VIS_VISITA_VIS_VISITADOR_ID_ACOMP1"))
				//{
				//	return new VisitAssistantDoesntExistException("El asistente de la visita no está registrado en el sistema");
				//}

				//else if (exception.InnerException.Message.Contains("FK_VIS_VISITA_GEN_CAMPANYA"))
				//{
				//	return new VisitCampaignDoesntExistException("La campaña de la visita no está registrado en el sistema");
				//}
				//else if (exception.InnerException.Message.Contains("FK_VIS_EMPRESA_GEN_PROVINCIA"))
				//{
				//	return new VisitCompanyProvinceDoesntExistException("La provincia de la empresa o del promotor de la visita no está registrada en el sistema");
				//}
				//else if (exception.InnerException.Message.Contains("FK_VIS_OBRA_GEN_PROVINCIA"))
				//{
				//	return new VisitConstructionSiteProvinceDoesntExistException("La provincia de la obra de la visita no está registrada en el sistema");
				//}
				//else if (exception.InnerException.Message.Contains("FK_VIS_VISITA_VIS_VISITADOR_ID_ACOMP2"))
				//{
				//	return new VisitSecondAssistantDoesntExistException("El segundo asistente de la visita no está registrado en el sistema");
				//}
				//else if (exception.InnerException.Message.Contains("FK_VIS_VISITA_VIS_ENCUESTA"))
				//{
				//	return new VisitSurveyDoesntExistException("La encuesta de la visita no está registrado en el sistema");
				//}
				//else if (exception.InnerException.Message.Contains("FK_VIS_VISITA_GEN_AMBITO_TERRITORIAL"))
				//{
				//	return new VisitTerritorialScopeDoesntExistException("El ámbito territorial de la visita no está registrado en el sistema");
				//}
				//else if (exception.InnerException.Message.Contains("FK_VIS_VISITA_VIS_VISITADOR"))
				//{
				//	return new VisitVisitorDoesntExistException("El visitador de la visita no está registrado en el sistema");
				//}
			}
			return exception;
		}
	}
}

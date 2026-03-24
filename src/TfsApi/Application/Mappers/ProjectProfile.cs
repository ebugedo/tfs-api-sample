using AutoMapper;
using Domain.AggregateModels.Projects;
using Tfs.Api.Application.Dtos.Projects;
using Tfs.Api.Application.Dtos.Technologies;

namespace Tfs.Api.Application.Mappers
{
	/// <summary>
	/// 
	/// </summary>
	public class ProjectProfile : Profile
	{
		/// <summary>
		/// 
		/// </summary>
		public override string ProfileName => "ProjectProfile";

		/// <summary>
		/// Inicializa una nueva instancia de la clase <see cref="ProjectProfile"/>.
		/// </summary>
		public ProjectProfile()
		{
			CreateMap<Project, ProjectGetDto>()
				.AfterMap((src, dest, context) =>
				{
					foreach (var projectTechnology in src.ProjectTechnologies)
					{
						var technology = context.Mapper.Map<TechnologyGetDto>(projectTechnology.Technology);
						dest.Technologies.Add(technology);
					}
				});
			CreateMap<ProjectPostDto, Project>()
				.ConstructUsing(src =>
					new Project(
						src.ClientId,
						src.Description,
						src.DurationInMonths,
						src.Name,
						src.SectorId,
						src.StartDateAndTime
						))
				.AfterMap((src, dest) =>
				{
					foreach (var technologyId in src.TechnologyIds)
					{
						dest.AddProjectTechnology(technologyId);
					}
				})
				.ForAllMembers(opt => opt.Ignore());
		}
	}
}

using AutoMapper;
using Domain.AggregateModels.Projects;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Tfs.Api.Application.Dtos.Projects;
using Tfs.Api.Application.Interfaces;
using Tfs.Api.Domain.Interfaces;
namespace Tfs.Api.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectService : IProjectService
	{
		private readonly ILogger<ProjectService> _logger;
		private readonly IMapper _mapper;
		IProjectRepository _repository;
		IUnitOfWork _unitOfWork;
		public ProjectService(
			ILogger<ProjectService> logger,
			IMapper mapper,
			IProjectRepository repository,
			IUnitOfWork unitOfWork
			)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}
		public async Task<ProjectGetDto> CreateAsync(ProjectPostDto project)
		{
			_logger.LogInformation("ProjectService - CreateAsync. Project: {@project}", project);
			ValidateCreate(project);
			var projectToCreate = _mapper.Map<Project>(project);
			var projectAdded = await _repository.AddAsync(projectToCreate);
			var saved = await _unitOfWork.SaveChangesAsync();
			var proyectCreated = await GetAsync(projectAdded.Id);
			return proyectCreated;
		}
		public async Task<IList<ProjectGetDto>> GetAllAsync()
		{
			_logger.LogInformation("ProjectService - GetAllAsync.");
			var projects = await _repository.RetrieveAllAsync();
			var projectsDto = _mapper.Map<IList<ProjectGetDto>>(projects);
			return projectsDto;
		}

		public async Task<ProjectGetDto> GetAsync(int id)
		{
			_logger.LogInformation("ProjectService - GetAsync. Id: {id}", id);
			
			var project=await _repository.RetrieveByKeyAsync(id);
			var projectDto = _mapper.Map<ProjectGetDto>(project);
			return projectDto;
		}

		private void ValidateCreate(ProjectPostDto project)
		{
			if (project == null)
			{
				throw new ArgumentNullException(nameof(project));
			}

		}
	}
}

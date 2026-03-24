using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Tfs.Api.Application.Interfaces;
using Tfs.Api.Domain.Interfaces;
using Domain.AggregateModels;
using Tfs.Api.Application.Dtos.Technologies;
namespace Tfs.Api.Application.Services
{
	/// <summary>
	/// 
	/// </summary>
	public class TechnologyService : ITechnologyService
	{
		private readonly ILogger<TechnologyService> _logger;
		private readonly IMapper _mapper;
		IRepository<Technology, int> _repository;
		IUnitOfWork _unitOfWork;
		public TechnologyService(
			ILogger<TechnologyService> logger,
			IMapper mapper,
			IRepository<Technology, int> repository,
			IUnitOfWork unitOfWork
			)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}
		public async Task<TechnologyGetDto> CreateAsync(TechnologyPostDto technology)
		{
			_logger.LogInformation("TechnologyService - CreateAsync. Technology: {@Technology}", technology);
			await ValidateCreateAsync(technology);
			var technologyToCreate = _mapper.Map<Technology>(technology);
			var technologyAdded = await _repository.AddAsync(technologyToCreate);
			var saved = await _unitOfWork.SaveChangesAsync();
			var technologyCreated= _mapper.Map<TechnologyGetDto>(technologyAdded);
			//var TechnologyCreated = await GetAsync(TechnologyAdded.Id);
			return technologyCreated;
		}
		public async Task<IList<TechnologyGetDto>> GetAllAsync()
		{
			_logger.LogInformation("TechnologyService - GetAllAsync.");
			var technologies = await _repository.RetrieveAllAsync();
			var technologiesDto = _mapper.Map<IList<TechnologyGetDto>>(technologies);
			return technologiesDto;
		}
		private async Task ValidateCreateAsync(TechnologyPostDto technology)
		{
			if (technology == null)
			{
				throw new ArgumentNullException(nameof(technology));
			}
		}
		public async Task<TechnologyGetDto> GetAsync(int id)
		{
			_logger.LogInformation("TechnologyService - GetAsync. Id: {id}", id);
			Expression<Func<Technology, bool>> expression = null;
			expression = c => c.Id == id;
			var technologys = await _repository.RetrieveByExpressionAsync(
				expression: expression
			);
			//var visitsSql = visits.ToQueryString();

			if (technologys.Count() != 1)
			{
				return null;
			}
			var technology = technologys.FirstOrDefault();
			var technologyDto = _mapper.Map<TechnologyGetDto>(technology);
			return technologyDto;
		}

		public async Task<TechnologyGetDto> GetByNameAsync(string name)
		{
			_logger.LogInformation("TechnologyService - GetByNameAsync. Name: {name}", name);
			Expression<Func<Technology, bool>> expression = null;
			expression = c => c.Name == name;
			var technologys = await _repository.RetrieveByExpressionAsync(
				expression: expression
			);
			//var visitsSql = visits.ToQueryString();

			if (technologys.Count() != 1)
			{
				return null;
			}
			var technology = technologys.FirstOrDefault();
			var technologyDto = _mapper.Map<TechnologyGetDto>(technology);
			return technologyDto;
		}
	}
}

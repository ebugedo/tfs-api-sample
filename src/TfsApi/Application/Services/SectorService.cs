using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Tfs.Api.Application.Dtos.Sectors;
using Tfs.Api.Application.Interfaces;
using Tfs.Api.Domain.AggretateModels;
using Tfs.Api.Domain.Interfaces;
namespace Tfs.Api.Application.Services
{
	/// <summary>
	/// 
	/// </summary>
	public class SectorService : ISectorService
	{
		private readonly ILogger<SectorService> _logger;
		private readonly IMapper _mapper;
		IRepository<Sector, int> _repository;
		IUnitOfWork _unitOfWork;
		public SectorService(
			ILogger<SectorService> logger,
			IMapper mapper,
			IRepository<Sector, int> repository,
			IUnitOfWork unitOfWork
			)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}
		public async Task<SectorGetDto> CreateAsync(SectorPostDto sector)
		{
			_logger.LogInformation("SectorService - CreateAsync. Sector: {@Sector}", sector);
			await ValidateCreateAsync(sector);
			var sectorToCreate = _mapper.Map<Sector>(sector);
			var sectorAdded = await _repository.AddAsync(sectorToCreate);
			var saved = await _unitOfWork.SaveChangesAsync();
			var sectorCreated= _mapper.Map<SectorGetDto>(sectorAdded);
			//var SectorCreated = await GetAsync(SectorAdded.Id);
			return sectorCreated;
		}
		public async Task<IList<SectorGetDto>> GetAllAsync()
		{
			{
				_logger.LogInformation("SectorService - GetAllAsync.");
				var sectors = await _repository.RetrieveAllAsync();
				var sectorsDto = _mapper.Map<IList<SectorGetDto>>(sectors);
				return sectorsDto;
			}
		}
		private async Task ValidateCreateAsync(SectorPostDto sector)
		{
			if (sector == null)
			{
				throw new ArgumentNullException(nameof(sector));
			}
		}
		public async Task<SectorGetDto> GetAsync(int id)
		{
			_logger.LogInformation("SectorService - GetAsync. Id: {id}", id);
			Expression<Func<Sector, bool>> expression = null;
			expression = c => c.Id == id;
			var sectors = await _repository.RetrieveByExpressionAsync(
				expression: expression
			);
			//var visitsSql = visits.ToQueryString();

			if (sectors.Count() != 1)
			{
				return null;
			}
			var sector = sectors.FirstOrDefault();
			var sectorDto = _mapper.Map<SectorGetDto>(sector);
			return sectorDto;
		}
		public async Task<SectorGetDto> GetByNameAsync(string name)
		{
			_logger.LogInformation("SectorService - GetByNameAsync. Name: {name}", name);
			Expression<Func<Sector, bool>> expression = null;
			expression = c => c.Name == name;
			var sectors = await _repository.RetrieveByExpressionAsync(
				expression: expression
			);
			if (sectors.Count() != 1)
			{
				return null;
			}
			var sector = sectors.FirstOrDefault();
			var sectorDto = _mapper.Map<SectorGetDto>(sector);
			return sectorDto;
		}
	}
}

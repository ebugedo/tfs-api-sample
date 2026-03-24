using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Tfs.Api.Application.Dtos.Clients;
using Tfs.Api.Application.Interfaces;
using Tfs.Api.Domain.AggretateModels;
using Tfs.Api.Domain.Interfaces;
namespace Tfs.Api.Application.Services
{
	/// <summary>
	/// 
	/// </summary>
	public class ClientService : IClientService
	{
		private readonly ILogger<ClientService> _logger;
		private readonly IMapper _mapper;
		IRepository<Client, int> _repository;
		IUnitOfWork _unitOfWork;
		public ClientService(
			ILogger<ClientService> logger,
			IMapper mapper,
			IRepository<Client, int> repository,
			IUnitOfWork unitOfWork
			)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}
		public async Task<ClientGetDto> CreateAsync(ClientPostDto client)
		{
			_logger.LogInformation("ClientService - CreateAsync. Client: {@Client}", client);
			await ValidateCreateAsync(client);
			var clientToCreate = _mapper.Map<Client>(client);
			var clientAdded = await _repository.AddAsync(clientToCreate);
			var saved = await _unitOfWork.SaveChangesAsync();
			var clientCreated= _mapper.Map<ClientGetDto>(clientAdded);
			//var ClientCreated = await GetAsync(ClientAdded.Id);
			return clientCreated;
		}
		private async Task ValidateCreateAsync(ClientPostDto client)
		{
			if (client == null)
			{
				throw new ArgumentNullException(nameof(client));
			}
		}
		public async Task<ClientGetDto> GetAsync(int id)
		{
			_logger.LogInformation("ClientService - GetAsync. Id: {id}", id);
			Expression<Func<Client, bool>> expression = null;
			expression = c => c.Id == id;
			var clients = await _repository.RetrieveByExpressionAsync(
				expression: expression
			);
			//var visitsSql = visits.ToQueryString();

			if (clients.Count() != 1)
			{
				return null;
			}
			var client = clients.FirstOrDefault();
			var clientDto = _mapper.Map<ClientGetDto>(client);
			return clientDto;
		}

		public async Task<IList<ClientGetDto>> GetAllAsync()
		{
			_logger.LogInformation("ClientService - GetAllAsync.");
			var clients = await _repository.RetrieveAllAsync();
			var clientsDto = _mapper.Map<IList<ClientGetDto>>(clients);
			return clientsDto;
		}
		public async Task<ClientGetDto> GetByNameAsync(string name)
		{
			_logger.LogInformation("ClientService - GetByNameAsync. Name: {name}", name);
			Expression<Func<Client, bool>> expression = null;
			expression = c => c.Name.ToUpper() == name.ToUpper();
			var clients = await _repository.RetrieveByExpressionAsync(
				expression: expression
			);
			if (clients.Count() != 1)
			{
				return null;
			}
			var client = clients.FirstOrDefault();
			var clientDto = _mapper.Map<ClientGetDto>(client);
			return clientDto;
		}
	}
}

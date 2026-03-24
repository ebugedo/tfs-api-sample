using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Tfs.Api.Application.Dtos.RequestTrace;
using Tfs.Api.Application.Interfaces;
using Tfs.Api.Domain.AggretateModels;
using Tfs.Api.Domain.Interfaces;
namespace Tfs.Api.Application.Services
{
	/// <summary>
	/// 
	/// </summary>
	public class RequestTraceService : IRequestTraceService
	{
		private readonly ILogger<RequestTraceService> _logger;
		private readonly IMapper _mapper;
		IRepository<RequestTrace, int> _repository;
		IUnitOfWork _unitOfWork;
		public RequestTraceService(
			ILogger<RequestTraceService> logger,
			IMapper mapper,
			IRepository<RequestTrace, int> repository,
			IUnitOfWork unitOfWork
			)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}
		public async Task<RequestTraceGetDto> CreateOrUpdateAsync(RequestTracePostDto requestTrace)
		{
			_logger.LogInformation("RequestTraceService - CreateOrUpdateAsync. Request trace: {@requestTrace}", requestTrace);
			ValidateCreateOrUpdateAsync(requestTrace);
			RequestTraceGetDto requestTraceToReturn= null;
			var requestTraceFound = await GetByIPAddressAsync(requestTrace.IPAddress);
			if (requestTraceFound != null)
			{
				var requestTraceToUpdate = new RequestTracePutDto()
				{
					IPAddress=requestTraceFound.IPAddress,
					NumberOfRequests=requestTraceFound.NumberOfRequests+1
				};
				requestTraceToReturn = await UpdateAsync(requestTraceFound.Id, requestTraceToUpdate);
			}
			else
			{
				requestTraceToReturn = await CreateAsync(requestTrace);
			}
			return requestTraceToReturn;
		}
		private async Task<RequestTraceGetDto> CreateAsync(RequestTracePostDto requestTrace)
		{
			_logger.LogInformation("RequestTraceService - CreateAsync. Request trace: {@requestTrace}", requestTrace);
			ValidateCreateAsync(requestTrace);
			var entityToCreate = _mapper.Map<RequestTrace>(requestTrace);
			var entityAdded = await _repository.AddAsync(entityToCreate);
			var saved = await _unitOfWork.SaveChangesAsync();
			var entityToReturn = await GetAsync(entityAdded.Id);
			return entityToReturn;
		}
		private async Task<RequestTraceGetDto> UpdateAsync(int id, RequestTracePutDto requestTrace)
		{
			_logger.LogInformation("RequestTraceService - UpdateAsync. Request trace: {@requestTrace}", requestTrace);
			ValidateUpdateAsync(requestTrace);
            Expression<Func<RequestTrace, bool>> expression = null;
            expression = c => c.Id == id;
            var requestTraces = await _repository.RetrieveByExpressionAsync(
                expression: expression
            );
            if (requestTraces.Count() != 1)
            {
                return null;
            }
            var requestTraceToUpdate = requestTraces.FirstOrDefault();
            requestTraceToUpdate.Update(requestTrace.IPAddress, requestTrace.NumberOfRequests);
			
			var entityUpdated= await _repository.UpdateAsync(requestTraceToUpdate);
			var saved = await _unitOfWork.SaveChangesAsync();
			var entityToReturn = await GetAsync(entityUpdated.Id);
			return entityToReturn;
		}
		private void ValidateCreateOrUpdateAsync(RequestTracePostDto requestTrace)
		{
			ValidateCreateAsync(requestTrace);
		}
		private void ValidateCreateAsync(RequestTracePostDto requestTrace)
		{
			if (requestTrace == null)
			{
				throw new ArgumentNullException(nameof(requestTrace));
			}
		}
		private void ValidateUpdateAsync(RequestTracePutDto requestTrace)
		{
			if (requestTrace == null)
			{
				throw new ArgumentNullException(nameof(requestTrace));
			}
		}
		public async Task<RequestTraceGetDto> GetAsync(int id)
		{
			_logger.LogInformation("RequestTraceService - GetAsync. Id: {id}", id);
			Expression<Func<RequestTrace, bool>> expression = null;
			expression = c => c.Id == id;
			var apiRequests = await _repository.RetrieveByExpressionAsync(
				expression: expression
			);
			if (apiRequests.Count() != 1)
			{
				return null;
			}
			var apiRequest = apiRequests.FirstOrDefault();
			var apiRequestDto = _mapper.Map<RequestTraceGetDto>(apiRequest);
			return apiRequestDto;
		}
		public async Task<RequestTraceGetDto> GetByIPAddressAsync(string ipAddress)
		{
			_logger.LogInformation("RequestTraceService - GetByIPAddressAsync. IP Address: {ipAddress}", ipAddress);
			Expression<Func<RequestTrace, bool>> expression = null;
			expression = c => c.IPAddress == ipAddress;
			var entities = await _repository.RetrieveByExpressionAsync(
				expression: expression
			);
			if (entities.Count() != 1)
			{
				return null;
			}
			var entity = entities.FirstOrDefault();
			var entityDto = _mapper.Map<RequestTraceGetDto>(entity);
			return entityDto;
		}
	}
}

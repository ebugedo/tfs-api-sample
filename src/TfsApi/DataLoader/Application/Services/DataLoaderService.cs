using DataLoader.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace DataLoader.Application.Services
{
	public class DataLoaderService : IDataLoaderService
	{
		private readonly HttpClient _httpClient;
		IDataLoaderClientsService _dataLoaderClientsService;
		IDataLoaderProjectsService _dataLoaderProjectsService;
		IDataLoaderSectorsService _dataLoaderSectorsService;
		IDataLoaderTechnologiesService _dataLoaderTechnologiesService;
		private readonly ILogger<DataLoaderService> _logger;
		public DataLoaderService(
			HttpClient httpClient,
			IDataLoaderClientsService dataLoaderClientsService,
			IDataLoaderProjectsService dataLoaderProjectsService,
		IDataLoaderSectorsService dataLoaderSectorsService,
			IDataLoaderTechnologiesService dataLoaderTechnologiesService,
			ILogger<DataLoaderService> logger
			)

		{
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));
			_dataLoaderClientsService = dataLoaderClientsService ?? throw new ArgumentNullException(nameof(dataLoaderClientsService));
			_dataLoaderProjectsService = dataLoaderProjectsService ?? throw new ArgumentNullException(nameof(dataLoaderProjectsService));
			_dataLoaderSectorsService = dataLoaderSectorsService ?? throw new ArgumentNullException(nameof(dataLoaderSectorsService));
			_dataLoaderTechnologiesService = dataLoaderTechnologiesService ?? throw new ArgumentNullException(nameof(dataLoaderTechnologiesService));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}
		public async Task Load()
		{
			await _dataLoaderClientsService.Load();
			await _dataLoaderSectorsService.Load();
			await _dataLoaderTechnologiesService.Load();
			await _dataLoaderProjectsService.Load();
		}
	}
}

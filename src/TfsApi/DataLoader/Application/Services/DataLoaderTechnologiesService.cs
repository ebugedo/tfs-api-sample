using System.Text.Json;
using Tfs.Api.Application.Dtos.Technologies;
using DataLoader.Application.Interfaces;
using DataLoader.Application.Dtos;
using Microsoft.Extensions.Logging;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System.Web;
namespace DataLoader.Application.Services
{
	public class DataLoaderTechnologiesService : IDataLoaderTechnologiesService
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<DataLoaderTechnologiesService> _logger;
		public DataLoaderTechnologiesService(
			HttpClient httpClient,
			ILogger<DataLoaderTechnologiesService> logger
			)

		{
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}
		public async Task Load()
		{
			_logger.LogInformation("DataLoaderTechnologiesService - Load");


			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				Delimiter = ";",
				HasHeaderRecord = true,
				PrepareHeaderForMatch = args => args.Header.ToLower(),
				

			};

			using (var reader = new StreamReader($@"{AppDomain.CurrentDomain.BaseDirectory}Data\technologies.csv"))
			using (var csv = new CsvReader(reader, config))
			{
				var records = csv.GetRecords<CsvTechnologyDto>();
				foreach (var record in records.ToList())
				{
					var technology = new TechnologyPostDto()
					{
						Description=record.Description,
						Name = record.Name
					};
					var technologyCreated = await CreateAsync(technology);
				}
			}
		}
		private async Task<TechnologyGetDto> CreateAsync(TechnologyPostDto technology)
		{
			var requestMessage = new TfsApiRequestMessage<TechnologyPostDto>(
				technology,
				HttpMethod.Post,
				$"https://localhost:7097/api/v1/technology");
			var response = await _httpClient.SendAsync(requestMessage);
			if (!response.IsSuccessStatusCode)
			{
				return await Task.FromResult<TechnologyGetDto>(null);
			}
			var responseBody = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<TechnologyGetDto>(responseBody);
			return result;
		}
		public async Task<TechnologyGetDto> GetTechnologyByNameAsync(string technologyName)
		{
			var technologyNameEncoded = HttpUtility.UrlEncode(technologyName);
			var requestMessage = new TfsApiRequestMessage<object>(
			null,
			HttpMethod.Get,
				$"https://localhost:7097/api/v1/technology/by-name?name={technologyNameEncoded}");
			var response = await _httpClient.SendAsync(requestMessage);
			if (!response.IsSuccessStatusCode)
			{
				return await Task.FromResult<TechnologyGetDto>(null);
			}
			var responseBody = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<TechnologyGetDto>(responseBody);
			return result;
		}
	}
}

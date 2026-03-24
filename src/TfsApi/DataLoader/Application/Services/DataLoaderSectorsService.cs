using System.Text.Json;
using DataLoader.Application.Interfaces;
using DataLoader.Application.Dtos;
using Tfs.Api.Application.Dtos.Sectors;
using Microsoft.Extensions.Logging;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System.Web;
namespace DataLoader.Application.Services
{
	public class DataLoaderSectorsService : IDataLoaderSectorsService
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<DataLoaderSectorsService> _logger;
		public DataLoaderSectorsService(
			HttpClient httpSector,
			ILogger<DataLoaderSectorsService> logger
			)

		{
			_httpClient = httpSector ?? throw new ArgumentNullException(nameof(httpSector));
			_httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}
		public async Task Load()
		{
			_logger.LogInformation("DataLoaderSectorsService - Load");
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				Delimiter = ";",
				HasHeaderRecord = true,
				PrepareHeaderForMatch = args => args.Header.ToLower(),

			};

			using (var reader = new StreamReader($@"{AppDomain.CurrentDomain.BaseDirectory}Data\sectors.csv"))
			using (var csv = new CsvReader(reader, config))
			{
				var records = csv.GetRecords<CsvSectorDto>();
				foreach (var record in records.ToList())
				{
					var sector = new SectorPostDto()
					{
						Name = record.Name
					};
					var sectorCreated = await CreateAsync(sector);
				}
			}
		}
		private async Task<SectorGetDto> CreateAsync(SectorPostDto client)
		{
			var requestMessage = new TfsApiRequestMessage<SectorPostDto>(
				client,
				HttpMethod.Post,
				$"https://localhost:7097/api/v1/sector");
			var response = await _httpClient.SendAsync(requestMessage);
			if (!response.IsSuccessStatusCode)
			{
				return await Task.FromResult<SectorGetDto>(null);
			}
			var responseBody = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<SectorGetDto>(responseBody);
			return result;
		}
		public async Task<SectorGetDto> GetSectorByNameAsync(string sectorName)
		{
			var sectorNameEncoded = HttpUtility.UrlEncode(sectorName);
			var requestMessage = new TfsApiRequestMessage<object>(
				null,
			HttpMethod.Get,
				$"https://localhost:7097/api/v1/sector/by-name?name={sectorNameEncoded}");
			var response = await _httpClient.SendAsync(requestMessage);
			if (!response.IsSuccessStatusCode)
			{
				return await Task.FromResult<SectorGetDto>(null);
			}
			var responseBody = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<SectorGetDto>(responseBody);
			return result;
		}
	}
}

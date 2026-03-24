using System.Text.Json;
using DataLoader.Application.Interfaces;
using DataLoader.Application.Dtos;
using Tfs.Api.Application.Dtos.Clients;
using Microsoft.Extensions.Logging;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using System.Web;
using Serilog;
using System.Text;
namespace DataLoader.Application.Services
{
	public class DataLoaderClientsService : IDataLoaderClientsService
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger<DataLoaderClientsService> _logger;
		public DataLoaderClientsService(
			HttpClient httpClient,
			ILogger<DataLoaderClientsService> logger
			)

		{
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}
		public async Task Load()
		{
			_logger.LogInformation("DataLoaderClientsService - Load");

			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
			{
				Delimiter = ";",
				HasHeaderRecord = true,
				PrepareHeaderForMatch = args => args.Header.ToLower(),

			};

			using (var reader = new StreamReader($@"{AppDomain.CurrentDomain.BaseDirectory}Data\clients.csv", Encoding.UTF8))
			using (var csv = new CsvReader(reader, config))
			{
				var records = csv.GetRecords<CsvClientDto>();
				foreach (var record in records.ToList())
				{
					var client = new ClientPostDto()
					{
						Name = record.Name
					};
					var clientCreated = await CreateAsync(client);
				}
			}
		}
		private async Task<ClientGetDto> CreateAsync(ClientPostDto client)
		{
			var requestMessage = new TfsApiRequestMessage<ClientPostDto>(
				client,
				HttpMethod.Post,
				$"https://localhost:7097/api/v1/client");
			var response = await _httpClient.SendAsync(requestMessage);
			var responseBody = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<ClientGetDto>(responseBody);
			return result;
		}
		public async Task<ClientGetDto> GetClientByNameAsync(string clientName)
		{
			var clientNameEncoded = HttpUtility.UrlEncode(clientName);
			var requestMessage = new TfsApiRequestMessage<object>(
				null,
				HttpMethod.Get,
				$"https://localhost:7097/api/v1/client/by-name?name={clientNameEncoded}");
			var response = await _httpClient.SendAsync(requestMessage);
			if (!response.IsSuccessStatusCode)
			{
				return await Task.FromResult<ClientGetDto>(null);
			}
			var responseBody = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<ClientGetDto>(responseBody);
			return result;
		}
	}
}

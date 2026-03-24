using CsvHelper.Configuration;
using CsvHelper;
using DataLoader.Application.Dtos;
using DataLoader.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.Json;
using Tfs.Api.Application.Dtos.Projects;
using System.Text;
namespace DataLoader.Application.Services
{
    public class DataLoaderProjetsService : IDataLoaderProjectsService
    {
        private readonly string _insideFiledDelimiter = ",";
        IDataLoaderClientsService _dataLoaderClientsService;
        IDataLoaderSectorsService _dataLoaderSectorsService;
        IDataLoaderTechnologiesService _dataLoaderTechnologiesService;
        private readonly HttpClient _httpClient;
        private readonly ILogger<DataLoaderProjetsService> _logger;
        public DataLoaderProjetsService(
            HttpClient httpClient,
            IDataLoaderClientsService dataLoaderClientsService,
            IDataLoaderSectorsService dataLoaderSectorsService,
            IDataLoaderTechnologiesService dataLoaderTechnologiesService,
            ILogger<DataLoaderProjetsService> logger
            )

        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));
            _dataLoaderClientsService = dataLoaderClientsService;
            _dataLoaderSectorsService = dataLoaderSectorsService;
            _dataLoaderTechnologiesService = dataLoaderTechnologiesService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task Load()
        {
            _logger.LogInformation("DataLoaderProjetsService - Load");
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                PrepareHeaderForMatch = args => args.Header.ToLower(),
            };

            using (var reader = new StreamReader($@"{AppDomain.CurrentDomain.BaseDirectory}Data\projects.csv", Encoding.UTF8))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<CsvProjectDto>();
                foreach (var record in records.ToList())
                {
                    _logger.LogInformation("DataLoaderProjetsService - Load. {@record}", record);
                    var project = new ProjectPostDto()
                    {
                        Description = record.Description,
                        DurationInMonths =record.Duration,
                        Name = record.Name,
                        StartDateAndTime = record.StartDate
                    };
                    var clientToFind = record.Client;
                    var client = await _dataLoaderClientsService.GetClientByNameAsync(clientToFind);
                    if (client == null)
                    {
                        _logger.LogError("DataLoaderClientsService - Load. No se encuentra el cliente {clientToFind}", clientToFind);
                        throw new Exception($"El cliente {clientToFind} del proyecto {project.Name} no está registrada en el sistema.");
                    }
                    var sectorToFind = record.Sector;
                    var sector = await _dataLoaderSectorsService.GetSectorByNameAsync(sectorToFind);
                    if (sector == null)
                    {
                        _logger.LogError("DataLoaderClientsService - Load. No se encuentra el sector {sectorToFind}", sectorToFind);
                        throw new Exception($"El sector {sectorToFind} del proyecto {project.Name} no está registrada en el sistema.");
                    }
                    var technologyIds = new List<int>();
                    if (!string.IsNullOrWhiteSpace(record.Technologies))
                    {
                        var technologiesToFind = record.Technologies.Split(_insideFiledDelimiter);

                        foreach (var technologyToFind in technologiesToFind)
                        {
                            var technology = await _dataLoaderTechnologiesService.GetTechnologyByNameAsync(technologyToFind);
                            if (technology == null)
                            {
                                _logger.LogError("DataLoaderClientsService - Load. No se encuentra la tecnología {technologyToFind}", technologyToFind);
                                throw new Exception($"La tecnología {technologyToFind} del proyecto {project.Name} no está registrada en el sistema.");
                            }
                            technologyIds.Add(technology.Id);
                        }
                    }
                    project.ClientId = client.Id;
                    project.SectorId = sector.Id;
                    project.TechnologyIds = technologyIds;
                    var projectCreated = await CreateAsync(project);
                }
            }
        }
        private async Task<ProjectGetDto> CreateAsync(ProjectPostDto project)
        {
            var requestMessage = new TfsApiRequestMessage<ProjectPostDto>(
                project,
                HttpMethod.Post,
                $"https://localhost:7097/api/v1/project");
            var response = await _httpClient.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
            {
                return await Task.FromResult<ProjectGetDto>(null);
            }
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ProjectGetDto>(responseBody);
            return result;
        }
    }
}

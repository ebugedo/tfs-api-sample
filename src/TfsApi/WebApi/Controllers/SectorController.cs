using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using Tfs.Api.Application.Dtos.Clients;
using Tfs.Api.Application.Dtos.Sectors;
using Tfs.Api.Application.Interfaces;

namespace Tfs.Api.WebApi.Controllers
{
	[Route("api/v1/sector")]
	[SwaggerTag("Métodos relativos a los sectores.")]
	public class SectorController : BaseController
	{
		private readonly ILogger<SectorController> _logger;
		private readonly ISectorService _service;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public SectorController(
			ILogger<SectorController> logger,
			ISectorService service,
			IWebHostEnvironment webHostEnvironment
			) : base()
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_service = service ?? throw new ArgumentNullException(nameof(service));
			_webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
		}
		[SwaggerOperation(
			Summary = "Este método crea un nuevo sector",
			Description = "Este método crea un nuevo sector",
			OperationId = "PostSectorAsync",
			Tags = new[] { "Sector" })]
		[HttpPost(Name = "PostSectorAsync")]
		[AllowAnonymous]
		public async Task<ActionResult<SectorGetDto>> PostAsync([FromBody] SectorPostDto sector)
		{
			using (_logger.BeginScope("PostSectorAsync"))
			{
				_logger.LogInformation("SectorController - PostAsync. Sector: {@sector}", sector);
				if (!_webHostEnvironment.IsDevelopment())
				{
					return Unauthorized("No tiene permisos para crear un nuevo sector");
				}
				var newSector = await _service.CreateAsync(sector);
				return CreatedAtRoute("GetSectorAsync", new { id = newSector.Id }, newSector);
			}
		}
		[SwaggerOperation(
					Summary = "Este método recupera un sector por nombre",
					Description = "Este método recupera un sector por nombre",
					OperationId = "GetSectorByNameAsync",
					Tags = new[] { "Sector" })]
		[HttpGet("by-name", Name = "GetSectorByNameAsync")]
		public async Task<ActionResult<SectorGetDto>> GetByNameAsync([FromQuery] string name)
		{
			using (_logger.BeginScope("GetSectorByNameAsync"))
			{
				_logger.LogInformation("SectorController - GetSectorByNameAsync. Sector name: {name}", name);
				var result = await _service.GetByNameAsync(name);
				if (result == null)
				{
					throw new KeyNotFoundException($"El sector de nombre {name} no está registrado en el sistema");
				}
				return Ok(result);
			}
		}
		[SwaggerOperation(
					Summary = "Este método recupera un sector",
					Description = "Este método recupera un sector<br />",
					OperationId = "GetSectorAsync",
					Tags = new[] { "Sector" })]
		[HttpGet("{id}", Name = "GetSectorAsync")]
		public async Task<ActionResult<SectorGetDto>> GetAsync(int id)
		{
			using (_logger.BeginScope("GetSectorAsync"))
			{
				_logger.LogInformation("SectorController - GetSectorAsync. Sector id: {id}", id);
				var result = await _service.GetAsync(id);
				if (result == null)
				{
					throw new KeyNotFoundException($"El sector de identificador {id} no está registrado en el sistema");
				}
				return Ok(result);
			}
		}
		[SwaggerOperation(
					Summary = "Este método recupera todos los sectores",
					Description = "Este método recupera todos los sectores<br />",
					OperationId = "GetSectorsAsync",
					Tags = new[] { "Sector" })]
		[SwaggerResponse((int)HttpStatusCode.InternalServerError,
					"Los serctores no se han recuperado porque se ha producido un error interno.<br />" +
					"Los valores posibles de Code y Message son:<br />" +
					"- Los HResult y Name de las excepciones de C#")]
		[SwaggerResponse((int)HttpStatusCode.OK,
					"Los sectores se han recuperado correctamente.<br />")]
		[HttpGet(Name = "GetSectorsAsync")]
		public async Task<ActionResult<IList<SectorGetDto>>> GetAsync()
		{
			using (_logger.BeginScope("GetSectorsAsync"))
			{
				_logger.LogInformation("SectorController - GetAsync.");
				var result = await _service.GetAllAsync();
				return Ok(result);
			}
		}
	}
}

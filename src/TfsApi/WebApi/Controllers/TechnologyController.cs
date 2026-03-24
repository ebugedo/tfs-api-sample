using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using Tfs.Api.Application.Dtos.Technologies;
using Tfs.Api.Application.Interfaces;

namespace Tfs.Api.WebApi.Controllers
{
	[Route("api/v1/technology")]
	[SwaggerTag("Métodos relativos a las tecnologías.")]
	public class TechnologyController : BaseController
	{
		private readonly ILogger<TechnologyController> _logger;
		private readonly ITechnologyService _service;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public TechnologyController(
			ILogger<TechnologyController> logger,
			ITechnologyService service,
			IWebHostEnvironment webHostEnvironment
			) : base()
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_service = service ?? throw new ArgumentNullException(nameof(service));
			_webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
		}
		[SwaggerOperation(
			Summary = "Este método crea una nueva tecnología",
			Description = "Este método crea una nueva tecnología",
			OperationId = "PostTechnologyAsync",
			Tags = new[] { "Technology" })]
		[HttpPost(Name = "PostTechnologyAsync")]
		[AllowAnonymous]
		public async Task<ActionResult<TechnologyGetDto>> PostAsync([FromBody] TechnologyPostDto technology)
		{
			using (_logger.BeginScope("PostTechnologyAsync"))
			{
				_logger.LogInformation("TechnologyController - PostAsync. Technology: {@technology}", technology);
				if (!_webHostEnvironment.IsDevelopment())
				{
					return Unauthorized("No tiene permisos para crear una nueva tecnología");
				}
				var newTechnology = await _service.CreateAsync(technology);
				return CreatedAtRoute("GetTechnologyAsync", new { id = newTechnology.Id }, newTechnology);
			}
		}
		[SwaggerOperation(
					Summary = "Este método recupera una tecnología por nombre",
					Description = "Este método recupera una tecnología por nombre",
					OperationId = "GetTechnologyByNameAsync",
					Tags = new[] { "Technology" })]
		[HttpGet("by-name", Name = "GetTechnologyByNameAsync")]
		public async Task<ActionResult<TechnologyGetDto>> GetByNameAsync([FromQuery] string name)
		{
			using (_logger.BeginScope("GetTechnologyByNameAsync"))
			{
				_logger.LogInformation("TechnologyController - GetTechnologyByNameAsync. Technology name: {name}", name);
				var result = await _service.GetByNameAsync(name);
				if (result == null)
				{
					throw new KeyNotFoundException($"La tecnología de nombre {name} no está registrada en el sistema");
				}
				return Ok(result);
			}
		}

		[SwaggerOperation(
					Summary = "Este método recupera una tecnología",
					Description = "Este método recupera una tecnología<br />",
					OperationId = "GetTechnologyAsync",
					Tags = new[] { "Technology" })]
		[HttpGet("{id}", Name = "GetTechnologyAsync")]
		public async Task<ActionResult<TechnologyGetDto>> GetAsync(int id)
		{
			using (_logger.BeginScope("GetTechnologyAsync"))
			{
				_logger.LogInformation("TechnologyController - GetTechnologyAsync. Technology id: {id}", id);
				var result = await _service.GetAsync(id);
				if (result == null)
				{
					throw new KeyNotFoundException($"La tecnología de identificador {id} no está registrada en el sistema");
				}
				return Ok(result);
			}
		}

		[SwaggerOperation(
						Summary = "Este método recupera todas las tecnologías",
						Description = "Este método recupera todas las tecnologías<br />",
						OperationId = "GetTechnologiesAsync",
						Tags = new[] { "Technology" })]
		[SwaggerResponse((int)HttpStatusCode.InternalServerError,
						"Las tecnologías no se han recuperado porque se ha producido un error interno.<br />" +
						"Los valores posibles de Code y Message son:<br />" +
						"- Los HResult y Name de las excepciones de C#")]
		[SwaggerResponse((int)HttpStatusCode.OK,
						"Las tecnologías se han recuperado correctamente.<br />")]
		[HttpGet(Name = "GetTechnologiesAsync")]
		public async Task<ActionResult<IList<TechnologyGetDto>>> GetAsync()
		{
			using (_logger.BeginScope("GetTechnologiesAsync"))
			{
				_logger.LogInformation("TechnologyController - GetAsync.");
				var result = await _service.GetAllAsync();
				return Ok(result);
			}
		}
	}
}

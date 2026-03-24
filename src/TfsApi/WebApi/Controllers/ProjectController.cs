using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using Tfs.Api.Application.Dtos.Clients;
using Tfs.Api.Application.Dtos.Projects;
using Tfs.Api.Application.Interfaces;

namespace Tfs.Api.WebApi.Controllers
{
	[Route("api/v1/project")]
	[SwaggerTag("Métodos relativos a los proyectos.")]
	public class ProjectController : BaseController
	{
		private readonly ILogger<ProjectController> _logger;
		private readonly IProjectService _service;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ProjectController(
			ILogger<ProjectController> logger,
			IProjectService service,
			IWebHostEnvironment webHostEnvironment
			) : base()
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_service = service ?? throw new ArgumentNullException(nameof(service));
			_webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
		}
		[SwaggerOperation(
			Summary = "Este método crea un nuevo proyecto",
			Description = "Este método crea un nuevo proyecto",
			OperationId = "PostProjectAsync",
			Tags = new[] { "Project" })]
		[HttpPost(Name = "PostProjectAsync")]
		[AllowAnonymous]
		public async Task<ActionResult<ProjectGetDto>> PostAsync([FromBody] ProjectPostDto project)
		{
			using (_logger.BeginScope("PostProjectAsync"))
			{
				_logger.LogInformation("ProjectController - PostAsync. Project: {@project}", project);
				if (!_webHostEnvironment.IsDevelopment())
				{
					return Unauthorized("No tiene permisos para crear un nuevo proyecto");
				}
				var newProject = await _service.CreateAsync(project);
				return CreatedAtRoute("GetProjectAsync", new { id = newProject.Id }, newProject);
			}
		}
		[SwaggerOperation(
					Summary = "Este método recupera un proyecto",
					Description = "Este método recupera un proyecto<br />",
					OperationId = "GetProjectAsync",
					Tags = new[] { "Project" })]
		[HttpGet("{id}", Name = "GetProjectAsync")]
		public async Task<ActionResult<ProjectGetDto>> GetAsync(int id)
		{
			using (_logger.BeginScope("GetProjectAsync"))
			{
				_logger.LogInformation("ProjectController - GetProjectAsync. Project id: {id}", id);
				var result = await _service.GetAsync(id);
				if (result == null)
				{
					throw new KeyNotFoundException($"El proyecto de identificador {id} no está registrado en el sistema");
				}
				return Ok(result);
			}
		}
		[SwaggerOperation(
					Summary = "Este método recupera todos los proyectos",
					Description = "Este método recupera todos los proyectos<br />",
					OperationId = "GetProjectsAsync",
					Tags = new[] { "Project" })]
		[SwaggerResponse((int)HttpStatusCode.InternalServerError,
					"Los proyectos no se han recuperado porque se ha producido un error interno.<br />" +
					"Los valores posibles de Code y Message son:<br />" +
					"- Los HResult y Name de las excepciones de C#")]
		[SwaggerResponse((int)HttpStatusCode.OK,
					"Los proyectos se han recuperado correctamente.<br />")]
		[HttpGet(Name = "GetProjectsAsync")]
		public async Task<ActionResult<IList<ProjectGetDto>>> GetAsync()
		{
			using (_logger.BeginScope("GetProjectsAsync"))
			{
				_logger.LogInformation("ProjectController - GetProjectsAsync.");
				var result = await _service.GetAllAsync();
				return Ok(result);
			}
		}
	}
}

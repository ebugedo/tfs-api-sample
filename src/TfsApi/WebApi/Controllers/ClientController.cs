using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using Tfs.Api.Application.Dtos.Clients;
using Tfs.Api.Application.Interfaces;

namespace Tfs.Api.WebApi.Controllers
{
	[Route("api/v1/client")]
	[SwaggerTag("Métodos relativos a los clientes.")]
	public class ClientController : BaseController
	{
		private readonly ILogger<ClientController> _logger;
		private readonly IClientService _service;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ClientController(
			ILogger<ClientController> logger,
			IClientService service,
			IWebHostEnvironment webHostEnvironment
			) : base()
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_service = service ?? throw new ArgumentNullException(nameof(service));
			_webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
		}
		[SwaggerOperation(
			Summary = "Este método crea un nuevo cliente",
			Description = "Este método crea un nuevo cliente",
			OperationId = "PostClientAsync",
			Tags = new[] { "Client" })]
		[HttpPost(Name = "PostClientAsync")]
		[AllowAnonymous]
		public async Task<ActionResult<ClientGetDto>> PostAsync([FromBody] ClientPostDto client)
		{
			using (_logger.BeginScope("PostClientAsync"))
			{
				_logger.LogInformation("ClientController - PostAsync. Client: {@Client}", client);
				//if (!_webHostEnvironment.IsDevelopment())
				//{
				//	return  Unauthorized("No tiene permisos para crear un nuevo cliente");
				//}
				var newClient = await _service.CreateAsync(client);
				return CreatedAtRoute("GetClientAsync", new { id = newClient.Id }, newClient);
			}
		}
		[SwaggerOperation(
					Summary = "Este método recupera un cliente",
					Description = "Este método recupera un cliente<br />",
					OperationId = "GetClientAsync",
					Tags = new[] { "Client" })]
		[HttpGet("{id}", Name = "GetClientAsync")]
		public async Task<ActionResult<ClientGetDto>> GetAsync(int id)
		{
			using (_logger.BeginScope("GetClientAsync"))
			{
				_logger.LogInformation("ClientController - GetClientAsync. Client id: {id}", id);
				var result = await _service.GetAsync(id);
				if (result == null)
				{
					throw new KeyNotFoundException($"El proyecto de identificador {id} no está registrado en el sistema");
				}
				return Ok(result);
			}
		}
		[SwaggerOperation(
					Summary = "Este método recupera todos los clientes",
					Description = "Este método recupera todos los clientes<br />",
					OperationId = "GetClientsAsync",
					Tags = new[] { "Client" })]
		[SwaggerResponse((int)HttpStatusCode.InternalServerError,
					"Los clientes no se han recuperado porque se ha producido un error interno.<br />" +
					"Los valores posibles de Code y Message son:<br />" +
					"- Los HResult y Name de las excepciones de C#")]
		[SwaggerResponse((int)HttpStatusCode.OK,
					"Los clientes se han recuperado correctamente.<br />")]
		[HttpGet(Name = "GetClientsAsync")]
		public async Task<ActionResult<IList<ClientGetDto>>> GetAsync()
		{
			using (_logger.BeginScope("GetClientsAsync"))
			{
				_logger.LogInformation("ClientController - GetClientsAsync.");
				var result = await _service.GetAllAsync();
				return Ok(result);
			}
		}
		[SwaggerOperation(
					Summary = "Este método recupera un cliente por nombre",
					Description = "Este método recupera un cliente por nombre",
					OperationId = "GetClientByNameAsync",
					Tags = new[] { "Client" })]
		[HttpGet("by-name", Name = "GetClientByNameAsync")]
		public async Task<ActionResult<ClientGetDto>> GetByNameAsync([FromQuery] string name)
		{
			using (_logger.BeginScope("GetClientByNameAsync"))
			{
				_logger.LogInformation("ClientController - GetClientByNameAsync. Client name: {name}", name);
				var result = await _service.GetByNameAsync(name);
				if (result == null)
				{
					throw new KeyNotFoundException($"El cliente de nombre {name} no está registrado en el sistema");
				}
				return Ok(result);
			}
		}
	}
}

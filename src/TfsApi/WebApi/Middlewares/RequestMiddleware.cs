using Tfs.Api.Application.Dtos.RequestTrace;
using Tfs.Api.Application.Interfaces;
using static System.Net.Mime.MediaTypeNames;
namespace WebApi.Middlewares
{

	public class RequestMiddleware
	{
        private readonly ILogger<RequestMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly IRequestTraceService _service;

        public RequestMiddleware(
            ILogger<RequestMiddleware> logger,
            IRequestTraceService service,
		RequestDelegate next)
		{
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _next = next;
			_service = service ?? throw new ArgumentNullException(nameof(service));
		}

		public async Task InvokeAsync(HttpContext context
            )
		{
            //_logger.LogInformation("RequestMiddleware - InvokeAsync. Context: {@context}", context);
            _logger.LogInformation("RequestMiddleware - InvokeAsync.");
            // Lógica de middleware antes de pasar al siguiente middleware en el pipeline
            var apiRequest = new RequestTracePostDto()
			{
				IPAddress = GetClientIp(context)
			};
			var apiRequestCreated=await _service.CreateOrUpdateAsync(apiRequest);
			// Ejemplo: Registro de la información de la solicitud
			var requestPath = context.Request.Path;
			var requestMethod = context.Request.Method;
            _logger.LogInformation("RequestMiddleware - InvokeAsync. Request: {requestMethod} {requestPath}", requestMethod, requestPath);

			// Pasar al siguiente middleware en el pipeline
			await _next(context);

			// Lógica de middleware después de que se ha completado la solicitud y se ha obtenido la respuesta
		}
		private string GetClientIp(HttpContext context)
		{
			// Intentar obtener la dirección IP del encabezado X-Forwarded-For (en caso de estar detrás de un proxy)
			var forwardedIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

			// Si el encabezado no está presente, obtener la dirección IP del cliente directamente
			var ipAddress = string.IsNullOrEmpty(forwardedIp) ? context.Connection.RemoteIpAddress?.ToString() : forwardedIp;

			return ipAddress ?? "Unknown"; // Devolver "Unknown" si no se puede determinar la dirección IP
		}
	}


}

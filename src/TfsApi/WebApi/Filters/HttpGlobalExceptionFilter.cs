using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace TfsApi.WebApi.Extensions
{
	public class HttpGlobalExceptionFilter<T> : IExceptionFilter where T : Exception
	{
		private readonly IHostEnvironment _environment;
		private readonly ILogger<HttpGlobalExceptionFilter<T>> _logger;
		public HttpGlobalExceptionFilter(
			IHostEnvironment environment,
			ILogger<HttpGlobalExceptionFilter<T>> logger)
		{
			_environment = environment ?? throw new ArgumentNullException(nameof(environment));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}
		public void OnException(ExceptionContext context)
		{
			_logger.LogError(new EventId(context.Exception.HResult),
				context.Exception,
				context.Exception.Message);
			context.Result = new ObjectResult(context.Exception);
			// NotFound
			if (context.Exception.GetType() == typeof(KeyNotFoundException))
			{
				context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
			}
			// BadRequest
			else if (
				context.Exception.GetType() == typeof(ArgumentException) ||
				context.Exception.GetType() == typeof(ArgumentNullException)
				// Visits
				//context.Exception.GetType() == typeof(PreviousVisitDoesntExistException) ||
				//context.Exception.GetType() == typeof(VisitAssistantDoesntExistException) ||
				//context.Exception.GetType() == typeof(VisitCampaignDoesntExistException) ||
				//context.Exception.GetType() == typeof(VisitCompanyProvinceDoesntExistException) ||
				//context.Exception.GetType() == typeof(VisitConstructionSiteProvinceDoesntExistException) ||
				//context.Exception.GetType() == typeof(VisitSecondAssistantDoesntExistException) ||
				//context.Exception.GetType() == typeof(VisitSurveyDoesntExistException) ||
				//context.Exception.GetType() == typeof(VisitTerritorialScopeDoesntExistException) ||
				//context.Exception.GetType() == typeof(VisitVisitorDoesntExistException)
				)
			{
				context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
			}
			// Errores de negocio
			else if (
				false
				//// Surveys
				//context.Exception.GetType() == typeof(SurveyCantBeModifiedBecauseIsActiveException) ||
				//context.Exception.GetType() == typeof(SurveyCantBeModifiedBecauseSameSerializedSurveyException) ||
				//context.Exception.GetType() == typeof(SurveyIsAlreadyActivatedException) ||
				//// Visits
				//context.Exception.GetType() == typeof(TooManyRevisitsException) ||
				//context.Exception.GetType() == typeof(VisitCodeGenerationException) ||
				//context.Exception.GetType() == typeof(VisitSurveyCampaignIsNotActiveException)
				)
			{
				context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
			}
			// Otros errores
			else if (context.Exception.GetType() == typeof(Exception))
			{
				context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			}
			else
			{
				context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			}
			context.ExceptionHandled = true;
		}
	}
}

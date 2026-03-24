using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
namespace TfsApi.WebApi.Extensions
{
	public class HttpResponseOuputFilter : ActionFilterAttribute
	{
		public override async void OnResultExecuting(ResultExecutingContext context)
		{
			//if (context == null)
			//{
			//	throw new ArgumentNullException(nameof(context));
			//}
			//if (!context.ModelState.IsValid)
			//{
			//	context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
			//	var modelStateErrors = new ModelStateErrors(context.ModelState);
			//	if (!modelStateErrors.ComeFromApiController)
			//	{
			//		context.Result = new ObjectResult(new ResponseBodyDto<string>(new ArgumentException(modelStateErrors.ToString())));
			//	}
			//	else
			//	{
			//		context.Result = new ObjectResult(new ResponseBodyDto<string>(new ArgumentNullException(modelStateErrors.ToString())));
			//	}
			//}
			base.OnResultExecuting(context);
		}
	}
}
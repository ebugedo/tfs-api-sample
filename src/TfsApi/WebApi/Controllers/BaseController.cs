using Microsoft.AspNetCore.Mvc;
namespace Tfs.Api.WebApi.Controllers
{
	[ApiController]
	public class BaseController : ControllerBase
	{
		//// De momento no lo uso, sólo el método IsDevelopment()
		//private readonly IWebHostEnvironment _webHostEnvironment;
		//public BaseController(
		//	IWebHostEnvironment webHostEnvironment
		//	)
		//{
		//	_webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
		//}

		public BaseController() { }
	}
}

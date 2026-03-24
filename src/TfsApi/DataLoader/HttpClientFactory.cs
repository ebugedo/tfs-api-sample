using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
namespace DataLoader
{


	public class HttpClientFactory : IHttpClientFactory
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _configuration;


		public HttpClientFactory(IHttpClientFactory httpClientFactory, IConfiguration configuration)
		{
			_httpClientFactory = httpClientFactory;
			_configuration = configuration;
			
		}

		public HttpClient CreateClient(string name)
		{
			var client = _httpClientFactory.CreateClient(name);
			var httpClientConfig = _configuration.GetSection($"HttpClientSettings:{name}");

			if (httpClientConfig != null)
			{
				client.BaseAddress = new Uri(httpClientConfig["BaseAddress"]);
				var timeoutSeconds = int.Parse(httpClientConfig["TimeoutSeconds"]);
				client.Timeout = System.TimeSpan.FromSeconds(timeoutSeconds);
				// Puedes agregar otras configuraciones según tus necesidades
			}
			return client;
		}
		
	}

}

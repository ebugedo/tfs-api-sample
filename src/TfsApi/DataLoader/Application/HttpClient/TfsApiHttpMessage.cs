using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
namespace DataLoader.Application.Dtos
{
	public class TfsApiRequestMessage<T>: HttpRequestMessage
	{
		private readonly JsonSerializerOptions _serializeOptions;
		public TfsApiRequestMessage(T? content, HttpMethod method, string url): base(method, url)
		{
			_serializeOptions = new JsonSerializerOptions
			{
				Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
			};
			if (content != null)
			{
				var contentSerialized = JsonSerializer.Serialize<T>(content, _serializeOptions);
				Content = new StringContent(contentSerialized, Encoding.UTF8, "application/json");
			}
		}

		//public HttpRequestMessage GetRequestMessage(T? content, HttpMethod method, string url)
		//{
		//	var httpRequestMessage = new HttpRequestMessage(method, url);
		//	if (content != null)
		//	{
		//		var contentSerialized = JsonSerializer.Serialize<T>(content, _serializeOptions);
		//		httpRequestMessage.Content = new StringContent(contentSerialized, Encoding.UTF8, "application/json");
		//	}
		//	return httpRequestMessage;
		//}
	}
}

using System.Text.Json.Serialization;
namespace Tfs.Api.Application.Dtos.RequestTrace
{
    public class RequestTraceBaseDto
	{
        [JsonPropertyName("ip")]
        public string IPAddress { get; set; }


    }
}

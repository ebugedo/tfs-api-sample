using System.Text.Json.Serialization;

namespace Tfs.Api.Application.Dtos.RequestTrace
{
    public class RequestTracePostDto : RequestTraceBaseDto
	{
        [JsonPropertyName("userIdOfCreation")]
        public int UserIdOfCreation { get; set; }
    }
}

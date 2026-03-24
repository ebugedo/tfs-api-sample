using System.Text.Json.Serialization;
namespace Tfs.Api.Application.Dtos.Clients
{
    public class ClientBaseDto
	{
        [JsonPropertyName("name")]
        public string Name { get; set; }

    }
}

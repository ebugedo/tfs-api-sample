using System.Text.Json.Serialization;
namespace Tfs.Api.Application.Dtos.Clients
{
    public class ClientGetDto : ClientBaseDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}

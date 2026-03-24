using System.Text.Json.Serialization;
namespace Tfs.Api.Application.Dtos.Technologies
{
    public class TechnologyGetDto : TechnologyBaseDto
	{
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}

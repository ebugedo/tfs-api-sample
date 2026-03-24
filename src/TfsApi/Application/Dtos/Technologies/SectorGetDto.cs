using System.Text.Json.Serialization;
namespace Tfs.Api.Application.Dtos.Sectors
{
    public class SectorGetDto : SectorBaseDto
	{
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}

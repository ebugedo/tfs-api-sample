using System.Text.Json.Serialization;
namespace Tfs.Api.Application.Dtos.Sectors
{
    public class SectorBaseDto
	{
		[JsonPropertyName("name")]
        public string Name { get; set; }

    }
}

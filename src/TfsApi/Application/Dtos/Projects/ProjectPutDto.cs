using System.Text.Json.Serialization;
namespace Tfs.Api.Application.Dtos.Projects
{
    public class ProjectPutDto : ProjectBaseDto
	{
		[JsonPropertyName("clientId")]
		public int ClientId { get; set; }
		[JsonPropertyName("sectorId")]
		public int SectorId { get; set; }
		[JsonPropertyName("technologies")]
		public ICollection<int> ProjectTechnologiesIds { get; set; } = new List<int>();
	}
}

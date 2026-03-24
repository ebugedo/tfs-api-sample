using System.Text.Json.Serialization;
namespace Tfs.Api.Application.Dtos.Projects
{
    public class ProjectPostDto : ProjectBaseDto
	{
		[JsonPropertyName("clientId")]
		public int ClientId { get; set; }
		[JsonPropertyName("sectorId")]
		public int SectorId { get; set; }
		[JsonPropertyName("technologies")]
		public ICollection<int> TechnologyIds { get; set; }= new List<int>();
	}
}

using System.Text.Json.Serialization;
namespace Tfs.Api.Application.Dtos.ProjectTechnologies
{
    public class ProjectTechnologyGetDto : ProjectTechnologyBaseDto
	{
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}

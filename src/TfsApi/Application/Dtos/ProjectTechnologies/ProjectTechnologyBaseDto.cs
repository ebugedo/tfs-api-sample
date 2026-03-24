using System.Text.Json.Serialization;
namespace Tfs.Api.Application.Dtos.ProjectTechnologies
{
    public class ProjectTechnologyBaseDto
	{
        [JsonPropertyName("name")]
        public string Name { get; set; }

    }
}

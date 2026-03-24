using System.Text.Json.Serialization;
namespace Tfs.Api.Application.Dtos.Technologies
{
    public class TechnologyBaseDto
	{
		[JsonPropertyName("description")]
		public string Description { get; set; }
		[JsonPropertyName("name")]
        public string Name { get; set; }

    }
}

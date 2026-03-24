using System.Text.Json.Serialization;
namespace Tfs.Api.Application.Dtos.Projects
{
    public class ProjectBaseDto
	{
		[JsonPropertyName("description")]
		public string Description { get; set; }
        [JsonPropertyName("durationInMonths")]
        public int? DurationInMonths { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("startDateAndTime")]
        public DateTime? StartDateAndTime { get; set; }
    }
}

using System.Text.Json.Serialization;
using Tfs.Api.Application.Dtos.Clients;
using Tfs.Api.Application.Dtos.Sectors;
using Tfs.Api.Application.Dtos.Technologies;

namespace Tfs.Api.Application.Dtos.Projects
{
    public class ProjectGetDto : ProjectBaseDto
    {
		[JsonPropertyName("client")]
		public ClientGetDto Client { get; set; }

		[JsonPropertyName("id")]
        public int Id { get; set; }
		[JsonPropertyName("sector")]
		public SectorGetDto Sector { get; set; }

		[JsonPropertyName("technologies")]
		public ICollection<TechnologyGetDto> Technologies { get; set; }=new List<TechnologyGetDto>();

	}
}

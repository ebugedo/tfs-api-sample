using Domain.Common;
using Tfs.Api.Domain.AggretateModels;
using Tfs.Api.Domain.Interfaces;
namespace Domain.AggregateModels.Projects
{
    public class Project : EntityBase<int>, IAggregateRoot
    {
        public Client Client { get; private set; }
        public int ClientId { get; private set; }
		public string Description { get; private set; }
        public int? DurationInMonths { get; private set; }
        public string Name { get; private set; }
        public ICollection<ProjectTechnology> ProjectTechnologies { get; private set; }
        public Sector Sector { get; private set; }
        public int SectorId { get; private set; }
        public DateTime? StartDateAndTime { get; private set; }
        public Project(
                    int clientId,
					string description,
                    int? durationInMonths,
					string name,
					int sectorId,
                    DateTime? startDateAndTime
            )
		{
            ClientId = clientId;
			Description = description;
            DurationInMonths = durationInMonths;
			Name = name;
			ProjectTechnologies = new List<ProjectTechnology>();
			SectorId = sectorId;
            StartDateAndTime = startDateAndTime;
        }
        
        public void AddProjectTechnology(int technologyId)
        {
            if (ProjectTechnologies == null)
            {
				ProjectTechnologies = new List<ProjectTechnology>();
			}
			ProjectTechnologies.Add(new ProjectTechnology(technologyId));
		}
	}
}
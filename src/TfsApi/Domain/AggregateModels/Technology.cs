using Domain.Common;

namespace Domain.AggregateModels
{
    public class Technology : EntityBase<int>
	{
		public string Description { get; private set; }
		public string Name { get; private set; }
		//public ICollection<ProjectTechnology> ProjectTechnologies { get; private set; }
		public Technology(string description, string name)
		{
			Description = description;
			Name = name;
		}
	}
}

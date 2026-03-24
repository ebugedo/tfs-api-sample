using Domain.Common;

namespace Domain.AggregateModels.Projects
{
    public class ProjectTechnology : EntityBase<int>
    {
        public Project Project { get; private set; }
        public int ProjectId { get; private set; }
        public Technology Technology { get; private set; }
        public int TechnologyId { get; private set; }
        public ProjectTechnology(int technologyId){
			TechnologyId = technologyId;
		}
		//public ProjectTechnology(
		//							int projectId,
		//							int technologyId
		//						)
		//{
		//	ProjectId = projectId;
		//	TechnologyId = technologyId;
		//}
	}
}

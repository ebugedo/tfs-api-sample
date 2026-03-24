using Domain.Common;

namespace Tfs.Api.Domain.AggretateModels
{
    public class Sector : EntityBase<int>
	{
		public string Name { get; private set; }
		public Sector(string name)
		{
			Name = name;
		}
	}
}

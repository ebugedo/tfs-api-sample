using Domain.Common;

namespace Tfs.Api.Domain.AggretateModels
{
    public class Client : EntityBase<int>
	{
		public string Name { get; private set; }
		public Client(string name)
		{
			Name = name;
		}
	}
}

using AutoMapper;
using Domain.AggregateModels;
using Tfs.Api.Application.Dtos.Technologies;
namespace Tfs.Api.Application.Mappers
{
	/// <summary>
	/// 
	/// </summary>
	public class TechnologyProfile : Profile
	{
		/// <summary>
		/// 
		/// </summary>
		public override string ProfileName => "TechnologyProfile";

		/// <summary>
		/// Inicializa una nueva instancia de la clase <see cref="TechnologyProfile"/>.
		/// </summary>
		public TechnologyProfile()
		{
			CreateMap<Technology, TechnologyGetDto>();
			CreateMap<TechnologyPostDto, Technology>()
				.ConstructUsing(src =>
					new Technology(
						src.Description, 
						src.Name
						))
				.ForAllMembers(opt => opt.Ignore());
		}
	}
}

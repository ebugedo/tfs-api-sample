using AutoMapper;
using Tfs.Api.Application.Dtos.Sectors;
using Tfs.Api.Domain.AggretateModels;

namespace Tfs.Api.Application.Mappers
{
	/// <summary>
	/// 
	/// </summary>
	public class SectorProfile : Profile
	{
		/// <summary>
		/// 
		/// </summary>
		public override string ProfileName => "SectorProfile";

		/// <summary>
		/// Inicializa una nueva instancia de la clase <see cref="SectorProfile"/>.
		/// </summary>
		public SectorProfile()
		{
			CreateMap<Sector, SectorGetDto>();
			CreateMap<SectorPostDto, Sector>()
				.ConstructUsing(src =>
					new Sector(
						src.Name
						))
				.ForAllMembers(opt => opt.Ignore());
		}
	}
}

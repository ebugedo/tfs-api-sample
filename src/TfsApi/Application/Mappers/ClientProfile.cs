using AutoMapper;
using Tfs.Api.Application.Dtos.Clients;
using Tfs.Api.Domain.AggretateModels;

namespace Tfs.Api.Application.Mappers
{
	/// <summary>
	/// 
	/// </summary>
	public class ClientProfile : Profile
	{
		/// <summary>
		/// 
		/// </summary>
		public override string ProfileName => "ClientProfile";

		/// <summary>
		/// Inicializa una nueva instancia de la clase <see cref="ClientProfile"/>.
		/// </summary>
		public ClientProfile()
		{
			CreateMap<Client, ClientGetDto>();
			CreateMap<ClientPostDto, Client>()
				.ConstructUsing(src =>
					new Client(
						src.Name
						))
				.ForAllMembers(opt => opt.Ignore());
		}
	}
}

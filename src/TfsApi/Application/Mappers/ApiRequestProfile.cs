using AutoMapper;
using Tfs.Api.Application.Dtos.RequestTrace;
using Tfs.Api.Domain.AggretateModels;
namespace Tfs.Api.Application.Mappers
{
	/// <summary>
	/// 
	/// </summary>
	public class RequestTraceProfile : Profile
	{
		/// <summary>
		/// 
		/// </summary>
		public override string ProfileName => "RequestTraceProfile";

		/// <summary>
		/// Inicializa una nueva instancia de la clase <see cref="RequestTraceProfile"/>.
		/// </summary>
		public RequestTraceProfile()
		{

			CreateMap<RequestTrace, RequestTraceGetDto>();
            CreateMap<RequestTraceGetDto, RequestTrace>()
                                .ConstructUsing(src =>
                                new RequestTrace(
                                    src.IPAddress,
                                    src.NumberOfRequests
                                    ))
                            .ForAllMembers(opt => opt.Ignore());
            CreateMap<RequestTracePostDto, RequestTrace>()
				.ConstructUsing(src =>
					new RequestTrace(
						src.IPAddress
						))
				.ForAllMembers(opt => opt.Ignore());
			CreateMap<RequestTracePutDto, RequestTrace>()
							.ConstructUsing(src =>
								new RequestTrace(
									src.IPAddress,
									src.NumberOfRequests
									))
							.ForAllMembers(opt => opt.Ignore());
		}
	}
}

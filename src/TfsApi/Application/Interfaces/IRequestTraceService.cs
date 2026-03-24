using Tfs.Api.Application.Dtos.RequestTrace;
namespace Tfs.Api.Application.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRequestTraceService
    {
		Task<RequestTraceGetDto> CreateOrUpdateAsync(RequestTracePostDto requestTrace);
		//Task<IList<ClientGetDto>> GetAllAsync();
		//Task<ClientGetDto> GetAsync(int id);
		//Task<ClientGetDto> GetByNameAsync(string name);
	}
}

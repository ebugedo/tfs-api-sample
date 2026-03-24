using Tfs.Api.Application.Dtos.Clients;
namespace Tfs.Api.Application.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IClientService
    {
		Task<ClientGetDto> CreateAsync(ClientPostDto client);
		Task<IList<ClientGetDto>> GetAllAsync();
		Task<ClientGetDto> GetAsync(int id);
		Task<ClientGetDto> GetByNameAsync(string name);
	}
}

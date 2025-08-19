using LastDanceAPI.Entities;
using LastDanceAPI.DTO;

namespace LastDanceAPI.Services
{
    public interface IClientService
    {
        Task<IEnumerable<Clients>> GetAllClientsNotDeletedAsync();

        Task<Clients?>SaveAsync(Clients client);

        Task<Clients?> UpdateAsync(Clients client);

        Task<bool> SoftDeleteAsync(ClientDeleteDto dto);
    }
}
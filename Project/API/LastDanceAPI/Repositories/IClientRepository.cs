using LastDanceAPI.DTO;
using LastDanceAPI.Entities;
using System.Threading.Tasks;

namespace LastDanceAPI.Repositories
{
    public interface IClientRepository
    {
        Task<IEnumerable<Clients>> GetAllClientsNotDeletedAsync();
        Task<Clients?> SaveAsync(Clients client);
        Task<Clients?> UpdateAsync(Clients client);
        Task<bool> SoftDeleteAsync(ClientDeleteDto dto);
        
    }
}
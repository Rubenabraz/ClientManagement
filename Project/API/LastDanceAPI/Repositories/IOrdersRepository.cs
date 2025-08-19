using LastDanceAPI.DTO;
using LastDanceAPI.Entities;

namespace LastDanceAPI.Repositories
{
    public interface IOrdersRepository
    {
        Task<IEnumerable<Orders>> GetAllOrdersNotDeletedAsync();
        Task<Orders?>SaveAsync(Orders orders);
        Task<Orders?> UpdateAsync(Orders orders);
        Task<bool>SoftDeleteAsync(OrdersDeleteDto dto);
    }
}

using LastDanceAPI.DTO;
using LastDanceAPI.Entities;

namespace LastDanceAPI.Services
{
    public interface IOrdersService
    {
        Task<IEnumerable<Orders>> GetAllOrdersNotDeletedAsync();
        Task<Orders?> SaveAsync(Orders orders);
        Task<Orders?> UpdateAsync(Orders order);
        Task<bool>SoftDeleteAsync(OrdersDeleteDto dto);
    }
}

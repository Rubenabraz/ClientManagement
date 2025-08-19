
using LastDanceAPI.DTO;
using LastDanceAPI.Entities;
using LastDanceAPI.Repositories;

namespace LastDanceAPI.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _repository;

        public OrdersService(IOrdersRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Orders>> GetAllOrdersNotDeletedAsync()
        {
            return await _repository.GetAllOrdersNotDeletedAsync();
        }

        public async Task<Orders?> SaveAsync(Orders orders)
        {
            return await _repository.SaveAsync(orders);
        }

        public async Task<Orders?>UpdateAsync(Orders orders)
        {
            return await _repository.SaveAsync(orders);
        }

        public async Task<bool> SoftDeleteAsync(OrdersDeleteDto dto)
        {
            return await _repository.SoftDeleteAsync(dto);
        }


    }
}

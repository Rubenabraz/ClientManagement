using LastDanceAPI.DTO;
using LastDanceAPI.Entities;
using LastDanceAPI.Repositories;
using LastDanceAPI.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _repository;

    public ClientService(IClientRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Clients>> GetAllClientsNotDeletedAsync()
    {
        return await _repository.GetAllClientsNotDeletedAsync();
    }

    public async Task<Clients?> SaveAsync(Clients client)
    {
        return await _repository.SaveAsync(client);
    }

    public async Task<Clients?> UpdateAsync(Clients client)
    {
        return await _repository.UpdateAsync(client);
    }

    public async Task<bool> SoftDeleteAsync(ClientDeleteDto dto)
    {
        return await _repository.SoftDeleteAsync(dto);
    }
}
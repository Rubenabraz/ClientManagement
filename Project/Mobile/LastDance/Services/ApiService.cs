using System.Net.Http.Json;
using LastDance.DTOs;

namespace LastDance.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;

    private readonly HttpClient _httpOrders;

    public ApiService()
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        _httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://10.0.2.2:7197/")
        };

        _httpOrders = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://10.0.2.2:7197/")
        };
    }

    public async Task<List<ClientDto>> GetClients()
    {
        try
        {
            var clients = await _httpClient.GetFromJsonAsync<List<ClientDto>>("clients");
            return clients ?? new List<ClientDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao obter clientes: {ex.Message}");
            return new List<ClientDto>();
        }
    }

    public async Task<List<ClientDto>> GetDeletedClients()
    {
        try
        {
            var clients = await _httpClient.GetFromJsonAsync<List<ClientDto>>("clients");

            return clients?
                .Where(c => c.cltStatus == "removido")
                .ToList()
                ?? new List<ClientDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao obter clientes: {ex.Message}");
            throw;
        }
    }

    public async Task<ClientDto> GetClientById(int id)
    {
        try
        {
            var client = await _httpClient.GetFromJsonAsync<ClientDto>($"clients/{id}");

            if (client == null || client.cltStatus?.ToLower() == "removido")
            {
                return null;
            }
            return client ?? new ClientDto();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao obter cliente: {ex.Message}");
            throw;
        }
    }

    public async Task<ClientDto> AddClient(ClientDto newClient)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("clients", newClient);

            response.EnsureSuccessStatusCode();

            var createdClient = await response.Content.ReadFromJsonAsync<ClientDto>();

            return createdClient ?? new ClientDto();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao adicionar cliente: {ex.Message}");
            throw;
        }
    }

    public async Task<ClientDto> UpdateClient(int id, ClientUpdateDto updatedClient)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"clients/{id}", updatedClient);

            response.EnsureSuccessStatusCode();

            var client = await response.Content.ReadFromJsonAsync<ClientDto>();

            return client ?? new ClientDto();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao atualizar cliente: {ex.Message}");
            throw;
        }
    }

    public async Task<ClientDeleteDto?> DeleteClient(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"clients/{id}");
            response.EnsureSuccessStatusCode();

            // Se a API devolver um JSON com o cliente apagado
            if (response.Content.Headers.ContentLength > 0)
            {
                var deletedClient = await response.Content.ReadFromJsonAsync<ClientDeleteDto>();
                return deletedClient ?? new ClientDeleteDto();
            }

            // Se não devolver nada (caso de NoContent)
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao apagar cliente: {ex.Message}");
            throw;
        }
    }

    //Orders deleted

    public async Task<List<OrderDto>> GetDeletedOrders()
    {
        try
        {
            var orders = await _httpOrders.GetFromJsonAsync<List<OrderDto>>("orders");

            return orders?
            .Where(c => c.ordStatus == "removido")
            .ToList()
            ?? new List<OrderDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao obter orders: {ex.Message}");
            throw;
        }
    }


    // orders gerais;
    public async Task<List<OrderDto>>GetOrders()
    {
        try
        {
            var orders = await _httpOrders.GetFromJsonAsync<List<OrderDto>>("ordersNotDeleted");
            return orders ?? new List<OrderDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao obter orders: {ex.Message}");
            throw;
        }
    }

    public async Task<OrderDto>GetOrdersById(int id)
    {
        try
        {
            var orders = await _httpOrders.GetFromJsonAsync<OrderDto>($"orders/{id}");
            return orders ?? new OrderDto();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao obter order por ID {id}: {ex.Message}");
            throw;
        }
    }

    public async Task<OrderDto> GetOrdersByClient(int id)
    {
        try
        {
            var order = await _httpOrders.GetFromJsonAsync<OrderDto>($"orders/{id}");

            if (order == null || order.ordStatus?.ToLower() == "removido")
                return null;

            return order;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao obter order: {ex.Message}");
            throw;
        }
    }

    public async Task<OrderDto> AddOrder(OrderDto newOrder)
    {
        try
        {
            var response = await _httpOrders.PostAsJsonAsync("orders", newOrder);

            response.EnsureSuccessStatusCode();

            var createdOrder = await response.Content.ReadFromJsonAsync<OrderDto>();

            return createdOrder ?? new OrderDto();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao adicionar order: {ex.Message}");
            throw;
        }
    }
    public async Task<OrderDto> UpdateOrder(int id, OrderUpdateDto updatedOrder)
    {
        try
        {
            var response = await _httpOrders.PutAsJsonAsync($"orders/{id}", updatedOrder);

            response.EnsureSuccessStatusCode();

            var order = await response.Content.ReadFromJsonAsync<OrderDto>();

            return order ?? new OrderDto();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao atualizar order: {ex.Message}");
            throw;
        }
    }
    public async Task<OrderDeleteDto?> DeleteOrder(int id)
    {
        try
        {
            var response = await _httpOrders.DeleteAsync($"orders/{id}");
            response.EnsureSuccessStatusCode();


            if (response.Content.Headers.ContentLength > 0)
            {
                var deletedOrders = await response.Content.ReadFromJsonAsync<OrderDeleteDto>();
                return deletedOrders ?? new OrderDeleteDto();
            }

            // Se não devolver nada
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao apagar order: {ex.Message}");
            throw;
        }
    }
        public async Task<List<OrderDto>> GetDeliveredOrders()
        {
        try
        {
            var orders = await _httpOrders.GetFromJsonAsync<List<OrderDto>>("orders");
            return orders?
                .Where(o => o.ordStatus?.ToLower() == "entregue")
                .ToList()
                ?? new List<OrderDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao obter encomendas entregues: {ex.Message}");
            throw;
        }
    }
}


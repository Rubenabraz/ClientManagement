using LastDance.DTOs;
using LastDance.Services;
using System.Collections.ObjectModel;

namespace LastDance.ViewModels;

public partial class DeletedOrders : ContentPage
{
    private readonly ApiService _apiService;
    private ObservableCollection<OrderDto> _deletedOrders;

    public DeletedOrders()
    {
        InitializeComponent();
        _apiService = new ApiService();
        _deletedOrders = new ObservableCollection<OrderDto>();
        DeletedOrdersListView.ItemsSource = _deletedOrders;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            var apiOrders = await _apiService.GetDeletedOrders();

            var clients = await _apiService.GetClients();
            var deletedClients = await _apiService.GetDeletedClients();

            // junta todos (ativos + removidos)
            var allClients = clients.Concat(deletedClients).ToList();

            _deletedOrders.Clear();

            foreach (var order in apiOrders.Where(o => o.ordStatus == "removido"))
            {
                var client = allClients.FirstOrDefault(c => c.cltID == order.ordClientID);

                if (client == null)
                {
                    order.ClientName = "Cliente não encontrado";
                }
                else if (client.cltStatus == "removido")
                {
                    order.ClientName = $"{client.cltName} {client.cltSurname}";
                }
                else
                {
                    order.ClientName = $"{client.cltName} {client.cltSurname}".Trim();
                }

                _deletedOrders.Add(order);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DEBUG] Erro ao carregar pedidos: {ex.Message}");
        }
    }
}

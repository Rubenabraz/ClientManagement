using LastDance.DTOs;
using LastDance.Services;
using System.Collections.ObjectModel;

namespace LastDance.ViewModels;

public partial class DeliveredOrders : ContentPage
{
    private readonly ApiService _apiService;

    public ObservableCollection<OrderDto> DeliveredOrdersList { get; set; } = new();

    public DeliveredOrders()
    {
        InitializeComponent();
        _apiService = new ApiService();

        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            var orders = await _apiService.GetDeliveredOrders();

            var clients = await _apiService.GetClients();

            var deletedClients = await _apiService.GetDeletedClients();

            var allClients = clients.Concat(deletedClients).ToList();

            DeliveredOrdersList.Clear();

            foreach (var order in orders)
            {
                var client = allClients.FirstOrDefault(c => c.cltID == order.ordClientID);

                if (client == null)
                {
                    order.ClientName = "Cliente não encontrado";
                }
                else if (client.cltStatus == "removido")
                {
                    order.ClientName = $"{client.cltName} {client.cltSurname} (Removido)";
                }
                else
                {
                    order.ClientName = $"{client.cltName} {client.cltSurname}".Trim();
                }

                DeliveredOrdersList.Add(order);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível carregar as encomendas entregues: {ex.Message}", "OK");
        }
    }
}
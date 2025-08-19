using LastDance.DTOs;
using LastDance.Services;
using System.Collections.ObjectModel;

namespace LastDance.ViewModels;

public partial class LoadOrders : ContentPage
{
    private readonly ApiService _apiService;
    private ObservableCollection<OrderDto> _orders;

    public LoadOrders()
    {
        InitializeComponent();
        _apiService = new ApiService();
        _orders = new ObservableCollection<OrderDto>();
        OrdersListView.ItemsSource = _orders;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            var apiOrders = await _apiService.GetOrders();

            var clients = await _apiService.GetClients();

            var deletedClients = await _apiService.GetDeletedClients();

            var allClients = clients.Concat(deletedClients).ToList();

            _orders.Clear();

            foreach (var order in apiOrders)
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

                _orders.Add(order);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DEBUG] Erro ao carregar orders: {ex.Message}");
        }
    }

    private async void UpdateOrder(object sender, EventArgs e)
    {
        if (sender is Label label && label.BindingContext is OrderDto order)
        {
            await Navigation.PushAsync(new UpdateOrder(order));
        }
    }

    private async void CreateOrder(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateOrder());
    }

    private async void DeletedOrders(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DeletedOrders());
    }

    private async void GetOrdersById(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoadOrderByID());
    }

    private async void DeliveredOrders(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DeliveredOrders());
    }
}

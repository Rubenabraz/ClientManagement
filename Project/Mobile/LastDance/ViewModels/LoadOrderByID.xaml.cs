using LastDance.DTOs;
using LastDance.Services;
using System.Collections.ObjectModel;

namespace LastDance.ViewModels;

public partial class LoadOrderByID : ContentPage
{
    private readonly ApiService _apiService;
    private ObservableCollection<OrderDto> _orders;
    public LoadOrderByID()
    {
        InitializeComponent();
        _apiService = new ApiService();
        _orders = new ObservableCollection<OrderDto>();

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
                    order.ClientName = $"{client.cltName} {client.cltSurname}";
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
            await DisplayAlert("Erro", $"Não foi possível carregar pedidos: {ex.Message}", "OK");
        }
    }

    private async void GetOrderById(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(orderIdEntry.Text))
        {
            await DisplayAlert("Aviso", "Insira um ID válido.", "OK");
            return;
        }

        if (int.TryParse(orderIdEntry.Text, out int orderId))
        {
            try
            {
                var order = await _apiService.GetOrdersById(orderId);

                if (order != null && order.ordStatus?.ToLower() != "removido")
                {
                    
                    var clients = await _apiService.GetClients();

                    var deletedClients = await _apiService.GetDeletedClients();

                    var allClients = clients.Concat(deletedClients).ToList();

                    //Associar um cliente a um pedido

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

                    //Atribuir ao BindingContext para UI

                    BindingContext = order;
                }
                else
                {
                    await DisplayAlert("Aviso", "Pedido não encontrado.", "OK");
                    BindingContext = new OrderDto();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Não foi possível carregar pedido: {ex.Message}", "OK");
                BindingContext = new OrderDto();
            }
        }
        else
        {
            await DisplayAlert("Aviso", "Insira um ID válido.", "OK");
            BindingContext = new OrderDto();
        }
    }
}
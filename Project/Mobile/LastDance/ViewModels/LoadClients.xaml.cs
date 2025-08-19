using LastDance.Services;
using LastDance.DTOs;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace LastDance.ViewModels;

public partial class LoadClients : ContentPage
{
    private readonly ApiService _apiService;

    private ObservableCollection<ClientDto> _clients;

    private List<ClientDto> _clientsList;

    public LoadClients()
    {
        InitializeComponent();
        _apiService = new ApiService();
        _clients = new ObservableCollection<ClientDto>();
        _clientsList = new List<ClientDto>();
        ClientsListView.ItemsSource = _clients;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            var apiClients = await _apiService.GetClients();

            _clientsList = apiClients.Where(c => c.cltStatus != "removido").ToList();

            PopulateClients();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível carregar pedidos: {ex.Message}", "OK");
        }
    }

    private void PopulateClients()
    {
        _clients.Clear();
        foreach (var c in _clientsList)
            _clients.Add(c);
    }

    private async void ClientInsert(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateClient());
    }
    private async void ClientUpdate(object sender, EventArgs e)
    {
        if (sender is Label label && label.BindingContext is ClientDto client)
        {
            await Navigation.PushAsync(new UpdateClient(client));
        }
    }

    private async void DeletedClients(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DeletedClients());
    }
    private async void OrdersList(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoadOrders());
    }
    private async void FindByID(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoadClientByID());
    }
}
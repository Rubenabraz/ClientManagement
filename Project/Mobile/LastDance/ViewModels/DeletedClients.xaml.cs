using LastDance.DTOs;
using LastDance.Services;
using System.Collections.ObjectModel;

namespace LastDance.ViewModels;

public partial class DeletedClients : ContentPage
{
    private readonly ApiService _apiService;
    private ObservableCollection<ClientDto> _deletedClients;

    public DeletedClients()
    {
        InitializeComponent();
        _apiService = new ApiService();
        _deletedClients = new ObservableCollection<ClientDto>();
        DeletedClientsListView.ItemsSource = _deletedClients;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            var apiClients = await _apiService.GetDeletedClients();

            _deletedClients.Clear();

            foreach (var client in apiClients.Where(c => c.cltStatus == "removido"))
                _deletedClients.Add(client);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível carregar clientes removidos: {ex.Message}", "OK");
        }
    }
}
using LastDance.DTOs;
using LastDance.Services;

namespace LastDance.ViewModels;

public partial class LoadClientByID : ContentPage
{
    private readonly ApiService _apiService;

    public LoadClientByID()
    {
        InitializeComponent();
        _apiService = new ApiService();
    }

    private async void GetClientById(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(clientIdEntry.Text))
        {
            await DisplayAlert("Aviso", "Insira um ID v�lido.", "OK");
            return;
        }

        if (int.TryParse(clientIdEntry.Text, out int clientId))
        {
            try
            {
                var client = await _apiService.GetClientById(clientId);

                if (client != null && client.cltStatus?.ToLower() != "removido")
                {
                    BindingContext = client;
                }
                else
                {
                    await DisplayAlert("Aviso", "Cliente n�o encontrado ou foi removido.", "OK");
                    BindingContext = new ClientDto();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"N�o foi poss�vel carregar cliente: {ex.Message}", "OK");
                BindingContext = new ClientDto();
            }
        }
        else
        {
            await DisplayAlert("Aviso", "Insira um ID v�lido.", "OK");
            BindingContext = new ClientDto();
        }
    }
}

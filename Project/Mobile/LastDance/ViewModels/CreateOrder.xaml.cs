using LastDance.Services;
using LastDance.DTOs;

namespace LastDance.ViewModels;

public partial class CreateOrder : ContentPage
{
    private readonly ApiService _apiService;
    private ClientDto? _selectedClient;

    public CreateOrder()
    {
        InitializeComponent();
        _apiService = new ApiService();
    }

    // Evento ao mudar ID do cliente
    private async void OnClientIdChanged(object sender, TextChangedEventArgs e)
    {
        _selectedClient = null; // reseta sempre que muda

        if (string.IsNullOrWhiteSpace(OrderClientIdEntry.Text))
        {
            OrderClientNameEntry.Text = string.Empty;
            return;
        }

        if (!int.TryParse(OrderClientIdEntry.Text, out var clientId) || clientId <= 0)
        {
            OrderClientNameEntry.Text = string.Empty;
            return;
        }

        try
        {
            var clients = await _apiService.GetClients();
            var deletedClients = await _apiService.GetDeletedClients();
            var allClients = clients.Concat(deletedClients).ToList();

            var client = allClients.FirstOrDefault(c => c.cltID == clientId);

            if (client == null)
            {
                OrderClientNameEntry.Text = "Cliente n�o encontrado";
            }
            else
            {
                _selectedClient = client; // guarda cliente v�lido

                if (client.cltStatus == "removido")
                {
                    OrderClientNameEntry.Text = $"{client.cltName} {client.cltSurname} (Removido)";
                }
                else
                {
                    OrderClientNameEntry.Text = $"{client.cltName} {client.cltSurname}".Trim();
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Falha ao carregar cliente: {ex.Message}", "OK");
            OrderClientNameEntry.Text = string.Empty;
        }
    }

    // Salvar Pedido
    private async void OnSaveOrderClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(OrderNameEntry.Text) || OrderNameEntry.Text.Length < 2)
        {
            await DisplayAlert("Erro", "O nome do pedido deve ter pelo menos 2 caracteres.", "OK");
            return;
        }

        if (!decimal.TryParse(OrderTotalAmountEntry.Text, out var totalAmount) || totalAmount <= 0)
        {
            await DisplayAlert("Erro", "O valor total do pedido deve ser um n�mero positivo.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(OrderDescriptionEntry.Text))
        {
            await DisplayAlert("Erro", "A descri��o do pedido n�o pode estar vazia.", "OK");
            return;
        }

        // valida cliente selecionado

        if (_selectedClient == null)
        {
            await DisplayAlert("Erro", "Cliente inv�lido. Escolha um cliente v�lido.", "OK");
            return;
        }

        // bloqueia cliente removido

        if (_selectedClient.cltStatus == "removido")
        {
            await DisplayAlert("Erro", "N�o � poss�vel criar pedidos para clientes removidos.", "OK");
            return;
        }

        var newOrder = new OrderDto
        {
            ordClientID = _selectedClient.cltID,
            ordName = OrderNameEntry.Text,
            ordTotalAmount = totalAmount,
            ordDescription = OrderDescriptionEntry.Text
        };

        try
        {
            var result = await _apiService.AddOrder(newOrder);
            await DisplayAlert("Sucesso", $"Pedido {result.ordName} criado com sucesso!", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"N�o foi poss�vel criar o pedido: {ex.Message}", "OK");
        }
    }
}

using LastDance.DTOs;
using LastDance.Services;
using System.Text.RegularExpressions;

namespace LastDance.ViewModels;

public partial class CreateClient : ContentPage
{
    private readonly ApiService _apiService;

    public CreateClient()
    {
        InitializeComponent();
        _apiService = new ApiService();
    }

    private async void OnSaveClientClicked(object sender, EventArgs e)
    {
        var allClients = (await _apiService.GetClients()).Concat(await _apiService.GetDeletedClients()).ToList();


        if (!ValidName(NameEntry.Text))
        {
            await DisplayAlert("Erro", "O nome deve ter pelo menos 2 caracteres e n�o pode conter caracteres inv�lidos.", "OK");
            return;
        }

        if (!ValidName(SurnameEntry.Text))
        {
            await DisplayAlert("Erro", "O apelido deve ter pelo menos 2 caracteres e n�o pode conter caracteres inv�lidos.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(NameEntry.Text) || NameEntry.Text.Length < 2)
        {
            await DisplayAlert("Erro", "O nome deve ter pelo menos 2 caracteres.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(SurnameEntry.Text) || SurnameEntry.Text.Length < 2)
        {
            await DisplayAlert("Erro", "O apelido deve ter pelo menos 2 caracteres.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(EmailEntry.Text) ||
            !Regex.IsMatch(EmailEntry.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            await DisplayAlert("Erro", "Introduza um email v�lido.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(PhoneEntry.Text) ||
            !Regex.IsMatch(PhoneEntry.Text, @"^9[0-9]{8}$"))
        {
            await DisplayAlert("Erro", "Introduza um n�mero de telefone v�lido (9 d�gitos e que comece por 9).", "OK");
            return;
        }

        if (GenderPicker.SelectedItem == null)
        {
            await DisplayAlert("Erro", "Selecione um g�nero.", "OK");
            return;
        }

        if (allClients.Any(c => c.cltEmail.Equals(EmailEntry.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
        {
            await DisplayAlert("Erro", "J� existe um cliente com este email.", "OK");
            return;
        }

        if (allClients.Any(c => c.cltPhoneNumber == PhoneEntry.Text.Trim()))
        {
            await DisplayAlert("Erro", "J� existe um cliente com este n�mero de telefone.", "OK");
            return;
        }

        var newClient = new ClientDto
        {
            cltName = NameEntry.Text.Trim(),
            cltSurname = SurnameEntry.Text.Trim(),
            cltEmail = EmailEntry.Text.Trim().ToLower(),
            cltPhoneNumber = PhoneEntry.Text.Trim(),
            cltGender = GenderPicker.SelectedItem.ToString()
        };

        try
        {
            var createdClient = await _apiService.AddClient(newClient);
            await DisplayAlert("Sucesso", $"Cliente {createdClient.cltName} registado com sucesso!", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"N�o foi poss�vel registar o cliente: {ex.Message}", "OK");
        }
    }

    private bool ValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
            return false;

        if (!Regex.IsMatch(name, @"^[A-Za-z�-�\s]+$"))
            return false;

        if (name.Distinct().Count() == 1)
            return false;

        return true;
    }
}
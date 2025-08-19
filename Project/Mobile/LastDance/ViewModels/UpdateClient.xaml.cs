using LastDance.DTOs;
using LastDance.Services;
using System.Text.RegularExpressions;

namespace LastDance.ViewModels;

public partial class UpdateClient : ContentPage
{
    private readonly ApiService _apiService;
    private readonly int _clientId;
    private readonly ClientDto _originalClient;

    public UpdateClient(ClientDto client)
    {
        InitializeComponent();
        _apiService = new ApiService();

        // Guardar ID para enviar no update

        _clientId = client.cltID;
        _originalClient = client;

        NameEntry.Text = client.cltName;
        SurnameEntry.Text = client.cltSurname;
        EmailEntry.Text = client.cltEmail;
        PhoneEntry.Text = client.cltPhoneNumber;
        GenderPicker.SelectedItem = client.cltGender;

        UpdateButton.IsVisible = false;

        NameEntry.TextChanged += OnFieldChanged;
        SurnameEntry.TextChanged += OnFieldChanged;
        EmailEntry.TextChanged += OnFieldChanged;
        PhoneEntry.TextChanged += OnFieldChanged;
        GenderPicker.SelectedIndexChanged += OnFieldChanged;

    }

    private void OnFieldChanged(object sender, EventArgs e)
    {
        UpdateButton.IsVisible =
            !NameEntry.Text.Equals(_originalClient.cltName, StringComparison.OrdinalIgnoreCase) ||
            !SurnameEntry.Text.Equals(_originalClient.cltSurname, StringComparison.OrdinalIgnoreCase) ||
            !EmailEntry.Text.Equals(_originalClient.cltEmail, StringComparison.OrdinalIgnoreCase) ||
            !PhoneEntry.Text.Equals(_originalClient.cltPhoneNumber, StringComparison.OrdinalIgnoreCase) ||
            (GenderPicker.SelectedItem?.ToString() != _originalClient.cltGender);
    }

    private async void OnUpdateClientClicked(object sender, EventArgs e)
    {
        var allClients = await _apiService.GetClients();
        var deletedClients = await _apiService.GetDeletedClients();

        var all = allClients.Concat(deletedClients).ToList();

        if (!NameEntry.Text.Equals(_originalClient.cltName, StringComparison.OrdinalIgnoreCase))
        {
            if (!ValidName(NameEntry.Text))
            {
                await DisplayAlert("Erro", "O nome deve ter pelo menos 2 caracteres e não pode conter caracteres inválidos.", "OK");
                return;
            }
        }

        // Apelido
        if (!SurnameEntry.Text.Equals(_originalClient.cltSurname, StringComparison.OrdinalIgnoreCase))
        {
            if (!ValidName(SurnameEntry.Text))
            {
                await DisplayAlert("Erro", "O apelido deve ter pelo menos 2 caracteres e não pode conter caracteres inválidos.", "OK");
                return;
            }
        }

        // Email
        if (!EmailEntry.Text.Equals(_originalClient.cltEmail, StringComparison.OrdinalIgnoreCase))
        {
            if (string.IsNullOrWhiteSpace(EmailEntry.Text) ||
                !Regex.IsMatch(EmailEntry.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                await DisplayAlert("Erro", "Introduza um email válido.", "OK");
                return;
            }

            if (allClients.Any(c => c.cltEmail.Equals(EmailEntry.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                await DisplayAlert("Erro", "Já existe um cliente com este email.", "OK");
                return;
            }
        }

        // Telefone
        if (!PhoneEntry.Text.Equals(_originalClient.cltPhoneNumber, StringComparison.OrdinalIgnoreCase))
        {
            if (string.IsNullOrWhiteSpace(PhoneEntry.Text) ||
                !Regex.IsMatch(PhoneEntry.Text, @"^9[0-9]{8}$"))
            {
                await DisplayAlert("Erro", "Introduza um número de telefone válido (9 dígitos e que comece por 9).", "OK");
                return;
            }

            if (allClients.Any(c => c.cltPhoneNumber == PhoneEntry.Text.Trim()))
            {
                await DisplayAlert("Erro", "Já existe um cliente com este número de telefone.", "OK");
                return;
            }
        }


        // Evitar email duplicado

        if (all.Any(c => c.cltEmail == EmailEntry.Text && c.cltID != _clientId))
        {
            await DisplayAlert("Erro", "Já existe um cliente com este email.", "OK");
            return;
        }

        // Evitar telefone duplicado

        if (all.Any(c => c.cltPhoneNumber == PhoneEntry.Text && c.cltID != _clientId))
        {
            await DisplayAlert("Erro", "Já existe um cliente com este número de telefone.", "OK");
            return;
        }

        // continua o update se passou as validações...

        var updatedClient = new ClientUpdateDto
        {
            cltName = NameEntry.Text,
            cltSurname = SurnameEntry.Text,
            cltEmail = EmailEntry.Text,
            cltPhoneNumber = PhoneEntry.Text,
            cltGender = GenderPicker.SelectedItem?.ToString()
        };

        try
        {
            var result = await _apiService.UpdateClient(_clientId, updatedClient);
            await DisplayAlert("Sucesso", $"Cliente {result.cltName} atualizado com sucesso!", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível atualizar o cliente: {ex.Message}", "OK");
        }
    }

    private async void OnDeleteClientClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await _apiService.DeleteClient(_clientId);
            if (result != null)
            {
                await DisplayAlert("Sucesso", $"Cliente {result.cltName} apagado com sucesso!", "OK");
            }
            else
            {
                await DisplayAlert("Aviso", "Cliente apagado com sucesso", "OK");
            }
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível apagar o cliente: {ex.Message}", "OK");
        }
    }

    private bool ValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
            return false;

        if (!Regex.IsMatch(name, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            return false;
        }

        if (name.Distinct().Count() == 1)
        {
            return false;
        }

        return true;
    }
}


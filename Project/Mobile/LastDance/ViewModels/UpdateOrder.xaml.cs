using LastDance.DTOs;
using LastDance.Services;

namespace LastDance.ViewModels
{
    public partial class UpdateOrder : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly int _orderId;
        private readonly int _orderClientID;

        private string _originalName;
        private decimal _originalTotal;
        private string _originalDescription;
        private bool _originalDelivered;
        private string _originalStatus = string.Empty;

        public UpdateOrder(OrderDto order)
        {
            InitializeComponent();

            _apiService = new ApiService();

            _orderId = order.ordId;
            _orderClientID = order.ordClientID;

            // Guardar valores originais para comparação
            _originalName = order.ordName;
            _originalTotal = order.ordTotalAmount;
            _originalStatus = order.ordStatus;
            _originalDescription = order.ordDescription;
            _originalDelivered = order.ordDelivered;

            // Preencher os campos
            OrderNameEntry.Text = order.ordName;
            OrderTotalAmountEntry.Text = order.ordTotalAmount.ToString("F2");
            OrderStatusEntry.Text = order.ordStatus;
            OrderDescriptionEntry.Text = order.ordDescription;

            if (order.ordStatus?.ToLower() == "entregue")
            {
                OrderDeliveredEntry.IsChecked = true;
                OrderDeliveredEntry.IsEnabled = false; // bloqueia checkbox
            }
            else
            {
                OrderDeliveredEntry.IsChecked = order.ordDelivered;
                OrderDeliveredEntry.IsEnabled = true; // permite alterar
            }

            UpdateButton.IsVisible = false;

            // Eventos para detectar alterações
            OrderNameEntry.TextChanged += OnFieldChanged;
            OrderTotalAmountEntry.TextChanged += OnFieldChanged;
            OrderStatusEntry.TextChanged += OnFieldChanged;
            OrderDescriptionEntry.TextChanged += OnFieldChanged;
            OrderDeliveredEntry.CheckedChanged += OnFieldChanged;
        }


        private void OnFieldChanged(object sender, EventArgs e)
        {
            UpdateButton.IsVisible =
                OrderNameEntry.Text != _originalName ||
                OrderDescriptionEntry.Text != _originalDescription ||
                OrderStatusEntry.Text != _originalStatus ||
                (decimal.TryParse(OrderTotalAmountEntry.Text, out var total) && total != _originalTotal) ||
                OrderDeliveredEntry.IsChecked != _originalDelivered;
        }

        private void OrderDeliveredEntry_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            bool delivered = e.Value;

            // Atualiza status conforme checkbox
            OrderStatusEntry.Text = ""; // limpa pra forçar refresh
            OrderStatusEntry.Text = delivered ? "entregue" : _originalStatus;
        }

        private async void onUpdateOrderClicked(object sender, EventArgs e)
        {
            // Garante que o status está correto
            string status = OrderDeliveredEntry.IsChecked ? "entregue" : _originalStatus;

            var updatedOrder = new OrderUpdateDto
            {
                ordClientID = _orderClientID,
                ordName = OrderNameEntry.Text,
                ordStatus = status,
                ordDescription = OrderDescriptionEntry.Text,
                ordDelivered = OrderDeliveredEntry.IsChecked,
                ordTotalAmount = decimal.TryParse(OrderTotalAmountEntry.Text, out var amount) ? amount : 0m
            };

            try
            {
                var result = await _apiService.UpdateOrder(_orderId, updatedOrder);

                await DisplayAlert("Sucesso", $"{result.ordName} atualizado com sucesso", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Não foi possível atualizar o pedido: {ex.Message}", "OK");
            }
        }
        private async void OnDeleteOrderClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await _apiService.DeleteOrder(_orderId);
                if (result != null)
                {
                    await DisplayAlert("Sucesso", $"Order {result.ordStatus} apagado com sucesso!", "OK");
                }
                else
                {
                    await DisplayAlert("Aviso", "Order apagado com sucesso", "OK");
                }
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Não foi possível apagar a order: {ex.Message}", "OK");
            }
        }
    }
}

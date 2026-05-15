using System.Text;
using System.Text.Json;

namespace ECommerceAdminPanel.Services;

// =============================================
// ✅ WhatsApp Service — Meta Cloud API
// =============================================

public interface IWhatsAppService
{
    Task<bool> SendInteractiveOrderMessageAsync(
        string token,
        string phoneNumberId,
        string customerPhone,
        string customerName,
        int orderId,
        string productDetails,
        decimal orderAmount
    );

    Task<bool> SendTextMessageAsync(
        string token,
        string phoneNumberId,
        string customerPhone,
        string message
    );
}

public class WhatsAppService : IWhatsAppService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WhatsAppService> _logger;
    private const string META_API_VERSION = "v19.0";

    public WhatsAppService(HttpClient httpClient, ILogger<WhatsAppService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    // =============================================
    // ✅ Interactive Order Message with Buttons
    // =============================================
    public async Task<bool> SendInteractiveOrderMessageAsync(
        string token,
        string phoneNumberId,
        string customerPhone,
        string customerName,
        int orderId,
        string productDetails,
        decimal orderAmount)
    {
        try
        {
            var url = $"https://graph.facebook.com/{META_API_VERSION}/{phoneNumberId}/messages";

            // Format phone — remove + if present, ensure country code
            var formattedPhone = customerPhone.Replace("+", "").Replace(" ", "").Replace("-", "");

            var payload = new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = formattedPhone,
                type = "interactive",
                interactive = new
                {
                    type = "button",
                    body = new
                    {
                        text = $"*Rilancio Order Received!* 🎉\n\n" +
                               $"Hi {customerName} 👋\n" +
                               $"We have received your order with following details:\n\n" +
                               $"🆔 *Order ID:* R{orderId}\n" +
                               $"📦 *Product Details:* {productDetails}\n" +
                               $"💰 *Order Amount PKR:* {orderAmount:N0}\n\n" +
                               $"Please select an option below to proceed the order."
                    },
                    action = new
                    {
                        buttons = new[]
                        {
                            new
                            {
                                type = "reply",
                                reply = new
                                {
                                    id = $"CONFIRM_ORDER_{orderId}",
                                    title = "✅ Confirm Order"
                                }
                            },
                            new
                            {
                                type = "reply",
                                reply = new
                                {
                                    id = $"CANCEL_ORDER_{orderId}",
                                    title = "❌ Cancel Order"
                                }
                            }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await _httpClient.PostAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("✅ WhatsApp sent to {Phone} for Order {OrderId}", formattedPhone, orderId);
                return true;
            }
            else
            {
                _logger.LogError("❌ WhatsApp failed. Status: {Status} | Body: {Body}", response.StatusCode, responseBody);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ WhatsApp exception for Order {OrderId}", orderId);
            return false;
        }
    }

    // =============================================
    // ✅ Simple Text Message (for status updates)
    // =============================================
    public async Task<bool> SendTextMessageAsync(
        string token,
        string phoneNumberId,
        string customerPhone,
        string message)
    {
        try
        {
            var url = $"https://graph.facebook.com/{META_API_VERSION}/{phoneNumberId}/messages";
            var formattedPhone = customerPhone.Replace("+", "").Replace(" ", "").Replace("-", "");

            var payload = new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = formattedPhone,
                type = "text",
                text = new { body = message }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await _httpClient.PostAsync(url, content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ WhatsApp text message exception");
            return false;
        }
    }
}
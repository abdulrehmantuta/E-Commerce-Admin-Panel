using System.Text;
using System.Text.Json;

namespace ECommerceAdminPanel.Services;

public interface IWhatsAppService
{
    Task<bool> SendInteractiveOrderMessageAsync(
        string token,
        string phoneNumberId,
        string customerPhone,
        string customerName,
        int orderId,
        string productDetails,
        decimal orderAmount,
        string provider = "Meta"
    );

    Task<bool> SendTextMessageAsync(
        string token,
        string phoneNumberId,
        string customerPhone,
        string message,
        string provider = "Meta"
    );
}

public class WhatsAppService : IWhatsAppService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WhatsAppService> _logger;
    private const string META_API_VERSION = "v19.0";
    private const string TWILIO_SANDBOX_NUMBER = "whatsapp:+14155238886";

    public WhatsAppService(HttpClient httpClient, ILogger<WhatsAppService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<bool> SendInteractiveOrderMessageAsync(
        string token,
        string phoneNumberId,
        string customerPhone,
        string customerName,
        int orderId,
        string productDetails,
        decimal orderAmount,
        string provider = "Meta")
    {
        if (provider == "Twilio")
        {
            // Twilio interactive buttons support nahi karta sandbox mein
            // Simple text message bhejte hain
            var msg = $"🎉 *Order Received!*\n\n" +
                      $"Hi {customerName} 👋\n" +
                      $"Order ID: #{orderId}\n" +
                      $"Product: {productDetails}\n" +
                      $"Amount: Rs. {orderAmount:N0}\n\n" +
                      $"Reply *CONFIRM* to confirm or *CANCEL* to cancel.";

            return await SendTwilioTextAsync(token, phoneNumberId, customerPhone, msg);
        }

        return await SendMetaInteractiveAsync(token, phoneNumberId, customerPhone,
            customerName, orderId, productDetails, orderAmount);
    }

    public async Task<bool> SendTextMessageAsync(
        string token,
        string phoneNumberId,
        string customerPhone,
        string message,
        string provider = "Meta")
    {
        if (provider == "Twilio")
            return await SendTwilioTextAsync(token, phoneNumberId, customerPhone, message);

        return await SendMetaTextAsync(token, phoneNumberId, customerPhone, message);
    }

    // =============================================
    // ✅ META — Interactive Message with Buttons
    // =============================================
    private async Task<bool> SendMetaInteractiveAsync(
        string token, string phoneNumberId, string customerPhone,
        string customerName, int orderId, string productDetails, decimal orderAmount)
    {
        try
        {
            var url = $"https://graph.facebook.com/{META_API_VERSION}/{phoneNumberId}/messages";
            var formattedPhone = FormatPhone(customerPhone);

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
                        text = $"*Order Received!* 🎉\n\n" +
                               $"Hi {customerName} 👋\n" +
                               $"🆔 *Order ID:* #{orderId}\n" +
                               $"📦 *Product:* {productDetails}\n" +
                               $"💰 *Amount:* Rs. {orderAmount:N0}\n\n" +
                               $"Please select an option below."
                    },
                    action = new
                    {
                        buttons = new[]
                        {
                            new { type = "reply", reply = new { id = $"CONFIRM_ORDER_{orderId}", title = "✅ Confirm Order" } },
                            new { type = "reply", reply = new { id = $"CANCEL_ORDER_{orderId}",  title = "❌ Cancel Order"  } }
                        }
                    }
                }
            };

            return await PostMetaAsync(url, token, payload);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Meta interactive message error");
            return false;
        }
    }

    // =============================================
    // ✅ META — Simple Text Message
    // =============================================
    private async Task<bool> SendMetaTextAsync(
        string token, string phoneNumberId, string customerPhone, string message)
    {
        try
        {
            var url = $"https://graph.facebook.com/{META_API_VERSION}/{phoneNumberId}/messages";
            var formattedPhone = FormatPhone(customerPhone);

            var payload = new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = formattedPhone,
                type = "text",
                text = new { body = message }
            };

            return await PostMetaAsync(url, token, payload);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Meta text message error");
            return false;
        }
    }

    // =============================================
    // ✅ TWILIO — Text Message
    // =============================================
    private async Task<bool> SendTwilioTextAsync(
        string accountSid, string authToken, string customerPhone, string message)
    {
        try
        {
            // Twilio: accountSid = token field, authToken = phoneNumberId field
            var formattedPhone = $"whatsapp:+{FormatPhone(customerPhone)}";
            var url = $"https://api.twilio.com/2010-04-01/Accounts/{accountSid}/Messages.json";

            var formData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("From", TWILIO_SANDBOX_NUMBER),
                new KeyValuePair<string, string>("To",   formattedPhone),
                new KeyValuePair<string, string>("Body", message)
            });

            _httpClient.DefaultRequestHeaders.Clear();

            // Twilio Basic Auth
            var credentials = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{accountSid}:{authToken}"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {credentials}");

            var response = await _httpClient.PostAsync(url, formData);
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("✅ Twilio WhatsApp sent to {Phone}", formattedPhone);
                return true;
            }

            _logger.LogError("❌ Twilio failed: {Body}", body);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Twilio exception");
            return false;
        }
    }

    // =============================================
    // ✅ HELPERS
    // =============================================
    private async Task<bool> PostMetaAsync(string url, string token, object payload)
    {
        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var response = await _httpClient.PostAsync(url, content);
        var body = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("✅ Meta WhatsApp sent successfully");
            return true;
        }

        _logger.LogError("❌ Meta failed: {Body}", body);
        return false;
    }

    private static string FormatPhone(string phone) =>
        phone.Replace("+", "").Replace(" ", "").Replace("-", "");
}
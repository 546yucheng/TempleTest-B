using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

public class LinePayClient
{
    private readonly HttpClient _httpClient;
    private readonly string _channelId;
    private readonly string _channelSecret;

    public LinePayClient(HttpClient httpClient, string channelId, string channelSecret)
    {
        _httpClient = httpClient;
        _channelId = channelId;
        _channelSecret = channelSecret;
    }

    public async Task<string> SendPaymentRequestAsync(object requestPayload)
    {
        string endpoint = "https://sandbox-api-pay.line.me/v2/payments/request";
        string nonce = Guid.NewGuid().ToString();
        string signature = GenerateSignature(nonce, requestPayload);

        var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = new StringContent(JsonConvert.SerializeObject(requestPayload), Encoding.UTF8, "application/json")
        };

        request.Headers.Add("X-LINE-ChannelId", _channelId);
        request.Headers.Add("X-LINE-Authorization-Nonce", nonce);
        request.Headers.Add("X-LINE-Authorization", signature);

        var response = await _httpClient.SendAsync(request);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> ConfirmPaymentAsync(string transactionId, object requestPayload)
    {
        string endpoint = $"https://sandbox-api-pay.line.me/v2/payments/{transactionId}/confirm";
        string nonce = Guid.NewGuid().ToString();
        string signature = GenerateSignature(nonce, requestPayload);

        var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = new StringContent(JsonConvert.SerializeObject(requestPayload), Encoding.UTF8, "application/json")
        };

        request.Headers.Add("X-LINE-ChannelId", _channelId);
        request.Headers.Add("X-LINE-Authorization-Nonce", nonce);
        request.Headers.Add("X-LINE-Authorization", signature);

        var response = await _httpClient.SendAsync(request);
        return await response.Content.ReadAsStringAsync();
    }
    private string GenerateSignature(string nonce, object requestPayload)
    {
        string rawSignature = $"{_channelSecret}{nonce}{JsonConvert.SerializeObject(requestPayload)}";
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_channelSecret)))
        {
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawSignature));
            return Convert.ToBase64String(hash);
        }
    }


}

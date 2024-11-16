using System.Text;
using Aprojectbackend.Providers;
using static Aprojectbackend.DTO.linepayDTO.linepayDTO;
using Aprojectbackend.Service.Linepayinterface;


namespace Aprojectbackend.Service.LinepayService
{
    public class LinepayService: ILinepayService
    {
        public LinepayService()
        {
            client = new HttpClient();
            _jsonProvider = new JsonProvider();
        }

        private readonly string channelId = "2006554568";
        private readonly string channelSecretKey = "11bde2469f8fd72c9e74002a09012ef7";


        private readonly string linePayBaseApiUrl = "https://sandbox-api-pay.line.me";

        private static HttpClient client;
        private readonly JsonProvider _jsonProvider;

        // 送出建立交易請求至 Line Pay Server
        public async Task<PaymentResponseDto> SendPaymentRequest(PaymentRequestDto dto)
        {
            var json = _jsonProvider.Serialize(dto);
            // 產生 GUID Nonce
            var nonce = Guid.NewGuid().ToString();
            // 要放入 signature 中的 requestUrl
            var requestUrl = "/v3/payments/request";

            //使用 channelSecretKey & requestUrl & jsonBody & nonce 做簽章
            var signature = Providers.SignatureProvider.HMACSHA256(channelSecretKey, channelSecretKey + requestUrl + json + nonce);

            var request = new HttpRequestMessage(HttpMethod.Post, linePayBaseApiUrl + requestUrl)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            // 帶入 Headers
            client.DefaultRequestHeaders.Add("X-LINE-ChannelId", channelId);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization", signature);

            var response = await client.SendAsync(request);
            var linePayResponse = _jsonProvider.Deserialize<PaymentResponseDto>(await response.Content.ReadAsStringAsync());

            Console.WriteLine(nonce);
            Console.WriteLine(signature);

            return linePayResponse;
        }

        // 取得 transactionId 後進行確認交易
        public async Task<PaymentConfirmResponseDto> ConfirmPayment(string transactionId, string orderId, PaymentConfirmDto dto) //加上 OrderId 去找資料
        {
            try
            {
                var json = _jsonProvider.Serialize(dto);

            var nonce = Guid.NewGuid().ToString();
            var requestUrl = string.Format("/v3/payments/{0}/confirm", transactionId);
            var signature = Providers.SignatureProvider.HMACSHA256(channelSecretKey, channelSecretKey + requestUrl + json + nonce);

            var request = new HttpRequestMessage(HttpMethod.Post, String.Format(linePayBaseApiUrl + requestUrl, transactionId))
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            client.DefaultRequestHeaders.Add("X-LINE-ChannelId", channelId);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization", signature);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                // 可以加入記錄或返回更有意義的錯誤給前端
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"LinePay API request failed. Status code: {response.StatusCode}, Content: {errorContent}");
            }

            var responseDto = _jsonProvider.Deserialize<PaymentConfirmResponseDto>(await response.Content.ReadAsStringAsync());
            return responseDto;
            }
            catch (Exception ex)
            {
                // 記錄異常
                throw new ApplicationException("An error occurred while confirming payment. Please try again later.");
            }
        }

        public async Task<PaymentConfirmResponseDto> CheckRegKey(string regKey)
        {
            var nonce = Guid.NewGuid().ToString();
            var requestUrl = string.Format("/v3/payments/preapprovedPay/{0}/check", regKey);
            var request = new HttpRequestMessage(HttpMethod.Get, linePayBaseApiUrl + requestUrl);
            var result = Providers.SignatureProvider.HMACSHA256(channelSecretKey, channelSecretKey + requestUrl + nonce);
            client.DefaultRequestHeaders.Add("X-LINE-ChannelId", channelId);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization", result);
            var response = await client.SendAsync(request);

            return _jsonProvider.Deserialize<PaymentConfirmResponseDto>(await response.Content.ReadAsStringAsync());
        }

        public async Task<PaymentConfirmResponseDto> PayPreapproved(string regKey, PayPreapprovedDto dto)
        {
            var json = _jsonProvider.Serialize(dto);
            var nonce = Guid.NewGuid().ToString();
            var requestUrl = string.Format("/v3/payments/preapprovedPay/{0}/payment", regKey);
            var request = new HttpRequestMessage(HttpMethod.Post, linePayBaseApiUrl + requestUrl)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            var result = Providers.SignatureProvider.HMACSHA256(channelSecretKey, channelSecretKey + requestUrl + json + nonce);
            client.DefaultRequestHeaders.Add("X-LINE-ChannelId", channelId);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization", result);
            var response = await client.SendAsync(request);

            return _jsonProvider.Deserialize<PaymentConfirmResponseDto>(await response.Content.ReadAsStringAsync());
        }

        public async Task<PaymentConfirmResponseDto> ExpireRegKey(string regKey)
        {
            var nonce = Guid.NewGuid().ToString();
            var requestUrl = string.Format("/v3/payments/preapprovedPay/{0}/expire", regKey);
            var request = new HttpRequestMessage(HttpMethod.Post, linePayBaseApiUrl + requestUrl);
            var result = Providers.SignatureProvider.HMACSHA256(channelSecretKey, channelSecretKey + requestUrl + nonce);
            client.DefaultRequestHeaders.Add("X-LINE-ChannelId", channelId);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization", result);
            var response = await client.SendAsync(request);

            return _jsonProvider.Deserialize<PaymentConfirmResponseDto>(await response.Content.ReadAsStringAsync());
        }

        public async void TransactionCancel(string transactionId)
        {
            //使用者取消交易則會到這裏。
            Console.WriteLine($"訂單 {transactionId} 已取消");
        }
    }
}

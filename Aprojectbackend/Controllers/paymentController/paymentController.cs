using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Aprojectbackend.Service.Conmon;
using Aprojectbackend.DTO;
using Aprojectbackend.DTO.PaymentDTO;

namespace Aprojectbackend.Controllers.paymentController
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly LinePayClient _linePayClient;

        public PaymentController(LinePayClient linePayClient)
        {
            _linePayClient = linePayClient;
        }

        // 初始化付款
        [HttpPost("initialize")]
        public async Task<IActionResult> InitializePayment([FromBody] PaymentRequestDTO model)
        {
            var requestPayload = new
            {
                amount = model.Amount,
                currency = "TWD",
                orderId = Guid.NewGuid().ToString(),
                packages = new[]
                {
                    new
                    {
                        id = "pkg-001",
                        amount = model.Amount,
                        name = model.PackageName,
                        products = new[]
                        {
                            new
                            {
                                name = model.ProductName,
                                quantity = model.Quantity,
                                price = model.Amount
                            }
                        }
                    }
                },
                redirectUrls = new
                {
                    confirmUrl = "https://your-domain.com/api/payment/confirm",
                    cancelUrl = "https://your-domain.com/api/payment/cancel"
                }
            };

            try
            {
                string response = await _linePayClient.SendPaymentRequestAsync(requestPayload);
                return Content(response, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // 確認付款
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmPayment([FromBody] string transactionId)
        {
            var requestPayload = new
            {
                amount = 100, // 確認時的金額應與初始化金額一致
                currency = "TWD"
            };

            try
            {
                string response = await _linePayClient.ConfirmPaymentAsync(transactionId, requestPayload);
                return Content(response, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}

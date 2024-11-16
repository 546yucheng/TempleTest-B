using Aprojectbackend.Models;
using Aprojectbackend.Service.LinepayService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Aprojectbackend.DTO.linepayDTO.linepayDTO;

namespace Aprojectbackend.Controllers.linepayController
{
    [ApiController]
    [EnableCors("All")]
    [Route("api/[Controller]")]
    public class LinepayController : ControllerBase
    {
        private readonly LinepayService _linePayService;
        private readonly AprojectContext _context;
        public LinepayController(AprojectContext context, LinepayService linePayService)
        {
            _linePayService =  linePayService;
            _context = context;
        }

        /// <summary>
        /// 新增付款請求(order結帳鈕)
        /// </summary>
        /// /// <returns></returns>
        [HttpPost("Create")]
        public async Task<PaymentResponseDto> CreatePayment(PaymentRequestDto dto)
        {
            return await _linePayService.SendPaymentRequest(dto);
        }

        /// <summary>
        /// 確認付款請求(confirmURL按鈕)
        /// </summary>
        /// /// <returns></returns>
        [HttpPost("Confirm")]
        public async Task<PaymentConfirmResponseDto> ConfirmPayment([FromQuery] string transactionId, [FromQuery] string orderId, PaymentConfirmDto dto)
        {
            return await _linePayService.ConfirmPayment(transactionId, orderId, dto);
        }

        [HttpGet("CheckRegKey/{regKey}")]
        public async Task<PaymentConfirmResponseDto> CheckRegKey(string regKey)
        {
            return await _linePayService.CheckRegKey(regKey);
        }

        [HttpPost("PayPreapproved/{regKey}")]
        public async Task<PaymentConfirmResponseDto> PayPreapproved([FromRoute] string regKey, PayPreapprovedDto dto)
        {
            return await _linePayService.PayPreapproved(regKey, dto);
        }

        [HttpPost("ExpireRegKey/{regKey}")]
        public async Task<PaymentConfirmResponseDto> ExpireRegKey(string regKey)
        {
            return await _linePayService.ExpireRegKey(regKey);
        }

        [HttpGet("Cancel")]
        public async void CancelTransaction([FromQuery] string transactionId)
        {
            _linePayService.TransactionCancel(transactionId);
        }
    }
}

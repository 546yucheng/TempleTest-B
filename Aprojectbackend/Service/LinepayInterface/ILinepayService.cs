using static Aprojectbackend.DTO.linepayDTO.linepayDTO;

namespace Aprojectbackend.Service.Linepayinterface
{
    public interface ILinepayService
    {
        Task<PaymentResponseDto> SendPaymentRequest(PaymentRequestDto dto);
        Task<PaymentConfirmResponseDto> ConfirmPayment(string transactionId, string orderId, PaymentConfirmDto dto);

    }
}

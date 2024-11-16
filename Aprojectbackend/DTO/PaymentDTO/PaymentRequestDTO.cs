namespace Aprojectbackend.DTO.PaymentDTO
{
    public class PaymentRequestDTO
    {
        public int Amount { get; set; } // 付款金額
        public string PackageName { get; set; } // 套件名稱
        public string ProductName { get; set; } // 商品名稱
        public int Quantity { get; set; } // 商品數量
    }
}

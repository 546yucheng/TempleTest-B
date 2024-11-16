using System.ComponentModel.DataAnnotations;

namespace Aprojectbackend.DTO.orderDTO
{
    //Orderlist 訂單列表頁
    public class OrderListDto
    {
        public int OrderId { get; set; }  // 訂單編號
        public string OrderDate { get; set; }  // 訂單日期
        public decimal TotalPrice { get; set; }  // 總金額
        public string PaymentStatus { get; set; }  // 付款狀態
        public string ShippingStatus { get; set; }  // 配送狀態
        public string OrderStatus { get; set; }  // 訂單狀態
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
    
    //Orderdetail 訂單明細頁
    public class OrderDetailDto
    {
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public string OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OCartItemDto> Products { get; set; }
        public string RecepientName { get; set; }
        public string RecepientAddress { get; set; }
        public string RecepientPhone { get; set; }
        public string RecepientEmail { get; set; }
        public string ShippingInfo { get; set; }
        public string PaymentInfo { get; set; }
        public string OrderRemarks { get; set; }
    }

    //訂單明細與下訂單頁通用的購物車項目
    public class OCartItemDto
    {
        public int ProductId { get; set; }  // 商品ID
        public string ProdName { get; set; }  // 商品名稱
        public decimal ProdSellingPrice { get; set; }  // 商品價格
        public int Quantity { get; set; }  // 購買數量
        // 讓 Price 成為計算屬性
        public decimal Price => ProdSellingPrice * Quantity;
    }

    //下訂單頁的購物車資料要傳回資料庫欄位
    public class CartItemDataDto
    {
        public int ProductId { get; set; }  // 商品ID
        public string ProductName { get; set; } // 商品名yu
        public int Quantity { get; set; }  // 購買數量
        public decimal Price { get; set; } //小計
    }


    //Order 下訂單頁
    public class OrderPageDto
    {
        public List<OCartItemDto> CartItems { get; set; }  // 購物車商品清單
        public decimal TotalPrice { get; set; }  // 改為可寫屬性的總金額
    }

    public class MemberDataDto //代入會員資料
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }

    public class SubmitOrderDto
    {
        public int UserId { get; set; } //測試用

        [Required(ErrorMessage = "收件人姓名是必填欄位")]
        public string RecepientName { get; set; }

        [Required(ErrorMessage = "收件人地址是必填欄位")]
        public string RecepientAddress { get; set; }

        [Required(ErrorMessage = "收件人電話是必填欄位")]
        //[Phone(ErrorMessage = "電話號碼格式不正確")]
        public string RecepientPhone { get; set; }
        
        [Required(ErrorMessage = "收件人信箱是必填欄位")]
        //[EmailAddress(ErrorMessage = "信箱格式不正確")]
        public string RecepientEmail { get; set; }

        //[Required(ErrorMessage = "配送方式是必填欄位")]
        public string ShippingInfo { get; set; }

        //[Required(ErrorMessage = "付款方式是必填欄位")]
        public string PaymentInfo { get; set; }

        public string OrderRemarks { get; set; }

        //[MinLength(1, ErrorMessage = "購物車中必須至少包含一項商品")]
        public List<CartItemDataDto> CartItems { get; set; }
    }

    //OrderComplete 訂單完成頁
    public class OrderCompleteDto
    {
        public int OrderId { get; set; }

        public string Web { get; set; }

    }
}

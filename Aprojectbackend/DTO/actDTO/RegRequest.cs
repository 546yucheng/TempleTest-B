namespace Aprojectbackend.DTO.actDTO
{
    // 訂單請求模型
    public class RegRequest
    {
        public int FActDetailId { get; set; }
        public int FUserId { get; set; }
        //public string FUserName { get; set; }
        public decimal? FRegFee { get; set; } //報名費用
        public int TotalAmount { get; set; } //交易總金額

        public int? FActDetailIdIndex { get; set; } //報名批次index

        public List<RegDetail> regDetail { get; set; }
    }


    public class RegDetail
    {

        public string fRegName { get; set; }
        public string? fRegTel { get; set; }
        public string? fRegEmail { get; set; }
        public string? fRecipientAddress { get; set; }
    }

    public class ActToReg 
    {
        public IList<actDetail> actDetail { get; set; }
        public List<RegDetail> RegDetail { get; set; }
    }

}




public class PaymentNotification
{
    public string MerchantID { get; set; }  // 商店ID
    public string MerchantTradeNo { get; set; }  // 廠商的交易編號
    public int RtnCode { get; set; }  // 交易狀態
    public int RtnMsg { get; set; }  // 交易訊息
    public int TradeAmt { get; set; }  // 交易金額
    public string PaymentDate { get; set; }  // 付款時間
    public string CheckMacValue { get; set; }  // 檢查碼

}


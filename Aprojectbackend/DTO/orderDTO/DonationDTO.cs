namespace Aprojectbackend.DTO.orderDTO
{
    public class DonationDTO
    {
        public int DonationId { get; set; } // 去除前綴
        public string DonationDate { get; set; } // 簡化命名
        public decimal Amount { get; set; } // 用 Amount 表達捐款金額
    }


    // 用來儲存捐款資料分頁結果
    public class donationPagesList
    {
        // 總頁數
        public int? pages { get; set; }

        // 當前頁的捐款資料列表
        public List<DonationDTO> donationList { get; set; }
    }
}

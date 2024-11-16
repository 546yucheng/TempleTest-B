using Aprojectbackend.Models;
using System.ComponentModel;

namespace Aprojectbackend.DTO.actDTO
{
    public class actDetail
    {
        public string? FUserName { get; set; }
        public string? FUserEmail { get; set; }
        public int? FActId { get; set; }

        public string FActCategory { get; set; }

        //[DisplayName("活動名稱")]
        public string FActName { get; set; }

        //[DisplayName("編號代碼")]
        public string FActDisplayId { get; set; }

        //[DisplayName("活動地點")]
        public string? FActLocation { get; set; }

        //[DisplayName("活動說明")]
        public string? FActDescription { get; set; }

        //[DisplayName("報名費用")]
        public decimal? FRegFee { get; set; }

        //[DisplayName("單場人數限制")]
        public int? FMaxNumber { get; set; }

        public List<actDetailDate>? actDetailDate { get; set; }

        public string FActImgName { get; set; }

        //public byte[] FActImg { get; set; }

        //public string? FActImgPath { get; set; }

        //[DisplayName("圖片說明")]
        //public string? FImgDescription { get; set; }

        //[DisplayName("活動圖片")]
        //public IFormFile? photo { get; set; }
    }

    public class actDetailDate
    {
        public int FActDetailId { get; set; }
        public string FDisplayId { get; set; }
        public string dates { get; set; }
        public int? FStatusId { get; set; }
    }
}

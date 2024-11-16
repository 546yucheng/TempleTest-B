using Aprojectbackend.Models;

namespace Aprojectbackend.DTO.matchDTO
{
    public class UserProfileDisplayDTO
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public UserProfileDisplayDTO()
        {
            //Age = 20;
        }

        /// <summary>
        /// 建構子
        /// </summary>
        public UserProfileDisplayDTO(TUserPrefer userPrefer)
        {
            UserNickName = userPrefer.FUser?.FUserNickName;
            Age = userPrefer.FAge;
            Gender = userPrefer.FGender ? "男" : "女";
        }


        public int UserId { get; set; }
        public string UserNickName { get; set; }
        public int? Age { get; set; }
        public string Gender { get; set; }
        public decimal? Height { get; set; }
        public List<string> Hobbies { get; set; } = new List<string>();
        public List<string> Traits { get; set; } = new List<string>();
        public string Bio { get; set; } // 新增 Bio 屬性
        public string FPhotoPath { get; set; }
        public string Base64ImageData { get; set; }
    }
}

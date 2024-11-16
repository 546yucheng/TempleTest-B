namespace Aprojectbackend.DTO.UserDTO
{
    public class UserPersonelinfoEdit
    {
        public string FUserName { get; set; }
        public string FUserPhone { get; set; }
        public bool? FUserGender { get; set; }
        public string FUserAddress { get; set; }
        public string FUserNickName { get; set; }
        public string FUserBirthdate { get; set; }
        public IFormFile? Photo { get; set; }
    }
}

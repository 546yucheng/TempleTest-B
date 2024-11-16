namespace Aprojectbackend.DTO.UserDTO
{
    public class UserPersonelinfo
    {
        public string FUserEmail { get; set; }
        public string FUserName { get; set; }
        public string FUserPhone { get; set; }
        public bool? FUserGender { get; set; }
        public string FUserAddress { get; set; }
        public string FUserNickName { get; set; }
        public string FUserState { get; set; }
        public int FUserStateId { get; set; }
        public DateOnly? FUserBirthdate { get;  set; }
        public string? FUserImage { get; set; }
    }
}

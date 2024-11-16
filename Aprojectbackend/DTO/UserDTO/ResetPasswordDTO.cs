namespace Aprojectbackend.DTO.UserDTO
{
    // 重設密碼的 DTO
    public class ResetPasswordDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}

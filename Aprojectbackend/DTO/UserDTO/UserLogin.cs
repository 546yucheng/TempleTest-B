namespace Aprojectbackend.DTO.UserDTO
{
    public class UserLogin
    {
        public string FUserEmail { get; set; }
        public string FUserPassword { get; set; }
    }

    public class LoginResponse
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}

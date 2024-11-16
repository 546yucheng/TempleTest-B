namespace Aprojectbackend.DTO.matchDTO
{
    public class InterestRequestDto
    {
        public int ToUserId { get; set; } // 收件人ID
        public int FromUserId { get; set; } // 寄件人ID（如果需要傳送寄件人的信息）
    }
}

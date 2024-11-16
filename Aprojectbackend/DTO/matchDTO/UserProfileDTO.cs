namespace Aprojectbackend.DTO.matchDTO
{
    public class UserProfileDTO
    {
        public int UserId { get; set; }
        public string UserNickName { get; set; } 
        public decimal? Height { get; set; }
        public string? PhotoPath { get; set; }
        public string? Info { get; set; }
        public List<string>? SelectedHobbies { get; set; }
        public List<string>? SelectedTraits { get; set; }
    }
    public class UploadPhoto()
    {
        public IFormFile PhotoFile { get; set; }
    }
}

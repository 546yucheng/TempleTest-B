namespace Aprojectbackend.Service.Conmon
{
    public class matchphoto
    {
        private string GetMimeType(string extension)
        {
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".bmp" => "image/bmp",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => null,
            };
        }

        public string Base64ImageData(string photoPath)
        {
            string result = "";

            string defaultImagePath = "uploads/預設.jpg";
            string uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string filePath = string.IsNullOrEmpty(photoPath) ? Path.Combine(uploadsFolderPath, defaultImagePath) : Path.Combine(uploadsFolderPath, photoPath);

            if (File.Exists(filePath))
            {
                byte[] imageBytes = File.ReadAllBytes(filePath);
                string fileExtension = Path.GetExtension(filePath).ToLower();
                string mimeType = GetMimeType(fileExtension);

                if (!string.IsNullOrEmpty(mimeType))
                {
                    result = $"data:{mimeType};base64,{Convert.ToBase64String(imageBytes)}";
                }
            }
            return result;
        }
    }
}

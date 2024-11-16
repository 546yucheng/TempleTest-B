namespace Aprojectbackend.Service.Conmon
{
    public static class ConvertImage
    {
        //轉Base64方法
        //"Controllers/productController/images/productRelate/XX.jpg"
        public static string ConvertImageBase64(string imagePath)
        {
            string base64Image = "";

            // 取得完整路徑
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), imagePath);

            // 確認是否有檔案
            if (System.IO.File.Exists(fullPath))
            {
                // 讀取檔案至byte陣列
                byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                // 將byte陣列轉換為Base64字串

                // <img src="data:image/jpeg;base64,你的BASE64字串" alt="圖片描述">
                //base64Image = Convert.ToBase64String(imageBytes);
                base64Image = $"data:image/jpeg;base64,{Convert.ToBase64String(imageBytes)}";
            }
            else
            {
                string defaultPath = "Controllers/productController/images/productRelate/default.jpg";
                defaultPath = Path.Combine(Directory.GetCurrentDirectory(), defaultPath);

                byte[] imageBytes = System.IO.File.ReadAllBytes(defaultPath);
                base64Image = $"data:image/jpeg;base64,{Convert.ToBase64String(imageBytes)}";
            }
            return base64Image;
        }
    }
}

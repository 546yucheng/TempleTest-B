public class PasswordService : IPasswordService
{
    // 實現 HashPassword 方法，對密碼進行加鹽和雜湊
    public (string HashedPassword, string Salt) HashPassword(string password)
    {
        var salt = GenerateSalt(); // 生成一個新的鹽值
        var hashedPassword = CreateHashedPassword(password, salt); // 使用鹽值和密碼生成雜湊密碼
        return (hashedPassword, salt); // 返回雜湊後的密碼和鹽值
    }

    // 實現 VerifyPassword 方法，驗證密碼是否正確
    public bool VerifyPassword(string password, string salt, string hashedPassword)
    {
        var computedHash = CreateHashedPassword(password, salt); // 使用輸入的密碼和鹽值生成雜湊
        return computedHash == hashedPassword; // 比較生成的雜湊與存儲的雜湊，返回是否匹配
    }

    private string GenerateSalt()
    {
        byte[] saltBytes = new byte[16]; // 建立一個 16 字節的鹽值
        using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
        {
            rng.GetBytes(saltBytes); // 使用加密安全的隨機數生成器來填充鹽值
        }
        return Convert.ToBase64String(saltBytes); // 將鹽值轉換為 Base64 字符串返回
    }

    private string CreateHashedPassword(string password, string salt)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var saltedPassword = password + salt; // 將密碼與鹽值拼接
            byte[] hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(saltedPassword)); // 對拼接後的字符串進行雜湊
            return Convert.ToBase64String(hashBytes); // 將雜湊結果轉換為 Base64 字符串返回
        } 
    }
} 
public interface IPasswordService
{
    // 這個方法用來加鹽並雜湊密碼，返回雜湊後的密碼和使用的鹽值
    (string HashedPassword, string Salt) HashPassword(string password);
    
    // 這個方法用來驗證輸入的密碼是否與存儲的雜湊密碼匹配
    bool VerifyPassword(string password, string salt, string hashedPassword);
} 
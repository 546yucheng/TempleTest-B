public class PasswordService : IPasswordService
{
    // ��{ HashPassword ��k�A��K�X�i��[�Q�M����
    public (string HashedPassword, string Salt) HashPassword(string password)
    {
        var salt = GenerateSalt(); // �ͦ��@�ӷs���Q��
        var hashedPassword = CreateHashedPassword(password, salt); // �ϥ��Q�ȩM�K�X�ͦ�����K�X
        return (hashedPassword, salt); // ��^����᪺�K�X�M�Q��
    }

    // ��{ VerifyPassword ��k�A���ұK�X�O�_���T
    public bool VerifyPassword(string password, string salt, string hashedPassword)
    {
        var computedHash = CreateHashedPassword(password, salt); // �ϥο�J���K�X�M�Q�ȥͦ�����
        return computedHash == hashedPassword; // ����ͦ�������P�s�x������A��^�O�_�ǰt
    }

    private string GenerateSalt()
    {
        byte[] saltBytes = new byte[16]; // �إߤ@�� 16 �r�`���Q��
        using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
        {
            rng.GetBytes(saltBytes); // �ϥΥ[�K�w�����H���ƥͦ����Ӷ�R�Q��
        }
        return Convert.ToBase64String(saltBytes); // �N�Q���ഫ�� Base64 �r�Ŧ��^
    }

    private string CreateHashedPassword(string password, string salt)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var saltedPassword = password + salt; // �N�K�X�P�Q�ȫ���
            byte[] hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(saltedPassword)); // ������᪺�r�Ŧ�i������
            return Convert.ToBase64String(hashBytes); // �N���굲�G�ഫ�� Base64 �r�Ŧ��^
        } 
    }
} 
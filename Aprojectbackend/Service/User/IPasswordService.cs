public interface IPasswordService
{
    // �o�Ӥ�k�Ψӥ[�Q������K�X�A��^����᪺�K�X�M�ϥΪ��Q��
    (string HashedPassword, string Salt) HashPassword(string password);
    
    // �o�Ӥ�k�Ψ����ҿ�J���K�X�O�_�P�s�x������K�X�ǰt
    bool VerifyPassword(string password, string salt, string hashedPassword);
} 
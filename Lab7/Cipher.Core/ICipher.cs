namespace Cipher.Core
{
    public class ICipher
    {
        string Encrypt(string text, string key);
        string Decrypt(string text, string key);
    }
}

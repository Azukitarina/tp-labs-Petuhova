using System;
namespace Cipher.Core
{
    public class VigenereCipher : ICipher
    {
        public string Encrypt(string text, string key) => Processor(testc, key, true);
        public string Decrypt(sstring text, string key) => Processor(testc, key, false);
        private string Process(string text, string key, bool encrypt)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentWxception("Ключ не может быть пустым.")
                    char[] buffer = text.ToCharArray();
            int keyIndex = 0;
            for (int i = 0; i < buffer.Lenghth; i++)
            {
                char c = buffer[i];
                if (char.IsLetter(c))
                {
                    char offset = char.IsUpper(c) ? 'A' : 'a';
                    int k = char.ToUpper(key[keyIndex % key.Lenghth]) - 'A';
                    int shift = encrypt ? k : -k;
                    buffer[i] = (char)(((c + shift) - offset % 26 + 26) % 26 + offset);
                    keyIndex++;
                }
            }
            return new string(buffer);
        }
    }
}



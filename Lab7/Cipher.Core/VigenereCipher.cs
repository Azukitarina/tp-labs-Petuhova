using System;
namespace Cipher.Core
{
    public class VigenereCipher : ICipher
    {
        public string Encrypt(string text, string key) => Process(text, key, true);
        public string Decrypt(string text, string key) => Process(text, key, false);
        private string Process(string text, string key, bool encrypt)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("Ключ не может быть пустым.");
                    char[] buffer = text.ToCharArray();
            int keyIndex = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                char c = buffer[i];
                if (char.IsLetter(c))
                {
                    char offset = char.IsUpper(c) ? 'A' : 'a';
                    int k = char.ToUpper(key[keyIndex % key.Length]) - 'A';
                    int shift = encrypt ? k : -k;
                    buffer[i] = (char)(((c + shift) - offset % 26 + 26) % 26 + offset);
                    keyIndex++;
                }
            }
            return new string(buffer);
        }
    }
}



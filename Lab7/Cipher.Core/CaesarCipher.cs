using System;

namespace Cipher.Core
{
    public class CaesarCipher : ICipher
    {
        public string Encrypt(string text, string key)
        {
            int shift = int.Parse(key);
            return ShiftText(text, shift);
        }

        public string Decrypt(string text, string key)
        {
            int shift = int.Parse(key);
            return ShiftText(text, -shift);
        }

        private string ShiftText(string text, int shift)
        {
            char[] buffer = text.ToCharArray();
            for (int i = 0; i < buffer.Length; i++)
            {
                char c = buffer[i];
                if (char.IsLetter(c))
                {
                    char offset = char.IsUpper(c) ? 'A' : 'a';
                    buffer[i] = (char)((((c + shift) - offset) % 26 + 26) % 26 + offset);
                }
            }
            return new string(buffer);
        }
    }
}
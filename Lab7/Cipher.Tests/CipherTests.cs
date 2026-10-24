using System;
using Xunit;
using Cipher.Core;

namespace Cipher.Tests
{
    public class CipherTests
    {
        [Theory]
        [InlineData("ABC", "3", "DEF")]
        [InlineData("xyz", "2", "zab")]
        public void Caesar_Encrypt_ReturnsCorrect(string text, string key, string expected)
        {
            var cipher = new CaesarCipher();
            Assert.Equal(expected, cipher.Encrypt(text, key));
        }

        [Fact]
        public void Caesar_Decrypt_ReturnsOriginal()
        {
            var cipher = new CaesarCipher();
            Assert.Equal("Hello", cipher.Decrypt(cipher.Encrypt("Hello", "5"), "5"));
        }

        [Fact]
        public void Caesar_Punctuation_Unchanged()
        {
            var cipher = new CaesarCipher();
            Assert.Equal("B c!1", cipher.Encrypt("A b!1", "1"));
        }

        [Fact]
        public void Vigenere_Encrypt_ReturnsCorrect()
        {
            var cipher = new VigenereCipher();
            Assert.Equal("LXFOPVEFRNHR", cipher.Encrypt("ATTACKATDAWN", "LEMON"));
        }

        [Fact]
        public void Vigenere_Decrypt_ReturnsOriginal()
        {
            var cipher = new VigenereCipher();
            Assert.Equal("HELLO", cipher.Decrypt(cipher.Encrypt("HELLO", "KEY"), "KEY"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Vigenere_EmptyKey_ThrowsException(string? key)
        {
            var cipher = new VigenereCipher();
            Assert.Throws<ArgumentException>(() => cipher.Encrypt("test", key!));
        }

        [Fact]
        public void FileStorage_SavesData()
        {
            var path = "test_output.txt";
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);

            var storage = new FileStorage(path);
            storage.Save("Test message");

            Assert.True(System.IO.File.Exists(path));
            Assert.Contains("Test message", System.IO.File.ReadAllText(path));
        }
    }
}
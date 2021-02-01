using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace TesteMC1.Application.Services
{
    public class CriptografiaService
    {
        private readonly byte[] _iv = new byte[] { 24, 69, 169, 130, 137, 180, 208, 70, 215, 160, 185, 84, 2, 6, 226, 237 };

        public string Criptografar(string textoOriginal, string senha)
        {
            try
            {
                if (string.IsNullOrEmpty(textoOriginal)) return "";
                if (string.IsNullOrEmpty(senha)) throw new Exception("Favor informar a senha a ser utilizada no processo de criptografia!");
                if (senha.Length < 16) throw new Exception("A senha a ser utilizada na criptografia precisa ter mais do que 15 caracteres e símbolos!");

                RijndaelManaged rijndael = new RijndaelManaged();
                ICryptoTransform cryptoTransform = rijndael.CreateEncryptor(Encoding.ASCII.GetBytes(senha), _iv);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
                UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
                byte[] dadosOrigem = unicodeEncoding.GetBytes(textoOriginal);
                cryptoStream.Write(dadosOrigem, 0, dadosOrigem.Length);
                cryptoStream.FlushFinalBlock();
                if (memoryStream.Length == 0) return "";
                byte[] textoCriptografadoBytes = memoryStream.ToArray();
                cryptoStream.Close();
                memoryStream.Close();

                return Convert.ToBase64String(textoCriptografadoBytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Descriptografar(string textoCriptografado, string senha)
        {
            try
            {
                if (string.IsNullOrEmpty(textoCriptografado)) return "";
                if (string.IsNullOrEmpty(senha)) throw new Exception("Favor informar a senha a ser utilizada no processo de descriptografia!");
                if (senha.Length < 16) throw new Exception("A senha a ser utilizada na criptografia precisa ter mais do que 15 caracteres e símbolos!");

                var textoCriptografadoBytes = Convert.FromBase64String(textoCriptografado);

                RijndaelManaged rijndael = new RijndaelManaged();
                ICryptoTransform cryptoTransform = rijndael.CreateDecryptor(Encoding.ASCII.GetBytes(senha), _iv);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
                cryptoStream.Write(textoCriptografadoBytes, 0, textoCriptografadoBytes.Length);
                cryptoStream.FlushFinalBlock();
                if (memoryStream.Length == 0) return "";
                byte[] textoOriginalBytes = memoryStream.ToArray();
                cryptoStream.Close();
                memoryStream.Close();

                UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
                return unicodeEncoding.GetString(textoOriginalBytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

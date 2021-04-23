using System;
using System.Text;
using Api.Services.Interfaces;

namespace Api.Services.Implementations
{
    public class CryptographyService : ICryptographyService
    {
        public string EncryptPassword(string password)
        {
            byte[] buffer = Encoding.Default.GetBytes(password);
            System.Security.Cryptography.SHA1CryptoServiceProvider cryptoTransformSHA1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            string hash = BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");
            return hash;
        }
    }
}
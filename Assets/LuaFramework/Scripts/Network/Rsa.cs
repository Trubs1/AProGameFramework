using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System;

namespace UEncrypt
{

    public class Rsa
    {

        private string privateKey;
        private string publicKey;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="privateKeyXml"></param>
        /// <param name="publicKeyXml"></param>
        public Rsa(string privateKeyXml, string publicKeyXml)
        {
            this.privateKey = privateKeyXml;
            this.publicKey = publicKeyXml;
        }
        

        /// <summary>
        /// 用RSA公钥 加密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] RSAEncrypt(byte[] data)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(this.publicKey);
            byte[] encryptData = rsa.Encrypt(data, false);
            return encryptData;
        }

        /// <summary>
        /// 用RSA公钥 加密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string RSAEncrypt(string data)
        {
            byte[] buf = RSAEncrypt(System.Text.Encoding.GetEncoding("utf-8").GetBytes(data));
            return Convert.ToBase64String(buf);
        }


        /// <summary>
        /// 用RSA私钥 解密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public byte[] RSADecrypt(byte[] data)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(this.privateKey);
            byte[] decryptData = rsa.Decrypt(data, false);
            return decryptData;
        }

        /// <summary>
        /// 用RSA私钥 解密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string RSADecrypt(string data)
        {
            byte[] buf = RSADecrypt(Convert.FromBase64String(data));
            return System.Text.Encoding.GetEncoding("utf-8").GetString(buf);
        }


        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="data">源数据</param>
        /// <param name="desrgbKey"></param>
        /// <param name="desrgbIV"></param>
        /// <returns></returns>
        public static byte[] DESEncrypt(byte[] data, byte[] desrgbKey, byte[] desrgbIV)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(desrgbKey, desrgbIV), CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();
            return memoryStream.ToArray();
        }


        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="data">源数据</param>
        /// <param name="desrgbKey"></param>
        /// <param name="desrgbIV"></param>
        /// <returns></returns>
        public static byte[] DESDecrypt(byte[] data, byte[] desrgbKey, byte[] desrgbIV)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(desrgbKey, desrgbIV), CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();
            return memoryStream.ToArray();
        }


    }

}


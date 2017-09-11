using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace GatePass.Library
{
    public class EncryptDecrypt
    {
        #region TEST

        //UserAuthentication.EncryptDecryptPassword encrypt = new UserAuthentication.EncryptDecryptPassword();
        //string strEncrypted =  encrypt.EncryptPassword("p@ssword");
        //Console.WriteLine(strEncrypted);

        //string strDecrypt = encrypt.DecryptPassword(strEncrypted);
        //Console.WriteLine(strDecrypt);

        //Console.ReadLine();

        #endregion TEST

        #region String Encryption

        /// <summary>
        /// I used the Advanced Encryption Standard
        /// please see the url http://en.wikipedia.org/wiki/Advanced_Encryption_Standard
        /// </summary>
        public class EncryptDecryptPassword
        {

            private static string strSalt = "##@123salt++1!!"; //string salt

            public string StrSalt
            {
                get { return strSalt; }
                set { strSalt = value; }
            }
            private static string strKey = "##@123keyy++1!!"; //string key

            public string StrKey
            {
                get { return strKey; }
                set { strKey = value; }
            }
            private byte[] bSalt = null;


            /// <summary>
            /// ADDED --- RANDOM STRSALT GENERATOR AND RANDOM STRKEY GENERATOR
            /// Author: Mr. Jan Russel N. Calachan
            /// Team Leader
            /// Jin Framework Solutions
            /// </summary>
            private readonly Random _rng = new Random();
            private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            public string RandomString(int size)
            {
                char[] buffer = new char[size];

                for (int i = 0; i < size; i++)
                {
                    buffer[i] = _chars[_rng.Next(_chars.Length)];
                }
                return new string(buffer);
            }

            public byte[] Salt
            {
                get
                {
                    if (bSalt == null)
                        throw new ArgumentNullException("Unexpected error! Invalid operation");
                    else
                        return this.bSalt;
                }
            }

            public string EncryptPassword(string strPassword)
            {
                string strRet = "";

                //1. create a symmetric-algorithm object

                RijndaelManaged algo = new RijndaelManaged();

                //2. specifies keys

                bSalt = Encoding.ASCII.GetBytes(strSalt);

                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(strKey, bSalt);

                algo.BlockSize = 256;
                algo.Mode = CipherMode.CBC;
                algo.Key = key.GetBytes(algo.KeySize / 8);
                algo.IV = key.GetBytes(algo.BlockSize / 8);

                using (ICryptoTransform encryptor = algo.CreateEncryptor())     //3. ICryptoTransform object
                using (MemoryStream ms = new MemoryStream()) // will handle encrypted values
                {
                    using (CryptoStream encryptStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))  //4. CryptoStream
                    using (StreamWriter wrtr = new StreamWriter(encryptStream))  //5. Write it to a stream
                    {
                        wrtr.AutoFlush = true;
                        wrtr.Write(strPassword);

                        encryptStream.FlushFinalBlock();
                        wrtr.Flush();
                        strRet = Convert.ToBase64String(ms.GetBuffer(), 0, (int) ms.Length);
                    }
                }

                return strRet;
            }

            public string DecryptPassword(string strPassword) // strpassword is the cipher text
            {
                string strRet = "";

                //1. create a symmetricalgorithm object

                RijndaelManaged algo = new RijndaelManaged();

                //2. specifies keys

                bSalt = Encoding.ASCII.GetBytes(strSalt);

                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(strKey, bSalt);

                algo.BlockSize = 256;
                algo.Mode = CipherMode.CBC;
                algo.Key = key.GetBytes(algo.KeySize / 8);
                algo.IV = key.GetBytes(algo.BlockSize / 8);

                using (ICryptoTransform encryptor = algo.CreateDecryptor())     //3. ICryptoTransform object
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(strPassword))) // will handle encrypted values
                using (CryptoStream encryptStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Read))  //4. CryptoStream
                using (StreamReader rdr = new StreamReader(encryptStream))  //5. Write it to a stream
                    strRet = rdr.ReadToEnd();

                return strRet;
            }



            public string DecryptPassword(string strPassword, string strSalty, string strKeyy) // strpassword is the cipher text
            {
                string strRet = "";

                //1. create a symmetricalgorithm object

                RijndaelManaged algo = new RijndaelManaged();

                //2. specifies keys

                bSalt = Encoding.ASCII.GetBytes(strSalty);

                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(strKeyy, bSalt);

                algo.BlockSize = 256;
                algo.Mode = CipherMode.CBC;
                algo.Key = key.GetBytes(algo.KeySize / 8);
                algo.IV = key.GetBytes(algo.BlockSize / 8);

                using (ICryptoTransform encryptor = algo.CreateDecryptor())     //3. ICryptoTransform object
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(strPassword))) // will handle encrypted values
                using (CryptoStream encryptStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Read))  //4. CryptoStream
                using (StreamReader rdr = new StreamReader(encryptStream))  //5. Write it to a stream
                    strRet = rdr.ReadToEnd();

                return strRet;
            }
        }

        #endregion String Encryption
    }
}
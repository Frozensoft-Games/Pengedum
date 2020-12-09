using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

class Rijndael
{
    public string Decrypt(byte[] soup, string key) // key must be 32chars
    {
        string outString = "";

        try
        {
            byte[] iv = Encoding.ASCII.GetBytes("1234567890123456");
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);

            // Create a new instance of the Rijndael
            // class.  This generates a new key and initialization
            // vector (IV).
            using (System.Security.Cryptography.Rijndael myRijndael = System.Security.Cryptography.Rijndael.Create())
            {
                myRijndael.Key = keyBytes;
                myRijndael.IV = iv;

                // Decrypt the bytes to a string.
                outString = DecryptStringFromBytes(soup, myRijndael.Key, myRijndael.IV);
            }
        }
        catch
        {           
        }

        return outString;
    }

    public byte[] Encrypt(string original, string key) // key must be 32chars
    {
        byte[] encrypted = null;

        try
        {
            byte[] iv = Encoding.ASCII.GetBytes("1234567890123456");
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);

            // Create a new instance of the Rijndael
            // class.  This generates a new key and initialization
            // vector (IV).
            using (System.Security.Cryptography.Rijndael myRijndael = System.Security.Cryptography.Rijndael.Create())
            {
                myRijndael.Key = keyBytes;
                myRijndael.IV = iv;

                // Encrypt the string to an array of bytes.
                encrypted = EncryptStringToBytes(original, myRijndael.Key, myRijndael.IV);
            }
        }
        catch
        {
        }

        return encrypted;
    }

    static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (plainText == null || plainText.Length <= 0)
            throw new ArgumentNullException("plainText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");
        byte[] encrypted;
        // Create an Rijndael object
        // with the specified key and IV.
        using (System.Security.Cryptography.Rijndael rijAlg = System.Security.Cryptography.Rijndael.Create())
        {
            rijAlg.Key = Key;
            rijAlg.IV = IV;

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {

                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        // Return the encrypted bytes from the memory stream.
        return encrypted;
    }

    static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (cipherText == null || cipherText.Length <= 0)
            throw new ArgumentNullException("cipherText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");

        // Declare the string used to hold
        // the decrypted text.
        string plaintext = null;

        // Create an Rijndael object
        // with the specified key and IV.
        using (System.Security.Cryptography.Rijndael rijAlg = System.Security.Cryptography.Rijndael.Create())
        {
            rijAlg.Key = Key;
            rijAlg.IV = IV;

            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {

                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        return plaintext;
    }
}

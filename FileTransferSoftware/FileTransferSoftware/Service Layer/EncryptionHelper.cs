using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class EncryptionHelper
{
    private static readonly string KEY = "1234567890ABCDEF"; // Same 16-char key
    private static readonly string IV = "RandomInitVector"; // Same 16-char IV

    public static string Decrypt(string encrypted)
    {
        if (string.IsNullOrWhiteSpace(encrypted))
            return null;

        try
        {
            byte[] cipherText = Convert.FromBase64String(encrypted);
            byte[] keyBytes = Encoding.UTF8.GetBytes(KEY);
            byte[] ivBytes = Encoding.UTF8.GetBytes(IV);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Invalid base64 input: {encrypted}");
        }
        catch (CryptographicException ex)
        {
            Console.WriteLine($"Decryption failed (probably wrong key/IV): {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error in Decrypt(): {ex.Message}");
        }

        return null;
    }


    public static string Encrypt(string plainText)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(KEY);
        byte[] ivBytes = Encoding.UTF8.GetBytes(IV);
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;
            aesAlg.Key = keyBytes;
            aesAlg.IV = ivBytes;

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                    csEncrypt.FlushFinalBlock();

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }
    }

}

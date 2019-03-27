using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities.Encoders;

/// <summary>
/// 加密算法类
/// </summary>
public static class EncryptionUtils
{
    public static byte[] Encrypt(MemoryStream data, byte[] key, byte[] iv)
    {
#if UNITY_WP8
        IBlockCipher blockCipher = new CfbBlockCipher(new AesFastEngine(), 16 * 8);
        IBufferedCipher c1 = new PaddedBufferedBlockCipher(blockCipher);
        KeyParameter k= new KeyParameter(key);
        c1.Init(true, new ParametersWithIV(k, iv));
        return c1.DoFinal(data.ToArray());
#else
        Rijndael alg = Rijndael.Create();
        alg.Key = key;
        alg.IV = iv;
        alg.Mode = CipherMode.CFB;
        ICryptoTransform cTransform = alg.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(data.ToArray(), 0, (int)data.Length);

        return resultArray;
#endif
    }

    public static byte[] Decrypt(MemoryStream data, byte[] key, byte[] iv)
    {
#if UNITY_WP8
        IBlockCipher blockCipher = new CfbBlockCipher(new AesFastEngine(), 16 * 8);
        IBufferedCipher c1 = new PaddedBufferedBlockCipher(blockCipher);
        KeyParameter k = new KeyParameter(key);
        c1.Init(true, new ParametersWithIV(k, iv));
        return c1.DoFinal(data.ToArray());
#else
        RijndaelManaged alg = new RijndaelManaged();
        alg.Key = key;
        alg.IV = iv;
        alg.Mode = CipherMode.CFB;
        ICryptoTransform cTransform = alg.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(data.ToArray(), 0, (int)data.Length);

        return resultArray;
#endif
    }

    private readonly static byte[] _codes = { 46, 56, 194, 57, 81, 203, 34, 37, 237, 175, 67, 96, 233, 185, 40, 248, 108, 187, 250, 103, 183, 9, 8, 249, 222, 238, 136, 164, 110, 107, 39, 13, 20, 88, 128, 229, 26, 11, 122, 109, 83, 156, 148, 151, 19, 210, 190, 123, 163, 24, 199, 132, 176, 41, 145, 94, 3, 6, 61, 230, 12, 133, 174, 29, 69, 113, 130, 105, 93, 79, 64, 182, 134, 153, 152, 209, 98, 87, 232, 173, 101, 97, 73, 85, 140, 82, 106, 120, 198, 44, 18, 129, 95, 48, 126, 131, 146, 10, 1, 211, 212, 135, 147, 219, 247, 14, 2, 65, 244, 181, 223, 89, 142, 143, 72, 141, 4, 157, 36, 90, 170, 217, 235, 171, 239, 251, 158, 80, 33, 205, 236, 84, 119, 31, 5, 200, 42, 111, 155, 17, 169, 180, 59, 137, 226, 114, 159, 218, 220, 47, 179, 68, 241, 255, 138, 117, 22, 197, 55, 30, 196, 186, 144, 121, 27, 60, 231, 189, 178, 161, 86, 74, 127, 50, 71, 53, 234, 21, 35, 201, 99, 32, 115, 91, 192, 116, 243, 62, 213, 77, 43, 118, 225, 154, 184, 52, 191, 66, 92, 104, 206, 112, 15, 38, 216, 168, 102, 253, 58, 166, 63, 172, 165, 149, 7, 245, 242, 49, 240, 221, 204, 195, 76, 193, 246, 214, 177, 45, 252, 78, 208, 150, 139, 224, 162, 16, 125, 75, 228, 124, 23, 254, 70, 227, 202, 167, 215, 28, 54, 207, 188, 160, 100, 51, 25 };

    public static byte[] DecryptXor(MemoryStream data, int keyIndex)
    {
        byte[] resultArray = data.ToArray();
        int count = (int)data.Length;
        byte code = _codes[keyIndex];
        for (int i = 0; i < count; ++i)
        {
            resultArray[i] = (byte)(resultArray[i] ^ code);
        }
        return resultArray;
    }


    private static byte[] _key = TextUtils.GetBytes("3258c33492acee12b5b9564ac785c9a9");
    private static string _secret = "6b6a042dd5a0cd7a975a208cab15b671";

    public static string EncodeForRegist(string txt)
    {
        byte[] process1 = new byte[txt.Length * 2];

        //加密Byte[]数组  
        byte[] md5 = TextUtils.GetBytes(MD5(TextUtils.GetBytes(UnityEngine.Random.Range(0, 32000).ToString())));
        byte[] baseData = TextUtils.GetBytes(txt);

        int i = 0;
        for (int j = 0; j < baseData.Length; ++j)
        {
            if (i == md5.Length) i = 0;
            process1[j + j] = md5[i];
            process1[j + j + 1] = (byte)(md5[i++] ^ baseData[j]);
        }

        byte[] keyMd5 = TextUtils.GetBytes(MD5(_key));
        byte[] process2 = new byte[process1.Length];
        i = 0;
        for (int j = 0; j < process1.Length; ++j)
        {
            if (i == keyMd5.Length) i = 0;
            process2[j] = (byte)(keyMd5[i++] ^ process1[j]);
        }

        return Convert.ToBase64String(process2);
    }

    public static string DecodeForRegist(string txt)
    {
        byte[] baseData = Convert.FromBase64String(txt);
        byte[] process1 = new byte[baseData.Length];

        byte[] keyMd5 = TextUtils.GetBytes(MD5(_key));
        int i = 0;
        for (int j = 0; j < baseData.Length; ++j)
        {
            if (i == keyMd5.Length) i = 0;
            process1[j] = (byte)(keyMd5[i++] ^ baseData[j]);
        }

        byte[] process2 = new byte[process1.Length / 2];
        i = 0;
        for (int j = 0; j < process1.Length; j += 2)
        {
            process2[i++] = (byte)(process1[j] ^ process1[j + 1]);
        }

        return TextUtils.GetString(process2);
    }


    public static string GetSign(string str)
    {
        return MD5(TextUtils.GetBytes(str + _secret));
    }

    public static string MD5(byte[] bytes)
    {
        MD5CryptoServiceProvider md5CSP = new MD5CryptoServiceProvider();
        byte[] targetData = md5CSP.ComputeHash(bytes);
        string byte2String = null;

        for (int i = 0; i < targetData.Length; i++)
        {
            byte2String += (targetData[i] / 16).ToString("x");
            byte2String += (targetData[i] % 16).ToString("x");
        }

        return byte2String; 
    }

    public static string UrlEncode(string str)
    {
        StringBuilder sb = new StringBuilder();
        byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
        for (int i = 0; i < byStr.Length; i++)
        {
            sb.Append(@"%" + Convert.ToString(byStr[i], 16));
        }

        return (sb.ToString());
    }
}


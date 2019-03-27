using UnityEngine;
using System.Collections;

public class CMyEncryptFile
{
    static int ENCRYPT_SIZE = 128;
    static byte[] m_bufCode = null;
    int seeeeeeed = 18011802;
    public CMyEncryptFile()
    {
        if (m_bufCode == null)
        {
            m_bufCode = new byte[ENCRYPT_SIZE];
            Random.seed = seeeeeeed;
            for (int i = 0; i < ENCRYPT_SIZE; ++i)
            {
                m_bufCode[i] = (byte)Random.Range(0, 255);
            }
        }
    }

    //º”√‹
    public byte[] Encrypt(byte[] pBuf, int nSize)
	{
		if (pBuf == null || nSize <= 0) {
			return null;
		}

        byte[] pNewBuf = new byte[nSize + 6];
        byte[] wodong = System.Text.Encoding.Default.GetBytes("WoDong");
        wodong.CopyTo(pNewBuf, 0);
        int temp;
		for (int i = 0; i < nSize; i++) 
        {
            temp = (int)pBuf[i];
            //temp = (temp << (i % 8)) + (temp >> (8 - i % 8));
            temp = temp ^ m_bufCode[i % ENCRYPT_SIZE];
            pNewBuf[i+6] = (byte)temp;
        }
        return pNewBuf;
	}

    //Ω‚√‹
    public byte[] Decrypt(byte[] pBuf, int nSize)
	{
        if (pBuf == null || nSize <= 0)
        {
			return null;
        }

        nSize = nSize - 6;
        byte[] pNewBuf = new byte[nSize];
        int temp;
        for (int i = 0; i < nSize; i++)
        {
            temp = (int)pBuf[i + 6];
            temp = temp ^ m_bufCode[i % ENCRYPT_SIZE];
            //temp = (temp >> (i % 8)) + (temp << (8 - i % 8));
            pNewBuf[i] = (byte)temp;
		}
        return pNewBuf;
	}
}

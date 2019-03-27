
using UnityEngine;
using System.Collections;
using System.IO;

public class CopyIOSIcon
{
	static char SepChar = Path.AltDirectorySeparatorChar;
	public static void MoveIcon(string desdir)
    { 
        string iconPath = Directory.GetParent(Application.dataPath).FullName;
		string srcdir = Path.Combine (iconPath, "IOSIcon" + SepChar + "Unity-iPhone");

		string folderName = Path.GetFileName(srcdir);
        Debug.LogWarning("folderName " + folderName);
		
		string desfolderdir = desdir + SepChar + folderName;
		
		if (desdir.LastIndexOf(SepChar) == (desdir.Length - 1)) {
			desfolderdir = desdir + folderName;
		}
		deleteDestDir (desfolderdir);

		CopyDirectory (srcdir, desdir);
    }

	private static void deleteDestDir(string desdir){
        if (!Directory.Exists(desdir)) {
            return;
        }
		string[] filenames = Directory.GetFileSystemEntries(desdir);
		foreach (string file in filenames)// 遍历所有的文件和目录
		{
			if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
			{
				deleteDestDir(file);
				Directory.Delete(file);
			}
			else // 否则直接copy文件
			{
				Debug.LogWarning("del file " + file);
				File.Delete(file);
			}
		}
	}

    private static void CopyDirectory(string srcdir, string desdir)
    {
        string folderName = Path.GetFileName(srcdir);
		//Debug.LogWarning("folderName " + folderName);
		
        string desfolderdir = desdir + SepChar + folderName;

        if (desdir.LastIndexOf(SepChar) == (desdir.Length - 1)) {
            desfolderdir = desdir + folderName;
        }

        string[] filenames = Directory.GetFileSystemEntries(srcdir);
        foreach (string file in filenames)// 遍历所有的文件和目录
        {
			if (file.Contains(".meta")) {
				continue;
			}
            if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
            {
                string fileName = Path.GetFileName(file);
                string currentdir = desfolderdir + SepChar + fileName;
                if (!Directory.Exists(currentdir))
                {
                    Directory.CreateDirectory(currentdir);
                }
//				Debug.LogWarning("CopyDirectory " + file);
//				Debug.LogWarning("desfolderdir " + desfolderdir);
				CopyDirectory(file, desfolderdir);
            }
            else // 否则直接copy文件
            {
                //Debug.LogWarning("file " + file);
                //Debug.LogWarning("SepChar " + SepChar);
                //string srcfileName = file.Substring(file.LastIndexOf(SepChar) + 1);
                string srcfileName = Path.GetFileName(file);
                srcfileName = desfolderdir + SepChar + srcfileName;
                if (!Directory.Exists(desfolderdir))
                {
                    //Debug.LogWarning("desfolderdir " + desfolderdir);
                    Directory.CreateDirectory(desfolderdir);
                }
                //Debug.LogWarning("copy file " + file);
                //Debug.LogWarning("srcfileName " + srcfileName);
                File.Copy(file, srcfileName, true);
            }
        }

    }
}

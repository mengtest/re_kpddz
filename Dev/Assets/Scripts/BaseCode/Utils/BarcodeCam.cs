using UnityEngine;
using System.Collections;
using Utils;
using ZXing;//引入库  
using ZXing.QrCode;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using ZXing.Common;
using ZXing.Rendering;
[SLua.CustomSingletonLuaClass]
public class BarcodeCam : Singleton<BarcodeCam>
{
    private static Dictionary<string, Texture2D> savePic = new Dictionary<string, Texture2D>();
    private static Dictionary<string, string> picUrlList = new Dictionary<string, string>();
    public static Texture2D encoded;
    public static Texture2D Testencoded;
    public string Lastresult;
    public static string userId = "";
    List<string> _sharePicUrl = new List<string>();
    public static List<string> SharePicList = new List<string>();
    public static RawImage s;
    public static int GetPicListLenth()
    {
        return SharePicList.Count;
    }

    public static string GetSharePic(int pos)
    {
        if (pos < SharePicList.Count)
        {
            return SharePicList[pos];
        }
        return "";
    }
    void Awake()
    {

    }
    void Start()
    {


    }
    public void InitParam()
    {
        encoded = new Texture2D(300, 300);
        userId = GameDataMgr.PLAYER_DATA.Account.ToString();
        InitData();

        Lastresult = GameDataMgr.PLAYER_DATA.ShareURL;
        Lastresult = Lastresult.Replace("\\", "");
        GetQRI();
        _sharePicUrl.Clear();
        SharePicList.Clear();
        AddShareUrl(GameDataMgr.PLAYER_DATA.ShareStr1);
        AddShareUrl(GameDataMgr.PLAYER_DATA.ShareStr2);
        AddShareUrl(GameDataMgr.PLAYER_DATA.ShareStr3);
        AddShareUrl(GameDataMgr.PLAYER_DATA.ShareStr4);
        AddShareUrl(GameDataMgr.PLAYER_DATA.ShareStr5);
        AddShareUrl(GameDataMgr.PLAYER_DATA.ShareStr6);
        LoadUrlPic();
    }
    void AddShareUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return;
        url = url.Replace("\\", "");
        _sharePicUrl.Add(url);
    }
    void LoadUrlPic()
    {
        for (int i = 0; i < _sharePicUrl.Count; i++)
        {
            StartCoroutine(LoadPic(_sharePicUrl[i], i.ToString()));
        }
    }
    public static IEnumerator LoadPic(string path, string key)
    {
        if (path.Length <= 3)
            yield break;
        WWW www = new WWW(path);
        yield return www;
        if (www != null && string.IsNullOrEmpty(www.error))
        {
            if (savePic.ContainsKey(key))
                savePic[key] = www.texture;
            else
                savePic.Add(key, www.texture);
            SavePicToLocal(key, path);
            Texture2D newTex = www.texture;
            SharePictureConfigItem picData = ConfigDataMgr.getInstance().SharePictureConfig.GetDataByKey(key);
            if (picData == null)
                yield break;
            Testencoded = new Texture2D(picData.qrisize, picData.qrisize);
            int pos_x = picData.pos_x - picData.qrisize / 2;
            int pos_y = picData.pos_y - picData.qrisize / 2;
            EncodeNew(Testencoded, BarcodeCam.getInstance().Lastresult, Testencoded.width, Testencoded.height, 0);
            newTex.SetPixels32(pos_x, pos_y, Testencoded.width, Testencoded.height, Testencoded.GetPixels32());
            newTex.Apply();
            //          if (sdk.SDKManager.isAppStoreVersion())
            //          {
            //              float targetWidth_f = (float)newTex.width * 0.6f;
            //              float targetHight_f = (float)newTex.height * 0.6f;
            //              int targetWidth_d = (int)targetWidth_f;
            //              int targetHight_d = (int)targetHight_f;
            //              Texture2D smallTex = new Texture2D(targetWidth_d, targetHight_d, newTex.format, false);
            //              targetWidth_d = smallTex.width;
            //              targetHight_d = smallTex.height;
            //              targetWidth_f = (float)targetWidth_d;
            //              targetHight_f = (float)targetHight_d;
            //              for (int i = 0; i < targetHight_d; i++)
            //              {
            //                  for (int j = 0; j < targetWidth_d; j++)
            //                  {
            //                      Color newColor = newTex.GetPixelBilinear((float)j / targetWidth_f, (float)i / targetHight_f);
            //                      smallTex.SetPixel(j,i,newColor);
            //                  }
            //              }
            //              smallTex.Apply();
            //              newTex = smallTex;
            //          }
            string newKey = key + "_qri";
            if (savePic.ContainsKey(newKey))
                savePic[newKey] = newTex;
            else
                savePic.Add(newKey, newTex);
            SavePicToLocal(newKey, path);
            string sPath = Application.persistentDataPath;
            string filePath = string.Format("{0}/ShareIcon/{1}/{2}.png", sPath, userId, newKey);
            SharePicList.Add(filePath);
        }
    }

    public static void SetQRIToUITexture(Transform tex)
    {
        UITexture mainTex = tex.GetComponent<UITexture>();
        mainTex.mainTexture = encoded;
    }
    public static void SetQRIToUITexture2(Transform tex)
    {
        UITexture mainTex = tex.GetComponent<UITexture>();
        mainTex.mainTexture = Testencoded;
    }

    private static Color32[] Encode(string textForEncoding, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width,
                Margin = 0
            }
        };
        return writer.Write(textForEncoding);
    }
    private static void EncodeNew(Texture2D tex, string textForEncoding, int width, int height, int magin = 0)
    {
        BitMatrix BIT;
        Dictionary<EncodeHintType, object> hints = new Dictionary<EncodeHintType, object>();

        //设置编码方式  
        hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
        hints.Add(EncodeHintType.MARGIN, magin);
        BIT = new MultiFormatWriter().encode(textForEncoding, BarcodeFormat.QR_CODE, width, height, hints);

        int bitWidth = BIT.Width;
        int bitHeight = BIT.Width;
        for (int x = 0; x < bitHeight; x++)
        {
            for (int y = 0; y < bitWidth; y++)
            {
                if (BIT[x, y])
                {
                    tex.SetPixel(y, x, Color.black);
                }
                else
                {
                    tex.SetPixel(y, x, Color.white);
                }

            }
        }
        tex.Apply();
    }
    private static void SavePicToLocal(string key, string sUrl)
    {
        string sPath = Application.persistentDataPath;
        sPath = sPath.Replace("\\", "/");
        if (picUrlList.ContainsKey(key))
        {
            picUrlList[key] = sUrl;
        }
        else
        {
            picUrlList.Add(key, sUrl);
        }
        Texture2D imgage = savePic[key];
        var bys = imgage.EncodeToPNG();//转换图片资源  


        //创建临时文件
        string filePath = string.Format("{0}/ShareIcon/{1}/{2}.png", sPath, userId, key);
        string tempFilePath = string.Format("{0}/ShareIcon/{1}/{2}_temp.png", sPath, userId, key);
        string dirPath = Path.GetDirectoryName(filePath);
        FileStream stream;
        try
        {
            stream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write);
            //stream = File.Create(tempFilePath);
        }
        catch (System.Exception ex)
        {
            Debug.LogException(ex);
            return;
        }

        //写临时文件
        try
        {
            stream.Write(bys, 0, bys.Length);
            stream.Flush();
            stream.Close();
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            return;
        }
        //名字改为正式
        try
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.SetAttributes(filePath, FileAttributes.Normal);
                System.IO.File.Delete(filePath);
            }
            System.IO.File.Move(tempFilePath, filePath);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            return;
        }
        INIParser pIniFile = new INIParser();
        pIniFile.Open(string.Format("{0}/ShareIcon/IconConfig.ini", sPath));
        pIniFile.WriteValue("IconList", key, sUrl);
        pIniFile.Close();

    }

    void GetQRI()
    {
        var textForEncoding = Lastresult;
        if (textForEncoding != null)
        {
            //二维码写入图片  
            //             var color32 = Encode(textForEncoding, encoded.width, encoded.height);
            //             encoded.SetPixels32(color32);
            //             encoded.Apply();
            EncodeNew(encoded, textForEncoding, encoded.width, encoded.height);
            if (string.IsNullOrEmpty(userId))
                return;
            //生成的二维码图片附给RawImage  
            savePic[userId] = encoded;
            SavePicToLocal(userId, "localqri");

        }
    }
    void InitData()
    {
        string sPath = Application.persistentDataPath;
        sPath = sPath.Replace("\\", "/");
        INIParser pIniFile = new INIParser();
        if (!System.IO.Directory.Exists(string.Format("{0}/ShareIcon", sPath)))
        {

            System.IO.Directory.CreateDirectory(string.Format("{0}/ShareIcon", sPath));
            System.IO.File.SetAttributes(string.Format("{0}/ShareIcon", sPath), FileAttributes.Normal);
        }
        if (!System.IO.Directory.Exists(string.Format("{0}/ShareIcon/{1}", sPath, userId)))
        {

            System.IO.Directory.CreateDirectory(string.Format("{0}/ShareIcon/{1}", sPath, userId));
            System.IO.File.SetAttributes(string.Format("{0}/ShareIcon/{1}", sPath, userId), FileAttributes.Normal);
        }
        pIniFile.Open(string.Format("{0}/ShareIcon/IconConfig.ini", sPath));
        picUrlList.Clear();
        Dictionary<string, string> dic_icon = pIniFile.GetOneSection("IconList");
        if (dic_icon != null)
        {
            foreach (KeyValuePair<string, string> dic in dic_icon)
            {
                if (File.Exists(string.Format("{0}/ShareIcon/{1}/{2}.png", sPath, userId, dic.Key)))//首先判断一下该图片文件是否存在  
                {
                    picUrlList.Add(dic.Key, dic.Value);
                }
            }
        }
        pIniFile.Close();

    }
    // 
    //         //将图片画出来  
    //         void OnGUI()
    //         {
    //             GUI.DrawTexture(new Rect(100, 100, 256, 256), encoded);
    //         }  

}
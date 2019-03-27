using UnityEngine;
using System.Collections;
using Utils;

public class GameHeadHttpUp : AsyncHttpResponseListner
{
    public static GameHeadHttpUp Instance;


    void Awake()
    {
        Instance = this;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /*public override void onStart()
    {

    }

    public override void onProgress(long currentSize, long totalSize)
    {
    }

    public override void onSuccess(string statusCode, string filePath, string msg)
    {
        UtilTools.MessageDialog("上传头像成功。。。statusCode="+statusCode+"  filePath = "+filePath+"  msg = "+msg);
    }

    public override void onFailure(string statusCode, string msg)
    {
        UtilTools.MessageDialog("上传头像失败。。。statusCode="+statusCode+"  msg = "+msg);
    }

    public override void onFinish()
    {
        UtilTools.ShowMessage("上传结束。。");
    }

    public override void onRetry()
    {
    }*/
    public override void onStartAbstract()
    {

    }

    public override void onProgressAbstract(long currentSize, long totalSize)
    {
        
    }

    public override void onSuccessAbstract(string statusCode, string filePath, string msg)
    {
        //UtilTools.MessageDialog("上传头像成功。。。statusCode="+statusCode+"  filePath = "+filePath+"  msg = "+msg);+
        Debug.Log("onsuccess:PicCount=" + GameDataMgr.PLAYER_DATA.PicCount);
        var count = int.Parse(GameDataMgr.PLAYER_DATA.PicCount) + 1;
        if (count > 1){
            count = 0;
        }
        string iconUrl = BaseConfig.HeadSaveImgUrl + GameDataMgr.PLAYER_DATA.Account + count + ".png";
        Debug.Log("onsuccess:iconUrl=" + iconUrl);
		GameDataMgr.PLAYER_DATA.Icon = iconUrl+"?time="+UtilTools.GetClientTime();
        //选择玩家头像下载下来
        Debug.Log("onsuccess:icon=" + GameDataMgr.PLAYER_DATA.Icon);
        Invoke("DelayLoadIcon", 1.3f);
    }
    public void DelayLoadIcon()
    {
        GameHeadLoader.Instance.LoadMainTexture(GameDataMgr.PLAYER_DATA.Icon);
    }
    public override void onFailureAbstract(string statusCode, string msg)
    {
        UtilTools.MessageDialog(GameText.GetStr("headIconUp_failed"));
    }

    public override void onFinishAbstract()
    {
        UtilTools.ShowMessage(GameText.GetStr("headIconUp_finished"));
    }

    public override void onRetryAbstract()
    {
    }
}

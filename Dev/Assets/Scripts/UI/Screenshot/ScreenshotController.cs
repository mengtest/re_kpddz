using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI.Controller;
using EventManager;
using network.protobuf;
using Scene;


public class ScreenshotController : ControllerBase {

    private ScreenshotMono monoComponent;
    public ScreenshotController(string uiName)
	{
		sName = uiName;
		ELevel = UILevel.BACKGROUND;
        prefabsPath = new string[] { UIPrefabPath.SCREENSHOT_WIN };
	}

    /// <summary>
    /// 界面加载完成后的回调
    /// </summary>
    protected override void UICreateCallback()
    {
        monoComponent = winObject.AddComponent<ScreenshotMono>();
    }

    /// <summary>
    /// 销毁后的处理
    /// </summary>
    protected override void UIDestroyCallback()
    {

        if (monoComponent != null)
        {
            UnityEngine.Object.DestroyImmediate(monoComponent);
            monoComponent = null;
        }
    }


    public void ShowScreenshot()
    {
        if (monoComponent != null && GameSceneManager.sceneCameraObj != null)
        {
           // monoComponent.ShowTexture();
            monoComponent.TakeAPhoto();
            GameSceneManager.sceneCameraObj.SetActive(false);
        }
    }
    public void HideScreenshot()
    {
        if (GameSceneManager.sceneCameraObj != null)
        {
            GameSceneManager.sceneCameraObj.SetActive(true);
        }
        if (monoComponent != null)
        {
            monoComponent.HideTexture();
        }
    }
}

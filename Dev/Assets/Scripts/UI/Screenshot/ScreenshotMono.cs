using System;
using UnityEngine;
using System.Collections;
using UI.Controller;
using Scene;

public class ScreenshotMono : MonoBehaviour
{
    ScreenshotController controller;
    UITexture uiTexture;
    Material mat;
    RenderTexture mTex;
    //Texture2D screenShot;
    void Awake()
    {
        controller = (ScreenshotController)UIManager.GetControler(UIName.SCREENSHOT_WIN);
        uiTexture = transform.Find("Container/Texture").gameObject.GetComponent<UITexture>();
    }

    void Start()
    {
//         mTex = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);//第3参数：深度信息的精确度
//         mTex.anisoLevel = 0;
//         mTex.name = transform.name + GetInstanceID();
//         mTex.wrapMode = TextureWrapMode.Clamp;
    }

    /// <summary>  
    /// 对相机截图。   
    /// </summary>  
    /// <returns>The screenshot2.</returns>  
    /// <param name="camera">Camera.要被截屏的相机</param>  
    /// <param name="rect">Rect.截屏的区域</param>  
    void CaptureCamera(Camera camera)
    {
        GameSceneManager.sceneCameraObj.SetActive(true);
        mTex = new RenderTexture(Screen.width, Screen.height, 0);//第3参数：深度信息的精确度
        mTex.anisoLevel = 0;
        mTex.name = transform.name + GetInstanceID();
        mTex.wrapMode = TextureWrapMode.Clamp;


        // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机
        camera.targetTexture = mTex;
        camera.Render();

        // 激活这个rt, 并从中中读取像素。  
        //RenderTexture.active = mTex;

        Shader shader = Shader.Find("Unlit/Transparent Colored Alpha");
        mat = new Material(shader);
        uiTexture.material = mat;

//         Rect rect = new Rect(0, 0, (float)Screen.width, (float)Screen.height);
//          screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
//          screenShot.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
//          screenShot.Apply();
//          screenShot.anisoLevel = 0;
//         mat.mainTexture = screenShot;
         uiTexture.material = mat;

         mat.mainTexture = mTex;
        // 重置相关参数，以使用camera继续在屏幕上显示  
         RenderTexture.active = null; // JC: added to avoid errors  
         camera.targetTexture = null;
         BaseScene baseScene = GameSceneManager.getInstance().SceneMono;
         if (baseScene != null)
             baseScene.BlurClose();
    }


    public void TakeAPhoto()
    {
        if (transform.Find("Container/Texture").gameObject.activeSelf)//如果已经显示了截屏，则不用截屏
        {
            return;
        }
        transform.Find("Container/Texture").gameObject.SetActive(true);
        Camera cam = GameSceneManager.sceneCameraObj.GetComponent<Camera>();
        CaptureCamera(cam);
    }

//     public void ShowTexture()
//     {
//         transform.Find("Container/Texture").gameObject.SetActive(true);
//     }

    public void HideTexture()
    {
        transform.Find("Container/Texture").gameObject.SetActive(false);
        if (mat != null && mat.mainTexture != null)
        {
            Destroy(mat.mainTexture);
            mat.mainTexture = null;
        }
        if (uiTexture != null && uiTexture.material != null)
        {
            Destroy(uiTexture.material);
            uiTexture.material = null;
        }
//         if (screenShot != null)
//         {
//             Destroy(screenShot);
//             screenShot = null;
//         }
        if (mTex != null)
        {
            Destroy(mTex);
            mTex = null;
        }
    }
}  
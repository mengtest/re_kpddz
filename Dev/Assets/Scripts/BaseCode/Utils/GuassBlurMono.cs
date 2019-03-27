/***************************************************************


 *
 *
 * Filename:  	GuassBlurMono.cs	
 * Summary: 	UI背景高斯模糊
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2017/04/06 22:38
 ***************************************************************/


using UnityEngine;
using Scene;

namespace Utils
{
    public class GuassBlurMono : MonoBehaviour
    {
        //Target texture
        RenderTexture _renderTex = null;

        // Use this for initialization
        void Start()
        {
            var goCamera = GameSceneManager.sceneCameraObj;
            if (goCamera == null)
                return;

            var cam = goCamera.GetComponent<Camera>();
            if (cam == null)
                return;

            _renderTex = new RenderTexture(1136, 640, 8);
            cam.targetTexture = _renderTex;

            //Set background
            UITexture uiTexture = GetComponent<UITexture>();
            if (uiTexture != null)
            {
                uiTexture.mainTexture = _renderTex;
                cam.Render();
            }
           
            // Reset camera
            cam.targetTexture = null;
        }
    }
}


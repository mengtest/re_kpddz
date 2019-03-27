/***************************************************************


 *
 *
 * Filename:  	RadialBlur.cs	
 * Summary: 	ImageEffect 镜像模糊
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/10/09 22:41
 ***************************************************************/


using UnityEngine;
using System.Collections;


namespace Utils
{
    public class RadialBlur : MonoBehaviour
    {
        public float _fSampleDist = 1.0f; //采样距离
        public float _fSampleIntensity = 2.0f; //采样强度
        public float _fSampleCenterX = 0.5f;
        public float _fSampleCenterY = 0.5f;

        public Material _matRadiusBlur = null;

        // 初始化
        void Start()
        {
            _matRadiusBlur = new Material(Shader.Find("Hidden/RadialBlur"));
        }

        //Postprocess the image
        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            _matRadiusBlur.SetFloat("_fSampleCenterX", _fSampleCenterX);
            _matRadiusBlur.SetFloat("_fSampleCenterY", _fSampleCenterY);
            _matRadiusBlur.SetFloat("_fSampleDist", _fSampleDist);
            _matRadiusBlur.SetFloat("_fSampleStrength", _fSampleIntensity);

            Graphics.Blit(source, destination, _matRadiusBlur);
        }
    }
}




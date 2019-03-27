/***************************************************************


 *
 *
 * Filename:  	ParticleScale.cs	
 * Summary: 	粒子运行时缩放
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2016/06/13 2:28
 ***************************************************************/


using UnityEngine;
using System.Collections;

namespace effect
{
    public enum ScaleType
    {
        Normal,  //普通缩放
        Time,   //随时间缩放
    }

    /// <summary>
    /// 粒子运行时缩放
    /// </summary>
    [System.Obsolete("运行时缩放有问题，暂时弃用，等修复后启用")]
    public class ParticleRuntimeScale : MonoBehaviour
    {
        #region 缩放控制变量

        /// <summary>
        /// 缩放类型
        /// </summary>
        public ScaleType scaleType = ScaleType.Normal;

        /// <summary>
        /// 开始大小
        /// </summary>
        public float startScale = 1.0f;

        /// <summary>
        /// 目标大小
        /// </summary>
        public float targetScale = 1.0f;

        /// <summary>
        /// 持续时间
        /// </summary>
        public float duration = 1.0f;

        #endregion //缩放控制变量

        ////////////////////////////////////////////////////////////////////////////////////////////////

        //粒子组件
        ParticleSystem[] _particles = null;

        //缩放持续时间
        float _fCompleteTime = 0.0f;

        ////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 粒子缩放
        /// </summary>
        void normalScale(float fScale)
        {
            for (int i = 0; i < _particles.Length; i++)
            {
                _particles[i].startSize *= fScale;
                _particles[i].startSpeed *= fScale;
                _particles[i].startRotation *= fScale;
                _particles[i].transform.localScale *= fScale;
            }

            gameObject.transform.localScale = new Vector3(fScale, fScale, 1.0f);
        }


        void timeScale(float fScale)
        {
            for (int i = 0; i < _particles.Length; i++)
            {
                _particles[i].transform.localScale = new Vector3(fScale, fScale, fScale);
            }

            gameObject.transform.localScale = new Vector3(fScale, fScale, 1.0f);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////

        // Use this for initialization
        void Start()
        {
            _particles = gameObject.GetComponentsInChildren<ParticleSystem>();

            if (scaleType == ScaleType.Normal)
                normalScale(targetScale);
        }

        // Update is called once per frame
        void Update()
        {
            if ((scaleType != ScaleType.Time))
                return;

            float fScale = Mathf.Lerp(startScale, targetScale, _fCompleteTime);
            timeScale(fScale);

            _fCompleteTime += Time.deltaTime;
        }

        void OnDestroy()
        {
            _particles = null;
        }
    }
}



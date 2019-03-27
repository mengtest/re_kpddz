/***************************************************************


 *
 *
 * Filename:  	BGM.cs	
 * Summary: 	背景音乐控制
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/10/27 20:13
 ***************************************************************/

#region Using
using asset;
using task;
using UnityEngine;
#endregion //Using


namespace sound
{
    public class BGM : MonoBehaviour
    {
        //BGM路径
        public string strAudioClipName = "";

        //播放开关
        public bool _bPlay = true;

        //音源
        AudioSource _csAudio = null;

        //音量
        public float _fVolume = 1.0f;

        /////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        void playBGM()
        {
            if (gameObject.activeSelf && _csAudio != null)
            {
                _csAudio.Play();
            }
        }

        /////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 开关事件
        /// </summary>
        /// <param name="bEnable"></param>
        void onBGMSwitchEventHandle(bool bEnable)
        {
            _bPlay = bEnable;
            if (_csAudio != null)
            {
                if (bEnable)
                    _csAudio.Play();
                else
                    _csAudio.Stop();
            }
        }

        /////////////////////////////////////////////////////////////////////////

        #region MONO

        //加载
        void Awake()
        {
            //监听事件
            //uiBattlePauseController.onBGMSwitchEvent += onBGMSwitchEventHandle;
        }

        /////////////////////////////////////////////////////////////////////////

        // 初始化
        void Start()
        {
            //_bPlay = uiBattlePauseController.BgMusicState;
            StartPlay();
        }

        public void StartPlay()
        {
            _csAudio = GetComponent<AudioSource>();
            if (_csAudio == null)
            {
                _csAudio = gameObject.AddComponent<AudioSource>();
                _csAudio.loop = true;
                _csAudio.playOnAwake = false;
                _csAudio.volume = _fVolume;
            }

            if (strAudioClipName.Equals(""))
            {
                return;
            }
            string checkedPath = CheckSoundPath(strAudioClipName);
            if (!strAudioClipName.Equals("Sound/BGM/login") && AssetManager.getInstance().IsStreamingAssets(checkedPath))
            {
                AssetManager.getInstance().loadAssetAsync(checkedPath,
                (bool manual, TaskBase currentTask) =>
                {
                    AssetManager.getInstance().minusAssetbundleRefCount(checkedPath);
                    _csAudio.clip = (AudioClip)AssetManager.getInstance().getAsset(checkedPath);
                    if (_bPlay)
                        Invoke("playBGM", 1.0f);
                });
            }
            else
            {
                //设置背景音乐
                _csAudio.clip = Resources.Load<AudioClip>(strAudioClipName);
                if (_bPlay)
                    Invoke("playBGM", 1.0f);
            }
        }

        /////////////////////////////////////////////////////////////////////////

        //启用
        void OnEnable()
        {
            if (_csAudio != null && _bPlay)
            {
                Invoke("playBGM", 1.0f);
            }
        }

        /////////////////////////////////////////////////////////////////////////

        //禁用
        public void Stop()
        {
            if (_csAudio != null)
            {
                _csAudio.Stop();
            }
        }

        //禁用
        void OnDisable()
        {
            if (_csAudio != null)
            {
                _csAudio.Stop();
            }
        }

        /////////////////////////////////////////////////////////////////////////

        //销毁
        void OnDestroy()
        {
            if (_csAudio != null)
            {
                _csAudio.Stop();
            }

            //取消监听
            //uiBattlePauseController.onBGMSwitchEvent -= onBGMSwitchEventHandle;
        }


        string CheckSoundPath(string path)
        {
            if (path == null || path.Length == 0)
                return path;

            if (path.Length <= 4)
                return string.Format("{0}.mp3", path);

            if (path.Substring(path.Length - 4).Equals(".mp3"))
                return path;
            else
                return string.Format("{0}.mp3", path);
        }
        #endregion //MONO
    }
}


/***************************************************************


 *
 *
 * Filename:  	SoundEffect.cs	
 * Summary: 	播放音效
 *
 * Version:    	1.0.0
 * Author: 	    XMG
 * Date:   	    2015/10/30 18:03
 ***************************************************************/

#region Using
using asset;
using task;
using UnityEngine;
#endregion //Using


namespace sound
{
    public class SoundEffect : MonoBehaviour
    {
        //音源

        AudioSource _csAudio = null;
        bool _bLoop = false;
        float _fPerTime = 0f;
        bool _bStop = false;

        //延迟播放
        float _fDelay = 0.0f;

        //可选标识
        int _optionTag = 0;
        float _volume = 1f;
        /////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 延迟
        /// </summary>
        public float Delay
        {
            get { return _fDelay; }
            set { _fDelay = value; }
        }

        /// <summary>
        /// 可选标识
        /// </summary>
        public int OptionTag
        {
            get { return _optionTag; }
            set { _optionTag = value; }
        }

        /// <summary>
        /// 音量
        /// </summary>
        public float Volume
        {
            get { return _volume; }
            set { _volume = value; }
        }


        /////////////////////////////////////////////////////////////////////////

        #region MONO

        //加载
        void Awake()
        {
            _csAudio = gameObject.AddComponent<AudioSource>();
            _csAudio.rolloffMode = AudioRolloffMode.Linear;
            _csAudio.playOnAwake = false;
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="strSoundName">音效文件路径</param>
        /// <param name="autoDestroyTime">n秒后，自动中断</param>
        /// <param name="bLoop">是否要循环</param>
        /// <param name="fPerTime">循环时才有作用：每次循环的间隔时间</param>
        public void Play(string strSoundName, float autoDestroyTime = 0f, bool bLoop = false, float fPerTime = 1f)
        {
            if (!strSoundName.Equals(""))
            {
                string checkedPath = CheckSoundPath(strSoundName);
                if (AssetManager.getInstance().IsStreamingAssets(checkedPath))
                {
                    AssetManager.getInstance().loadAssetAsync(checkedPath,
                    (bool manual, TaskBase currentTask) =>
                    {
                        AssetManager.getInstance().minusAssetbundleRefCount(checkedPath);
                        AudioClip clip = (AudioClip)AssetManager.getInstance().getAsset(checkedPath);
                        if (_csAudio == null)
                            Awake();
                        SoundLoadCallback(clip, autoDestroyTime, bLoop, fPerTime);
                    });

                }
                else
                {
                    //设置背景音乐
                    AudioClip clip = Resources.Load<AudioClip>(strSoundName);

                    if (_csAudio == null)
                        Awake();
                    SoundLoadCallback(clip, autoDestroyTime, bLoop, fPerTime);
                }


            }
        }

        void SoundLoadCallback(AudioClip clip, float autoDestroyTime, bool bLoop, float fPerTime)
        {
            if (clip != null)
            {
                _csAudio.loop = false;//此处用true会有BUG,有时不会循环
                _csAudio.clip = clip;
                _csAudio.volume = Volume;
                if (_fDelay > 0.0f)
                    _csAudio.PlayDelayed(_fDelay);
                else
                    _csAudio.Play();


                _bLoop = bLoop;
                _fPerTime = fPerTime;

                //循环
                if (_bLoop)
                {
                    Invoke("AutoLoop", _fPerTime);
                }
                else
                {
                    //非循环则自动销毁
                    float fAutoDestroyTime = clip.length;
                    if (autoDestroyTime > 0) fAutoDestroyTime = autoDestroyTime;
                    Invoke("Stop", fAutoDestroyTime);
                }
            }
            else
            {
                //音效加载失败，直接销毁
                Stop();
            }

        }

        public void AutoLoop()
        {
            if (_bStop)
                return;

            _csAudio.Play();
            Invoke("AutoLoop", _fPerTime);
        }

        //中断
        public void Stop()
        {
            if (_csAudio != null)
            {
                _csAudio.Stop();
                Destroy(_csAudio);
                _csAudio = null;
                _bStop = true;
                Destroy(this);
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


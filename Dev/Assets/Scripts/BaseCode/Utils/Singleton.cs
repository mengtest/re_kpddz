/***************************************************************


 *
 *
 * Filename:  	Singleton.cs	
 * Summary: 	提供两种单例模板：非线程安全和线程安全
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/03/19 16:34
 ***************************************************************/

//线程安全控制
#region defines
#define SINGLETON_THREAD_SAFE
#endregion

#region using
using System;
using UnityEngine;
#endregion



namespace Utils
{
    
    /// <summary>
    /// 单例模板
    /// </summary>
    public class Singleton<T>: MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance = default(T); //实例对象

#if SINGLETON_THREAD_SAFE
        private static object _lock = new object(); //锁
#endif


        /////////////////////////////////////////////////////////////////////


        /// <summary>
        /// 获取单例对象
        /// </summary>
        /// <returns>对象实例</returns>
        public static T getInstance()
        {
            if (_instance == null)
            {

#if SINGLETON_THREAD_SAFE
                lock (_lock) 
                {
                    createInstance();
                }
#else
                createInstance();
#endif
            }

            return _instance;
        }


        /// <summary>
        /// 创建单例对象
        /// </summary>
        private static void createInstance()
        {
            if (_instance == null)
            {
                //_instance = new T();
                _instance = FindObjectOfType(typeof(T)) as T;

                if (_instance == null)
                {
                    GameObject pSingletonObj = GameObject.Find("Singleton");
                    if (pSingletonObj == null)
                    {
                        Utils.LogSys.LogError("There is no Singleton object in the scene");
                    }
                    else
                    {
                        _instance = pSingletonObj.AddComponent(typeof(T)) as T;
                    }
                }

            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}
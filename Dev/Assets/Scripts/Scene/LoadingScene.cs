/***************************************************************


 *
 *
 * Filename:  	LoadingScene.cs	
 * Summary: 	加载场景控制
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/03/21 3:09
 ***************************************************************/

#region Using
using UnityEngine;
using System.Collections;
using UI.Controller;
using Utils;
using asset;
#endregion

namespace Scene

{
    //加载类型
    public enum ELoadingType
    {
        eLogin2Main,
    }


    public class LoadingScene : BaseScene
    {
        //目标场景
        public static string TargetScene = SceneName.s_MainScene;

        public static ELoadingType _eType;

        /// <summary>
        /// 切换到目标场景
        /// </summary>
        public void changeToTargetScene()
        {
            if (!string.IsNullOrEmpty(TargetScene))
                GameSceneManager.getInstance().ChangeToSceneImpl(TargetScene);
        }

        void Awake() {
            //UIManager.CreateWin(UIName.LOADING_WIN);
            AssetManager.getInstance().regularlyClearAssets();
        }

        // 初始化
        override public void Start()
        {
            base.Start();
            System.GC.Collect();
            Invoke("changeToTargetScene", 0.5f);
        }

        // Update 每帧调用一次
        override public void Update()
        {
            base.Update();
        }
    }
}



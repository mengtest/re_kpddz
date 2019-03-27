/***************************************************************


 *
 *
 * Filename:  	DelegateType.cs	
 * Summary: 	委托类型定义
 *
 * Version:   	1.0.0
 * Author: 		XMG
 * Date:   		2015/03/17 19:22
 ***************************************************************/

using Scene;
namespace EventManager
{

    [SLua.CustomLuaClass]
    public class DelegateType
    {
        public delegate void ProtoCallback(object proto);                           //消息回调
        public delegate void EventCallback(EventMultiArgs args);                    //事件回调
        public delegate void UIEventCallback(EventMultiArgs args);                  //事件回调
        public delegate void OperationVerify();                                     //请求确认协议回调
        public delegate void LoginCompleteEvent(uint result, byte[] reason);
        public delegate void MessageDialogCallback();                               //提示弹窗按钮事件
        public delegate void MessageDialogUseMoneyCallBack();
        public delegate void CameraAnimationCallback(string pathName);              //镜头动画回调
        public delegate void MovePathCallback(float value);                         //轨迹运动回调
        public delegate void AStarPathCallback();                                   //A*运动回调
        public delegate void FadeInBlackCallback(string strSceneName, SceneConfig config);//全黑时的回调
        public delegate void FadeInLoadingCallback();                               //加载界面显示完全时的回调
        public delegate void ChangeSceneCallback(string strSceneName);              //切场景时的回调
    }
}

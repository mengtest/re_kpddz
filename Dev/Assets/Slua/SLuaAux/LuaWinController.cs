/***************************************************************


 *
 *
 * Filename:  	LuaWinController.cs	
 * Summary: 	Lua界面控制器
 *
 * Version:   	1.0.0
 * Author: 		chr
 * Date:   		2016-11-10 15:52:15
 ***************************************************************/

using EventManager;
using System.Collections;
using System.Text;
using UI.Controller;
using UnityEngine;
using sluaAux;


public class LuaWinController : ControllerBase
{
    private luaMonoBehaviour monoComponent;

    public LuaWinController(string uiName)
    {
        sName = uiName;
        ELevel = luaSvrManager.getInstance().GetLuaWinLevel(uiName);
        prefabsPath = new string[] { luaSvrManager.getInstance().GetLuaWinPrefabPath(uiName)};
    }

    /// <summary>
    /// 界面加载完成后的回调
    protected override void UICreateCallback()
    {
        UIManager.CallLuaWinOnCreateFunc(sName, winObject);
    }

    /// <summary>
    /// 销毁后的处理
    /// </summary>
    protected override void UIDestroyCallback()
    {
        UIManager.CallLuaWinOnDestoryFunc(sName, winObject);
    }


    public override void ChangeWindowRenderQueue()
    {
        UIManager.CallLuaWinRenderFunc(sName, winObject);
    }

    public void GoBack()
    {
        UIManager.DestroyWin(sName);
    }
}

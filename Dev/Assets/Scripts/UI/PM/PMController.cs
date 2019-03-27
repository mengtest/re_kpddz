/***************************************************************


 *
 *
 * Filename:  	BackgroundBlackController.cs	
 * Summary: 	纯粹的黑色背景
 *
 * Version:    	1.0.0
 * Author: 	    XMG
 * Date:   	    2015/04/15 16:24
 ***************************************************************/

using UnityEngine;
using System.Collections;
using UI.Controller;
using EventManager;

public class PMController : ControllerBase {

    private PMMono monoComponent;

    public PMController(string uiName)
	{
		sName = uiName;
		ELevel = UILevel.TOP;
		prefabsPath = new string[] { UIPrefabPath.PM_WIN };
	}

    /// <summary>
    /// 界面加载完成后的回调
    /// </summary>
    protected override void UICreateCallback()
    {
        monoComponent = winObject.AddComponent<PMMono>();
    }

    /// <summary>
    /// 销毁后的处理
    /// </summary>
    protected override void UIDestroyCallback()
    {

        if (monoComponent != null)
        {
            UnityEngine.Object.DestroyImmediate(monoComponent);
            monoComponent = null;
        }
    }

}

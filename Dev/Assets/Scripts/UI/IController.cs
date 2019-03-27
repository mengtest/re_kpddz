/***************************************************************


 *
 *
 * Filename:  	IController.cs	
 * Summary: 	controller接口类：创建和销毁UI
 *
 * Version:   	1.0.0
 * Author: 		XMG
 * Date:   		2015/03/12 19:22
 ***************************************************************/

namespace UI.Controller
{
    public interface IController
	{
        bool CreateWin(int iDepth, bool byAction);
        bool DestroyWin(bool byAction);
	}
}

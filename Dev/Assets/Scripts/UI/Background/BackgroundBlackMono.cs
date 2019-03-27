/***************************************************************


 *
 *
 * Filename:  	BackgroundBlackMono.cs	
 * Summary: 	纯粹的黑色背景
 *
 * Version:    	1.0.0
 * Author: 	    XMG
 * Date:   	    2015/04/15 16:24
 ***************************************************************/

using UnityEngine;
using System.Collections;
using UI.Controller;

public class BackgroundBlackMono : MonoBehaviour
{
    private BackgroundBlackController controller;
    void Awake()
    {
        controller = (BackgroundBlackController)UIManager.GetControler(UIName.BACKGROUND_BLACK);
    }
}

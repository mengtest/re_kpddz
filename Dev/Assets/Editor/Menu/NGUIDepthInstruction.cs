/***************************************************************


 *
 *
 * Filename:  	PackageSourceVersion.cs	
 * Summary: 	资源版本打包工具
 *
 * Version:    	1.0.0
 * Author: 	    XMG
 * Date:   	    2015/07/11 15:33
 ***************************************************************/

using UnityEngine;
using UnityEditor;





public class NGUIDepthInstruction : EditorWindow
{
    public static void ShowDialog()
    {
        //创建窗口
        Rect wr = new Rect(0, 0, 500, 220);
        NGUIDepthInstruction window = (NGUIDepthInstruction)EditorWindow.GetWindowWithRect(typeof(NGUIDepthInstruction), wr, true, "Depth分段说明");
        window.Show();
    }

    //绘制窗口时调用
    void OnGUI()
    {

        GUILayout.Space(20f);
        GUIStyle style = new GUIStyle();
        style.fontSize = 17;
        GUILayout.Label("0~99:          common_1(公用底板)", style, GUILayout.Height(28f));
        GUILayout.Label("100~199:   当前界面图集",  style, GUILayout.Height(28f));
        GUILayout.Label("200~299:   common_2(装备底框和技能底框等)", style, GUILayout.Height(28f));
        GUILayout.Label("300~399:   英雄头像和物品等图标", style, GUILayout.Height(28f));
        GUILayout.Label("400~499:   common_3(英雄品质框等)", style, GUILayout.Height(28f));
        GUILayout.Label("500~599:   其他(图字、UITexture、序列帧、特殊图集等)", style, GUILayout.Height(28f));
        GUILayout.Label("900~999:   UILabel", style, GUILayout.Height(28f));
        GUILayout.Space(6f);
        GUILayout.BeginHorizontal();

    }
    void OnInspectorUpdate()
    {
        //Debug.Log("窗口面板的更新");
        //这里开启窗口的重绘，不然窗口信息不会刷新
        this.Repaint();
    }

    void OnSelectionChange()
    {
        //当窗口出去开启状态，并且在Hierarchy视图中选择某游戏对象时调用
        foreach (Transform t in Selection.transforms)
        {
            //有可能是多选，这里开启一个循环打印选中游戏对象的名称
            Debug.Log("OnSelectionChange" + t.name);
        }
    }

    void OnDestroy()
    {
        Debug.Log("当窗口关闭时调用");
    }

}

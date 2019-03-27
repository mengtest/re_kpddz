using UnityEngine;
using System.Collections;
using UnityEditor;
using player;

//启动 Unity的时候自动调用该脚本的静态构造函数
[InitializeOnLoad]

public class DrawGameObjectHierarchyIcon
{
    //触发器图标
    static Texture2D _txt2DTrigger = null;

    //英雄位置图标
    static Texture2D _txt2DHeroPoint = null;

    //怪物位置图标
    static Texture2D _txt2DMonsterPoint = null;

    //绘制回调
    EditorApplication.HierarchyWindowItemCallback hiearchyItemCallback = null;

    ////////////////////////////////////////////////////////////////

    static DrawGameObjectHierarchyIcon()
    {
        //加载图标
        _txt2DTrigger = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Gizmos/TriggerPoint.png"); // Resources.Load<Texture2D>("trigger");
        _txt2DHeroPoint = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Gizmos/HeroPoint.png"); //Resources.Load<Texture2D>("HeroPoint");
        _txt2DMonsterPoint = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Gizmos/MonsterPoint.png"); //Resources.Load<Texture2D>("MonsterPoint");

        //代理
        EditorApplication.hierarchyWindowItemOnGUI += drawHierarchyIcon;
    }

    //绘制图标
    static void drawHierarchyIcon(int instanceID, Rect selectionRect)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if(gameObject != null)
        {
            Rect rect = new Rect(selectionRect.x + selectionRect.width - 32f, selectionRect.y, 32f, 16f);
            //触发器
//             if (gameObject.GetComponent<PlayerObjectCreateTrigger>() != null)
//                 GUI.DrawTexture(rect, _txt2DTrigger);
//             else if (gameObject.GetComponent<MonsterEditInfo>() != null)
//                 GUI.DrawTexture(rect, _txt2DMonsterPoint);
//             else if (gameObject.GetComponent<HeroEditInfo>() != null)
//                 GUI.DrawTexture(rect, _txt2DHeroPoint);
        }
    }

}

/***************************************************************


 *
 *
 * Filename:  	CustomLuaMonoEditor.cs	
 * Summary: 	对绑定Lua脚本的对象绘制特定标记
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2016/10/17 11:00
 ***************************************************************/

using UnityEngine;
using System.Collections;
using UnityEditor;
using sluaAux;

namespace sluaAuxEditor
{
    //启动 Unity的时候自动调用该脚本的静态构造函数
    [InitializeOnLoad]
    public class DrawGameObjectHierarchyIcon
    {
        //Lua对象已绑定脚本图标
        static Texture2D _txt2DLuaGameObjNormal = null;

        //Lua对象未绑定脚本图标
        static Texture2D _txt2DLuaGameObjNull = null;

        //绘制回调
        EditorApplication.HierarchyWindowItemCallback hiearchyItemCallback = null;

        ///////////////////////////////////////////////////////////////////////////////////////

        static DrawGameObjectHierarchyIcon()
        {
            //加载图标
            _txt2DLuaGameObjNormal = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Gizmos/LuaGameObjectNormal.png");
            _txt2DLuaGameObjNull = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Gizmos/LuaGameObjectNull.png");

            //响应绘制
            EditorApplication.hierarchyWindowItemOnGUI += drawHierarchyIcon;
        }

        //绘制图标
        static void drawHierarchyIcon(int instanceID, Rect selectionRect)
        {
            GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (gameObject == null)
                return;

            var luaMono = gameObject.GetComponent<luaMonoBehaviour>();
            if (luaMono != null)
            {
                Rect rect = new Rect(selectionRect.x + selectionRect.width - 32f, selectionRect.y, 32f, 16f);
                Texture2D txt2d = _txt2DLuaGameObjNormal;
                if (string.IsNullOrEmpty(luaMono.bindScript))
                    txt2d = _txt2DLuaGameObjNull;

                GUI.DrawTexture(rect, txt2d);
            }
        }

    }
}



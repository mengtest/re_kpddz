/***************************************************************


 *
 *
 * Filename:  	StaticLabel.cs	
 * Summary: 	静态文本自动加载
 *
 * Version:   	1.0.0
 * Author: 		XB.Wu
 * Date:   		2015/04/07 20:43
 ***************************************************************/
using UnityEngine;
using Utils;

[RequireComponent(typeof(UILabel))]
[DisallowMultipleComponent]
public class StaticLabel : MonoBehaviour {
    public string key;//字符串key
    public bool FromLua = true;

    void Awake() {
        if (key.Length == 0) {
            LogSys.LogWarning("key is empty");
            return;
        }
        string str = "";
        if (FromLua == false || !sluaAux.luaSvrManager.getInstance().IsLoaded)
        {
            str = GameText.GetStr(key);
        }
        else
        {
            SLua.LuaState ls = sluaAux.luaSvrManager.getInstance().GetLuaState();
            if (ls != null)
            {
                SLua.LuaTable tb = ls.getTable("LuaText");
                if (tb[key] == null)
                {
                    str = GameText.GetStr(key);
                }
                else
                {
                    str = tb[key].ToString();
                }
                
            }
            
        }
        gameObject.GetComponent<UILabel>().text = UtilTools.Wrap(str);
    }
}

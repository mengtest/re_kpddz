/***************************************************************


 *
 *
 * Filename:  	GameDataMgr.cs	
 * Summary: 	游戏数据管理
 *
 * Version:   	1.0.0
 * Author: 		XMG
 * Date:   		2015/03/17 19:22
 ***************************************************************/

using UnityEngine;
using System.Collections;


[SLua.CustomLuaClass]
public class GameDataMgr
{
    public static LoginData LOGIN_DATA;
    public static PlayerData PLAYER_DATA;
    public static object MyPlayer { get; internal set; }

    public static void InitData()
    {
        if (LOGIN_DATA != null)
            return;

        LOGIN_DATA = new LoginData();
        PLAYER_DATA = new PlayerData();
    }

    //置空login_data, Init时就会全部新建
    public static void ClearAll()
    {
        LOGIN_DATA = null;
        PLAYER_DATA = null;
    }
    public static void ClearAllData()
    {
        LOGIN_DATA.ClearData();
//        ITEM_DATA.ClearData();
//        MAIL_DATA.ClearData();
        PLAYER_DATA.ClearData();
//        SHOP_DATA.ClearData();
    }
}

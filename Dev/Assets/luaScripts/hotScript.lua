-- local gameMgr = IMPORT_MODULE("GameMgr")
-- gameMgr.GetLoadedMgrs[2] = nil

g_logState = true
-- local lastVersion = "1.0.0"
-- local needUpdateForce = true
-- function gDelayCheck() 
--     UnityEngine.Application.Quit()
-- end
 
-- function gVersionCheck()
--     if UnityEngine.Application.version ~= lastVersion then 
--         local UnityTools = IMPORT_MODULE("UnityTools")
--         UnityTools.MessageDialog("请至FTP升级最新版本:" .. lastVersion .. "." .. g_luaVersion, {okCall = gDelayCheck, cancelCall = gDelayCheck})
--     end
-- end

-- -- 版本更新检测
-- if needUpdateForce == true and UnityEngine.Application.version ~= "1.0.0" then
--     gVersionCheck()
-- else
--     if UnityEngine.Application.version ~= lastVersion then 
--         local UnityTools = IMPORT_MODULE("UnityTools")
--         UnityTools.MessageDialog("发现最新版本:" .. lastVersion .. "." .. g_luaVersion .. ", 可前往FTP下载升级")
--     end
-- end
 
-- 充值
local PlatformMgr = IMPORT_MODULE("PlatformMgr")
PlatformMgr.rechangeCfg.VX_recharge = true
PlatformMgr.rechangeCfg.ZFB_recharge = true

-- 关闭高斯模糊
g_openGuassBlur = false

-------------
g_openAnnouncement = false
g_announcementStr = "[874C38]4月17日15:00更新预告\n1.调整红包兑换数量：5元红包次数兑换上限\n2.新手任务优化：提高新手玩家福利，大幅提升奖励\n3.每日任务优化：增加活动数量，大幅提高活动奖励。\n\n\n\n\n\n\n                                                                   就是牛游戏运营组\n                                                                      2017年4月15日[-]"
-------------


___activity_announcement_config___ = {
    -- 公告配置
        [1] = {
            title = "反作弊声明",
            content ="亲爱的玩家：\n\n    适度游戏益脑，沉迷游戏伤身，合理安排时间，享受健康生活。\n\n    本网络游戏适合年满18岁以上用户使用，为了您的健康，请合理控制游戏时间。\n\n    感谢您对《就是牛》的支持和关注，有了大家的热切支持，相信《就是牛》会做得更好，给您带来更多乐趣体验！\n\n                                           《就是牛》运营组敬上",
        },

        [2] = {
            title = "反作弊声明",
            content ="亲爱的玩家：\n\n   《就是牛》致力于打造一个公平公正、绿色健康的游戏环境。\n\n   为保证玩家的利益，《就是牛》运营团队听取各方面意见并经过多次讨论后，郑重决定通过封号等措施，全面打击非正常游戏的行为。具体措施如下：\n\n   一、非正常游戏的行为包括但不限于：\n\n   1.安装使用会影响《就是牛》游戏平衡的第三方软件；\n   2.利用游戏漏洞采用盗刷游戏币等方式，谋取游戏利益、破坏游戏的公平秩序；\n"
        }
    }
}
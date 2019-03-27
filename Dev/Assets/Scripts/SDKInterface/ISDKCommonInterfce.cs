/***************************************************************


 *
 *
 * Filename:  	ISDKCommon.cs	
 * Summary: 	SDK通用接口
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2016/04/04 22:17
 ***************************************************************/


namespace sdk
{
    /// <summary>
    /// 分享信息
    /// </summary>
    public class CommonShareInfo
    {
        public string shareUrl;
        public string title;
        public string description;
        public string iconUrl;
        public bool isToFriend;
    }
    /// <summary>
    /// 用户信息
    /// </summary>
    public class CommonUserInfo
    {
        public string rlt;
        public string uid;
        public string userName;
        public string token;
        public string timeStamp;
        public string msg;
        public string sign;

        public CommonUserInfo()
        {
            rlt = "";
            uid = "";
            userName = "";
            token = "";
            timeStamp = "";
            msg = "";
            sign = "";
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 通用订单信息
    /// </summary>
    public class CommonOrderInfo
    {
        public string   goodsID;        // string商品ID
        public string   goodsName;      //string商品名称，以“月卡”、“钻石”、“元宝”的形式传入，不带数量
        public string   goodsDesc;      //string商品描述
        public string   quantifier;     //string商品量词，比如商品为“元宝”时，传值为“个”，商品为“月卡”时传值为“张”
        public string   cpOrderID;      //string游戏订单号
        public string   callbackUrl;    // string回调地址
        public string   extrasParams;   //额外传参数
        public double   price;          //double商品单价
        public double   amount;         //double支付总额
        public int      count;          //int 商品数量
        public string   payResultOrderId; //支付结果订单

        public CommonOrderInfo()
        {
            goodsID = "";
            goodsName = "";
            goodsDesc = "";
            quantifier = "";
            cpOrderID = "";
            callbackUrl = "";
            extrasParams = "";
            payResultOrderId = "";
        }
    };

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 通用游戏角色信息
    /// </summary>
    public class CommonGameRoleInfo
    {
        public string serverName;
        public string serverID;
        public string gameRoleName;
        public string gameRoleID;
        public string gameRoleBalance;
        public string vipLevel;
        public string gameRoleLevel;
        public string partyName;
        public CommonGameRoleInfo()
        {
            serverName = "";
            serverID = "";
            gameRoleName = "";
            gameRoleID = "";
            gameRoleBalance = "";
            vipLevel = "";
            gameRoleLevel = "";
            partyName = "";
        }

    };

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 支付结果
    /// </summary>
    public class CommonPayResult
    {
        public string orderId;
        public string cpOrderId;
        public string extraParam;
        public string msg;

        public CommonPayResult()
        {
            orderId = "";
            cpOrderId = "";
            extraParam = "";
            msg = "";
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 错误信息
    /// </summary>
    public class CommonErrorMsg
    {
        public string errMsg;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 工具条位置
    /// </summary>
    public enum CustomerToolbarPlace
    {
        QUICK_SDK_TOOLBAR_TOP_LEFT = 1,           /* 左上 */
        QUICK_SDK_TOOLBAR_TOP_RIGHT = 2,           /* 右上 */
        QUICK_SDK_TOOLBAR_MID_LEFT = 3,           /* 左中 */
        QUICK_SDK_TOOLBAR_MID_RIGHT = 4,           /* 右中 */
        QUICK_SDK_TOOLBAR_BOT_LEFT = 5,           /* 左下 */
        QUICK_SDK_TOOLBAR_BOT_RIGHT = 6,           /* 右下 */
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// SDK通用接口
    /// </summary>
    public abstract class ISDKCommonInterfce
    {
        public CommonOrderInfo LastOrderInfo = null;

        /// <summary>
        /// 注销后自动弹出登录对话框
        /// </summary>
        public bool AutoOpenLoginDlgAfterLogout = false;

        //初始化
        public abstract void init();

        //登陆
        public abstract void login();

        //登出
        public abstract void logout();

        //支付信息
        public abstract void pay(CommonOrderInfo orderInfo, CommonGameRoleInfo roleInfo);
        public abstract void InitPaySdk(string appId, string partnerId);
        
        public abstract void OnRegister(string Id);
        public abstract void OnUmSdkInit(string appkey, string channel);
        public abstract void GetAvmpSign(string input, int type);

        public abstract void ShareMutilPic(int flag, string desc, string img1, string img2, string img3, string img4, string img5, string img6);
        
        //更新角色信息(不同渠道需求不同)
        public abstract void updateRoleInfo(CommonGameRoleInfo roleInfo, bool bIsCreatRole);

        //用户中心
        public virtual void enterUserCenter()
        { }

        //客服界面
        public virtual void enterCustomer()
        { }

        //进入BBS
        public virtual void enterBBS()
        { }

        //退出
        public abstract void exit();

        //是否有SDK退出框
        public virtual bool isSDKHasExitDialog()
        {
            return false;
        }

        //显示工具条
        public virtual int showToolBar(CustomerToolbarPlace place)
        {
            return 0;
        }

        //隐藏工具条
        public virtual void hideToolBar()
        { }

        //获取Userid
        public virtual string userId()
        {
            return "";
        }

        //渠道名(channel)
        public virtual string channelName()
        {
            return "";
        }

        //渠道版本
        public virtual string channelVersion()
        {
            return "";
        }

        //渠道标识
        public virtual string channelType()
        {
            return "0";
        }

        //SDK版本
        public virtual string SDKVersion()
        {
            return "1.0.0";
        }

        //获取自定义参数
        public virtual string getCustomerConfigValue(string key)
        {
            return "";
        }
        //分享
        public abstract void Share(CommonShareInfo shareInfo);
    }
}



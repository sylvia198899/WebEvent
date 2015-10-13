using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// BaseGashLogin 的摘要描述
/// </summary>
public class BaseGashLogin : System.Web.UI.Page
{
    private string str_GashAccount = string.Empty;
    private string str_GameAccount = string.Empty;
    private string str_GashRegion = string.Empty;
    private string str_UserAgent = string.Empty;
   
    protected string str_SetUserGashRegion = string.Empty;
    protected int int_LoginMode = 0;   
    protected string isDisplayTitle = ConfigurationManager.AppSettings["BeanFun_pageFlag"] ?? "0";
    protected string str_ServiceCode = ConfigurationManager.AppSettings["ServiceCode"] ?? string.Empty;
    protected string str_ServiceRegion = ConfigurationManager.AppSettings["ServiceRegion"] ?? string.Empty;
    protected string _stralphaFlag = ConfigurationManager.AppSettings["IsaphaFlag"] ?? "0";
    protected string _stralphaUserIP = ConfigurationManager.AppSettings["alphaUserIP"] ?? string.Empty;
    
    protected string _IsWebSiteStop = ConfigurationManager.AppSettings["IsWebSiteStop"] ?? "0";
    protected string _StopStartDate = ConfigurationManager.AppSettings["StopStartDate"] ?? string.Empty;
    protected string _StopEndDate = ConfigurationManager.AppSettings["StopEndDate"] ?? string.Empty;
    protected string _StopAlertMessage = ConfigurationManager.AppSettings["StopAlertMessage"] ?? "目前網站維護中!!";
    
    protected string _LoginTimeTick = ConfigurationManager.AppSettings["LoginTimeTick"] ?? "0";

    protected GashUserData CommonGashUserData;
    public BaseGashLogin()
        : base()
    {
        //
        // TODO: 在此加入建構函式的程式碼
        //
    }
    protected override void OnPreLoad(EventArgs e)
    {
        base.OnPreLoad(e);
        this.ValidGashLogin();

    }

    protected void ValidGashLogin()
    {
    	//2009/12/02 INX新增
        if (_IsWebSiteStop == "1" && SDCodeCheck.Valid.isDate(_StopStartDate) && SDCodeCheck.Valid.isDate(_StopStartDate))
        {
            if (System.DateTime.Now >= Convert.ToDateTime(_StopStartDate) && System.DateTime.Now <= Convert.ToDateTime(_StopEndDate))
            {
                Session.Clear();
                SDCodeCheck.JSUtil.AlertCloseMsg(_StopAlertMessage);
                return;             
            }
        }
        //====================
    	
        if (str_ServiceCode == string.Empty || str_ServiceRegion == string.Empty)
        {
            SDCodeCheck.JSUtil.AlertCloseMsg("系統發生異常,請聯絡客服人員!!");
            return;
        }
        string str_LoginMode = ConfigurationManager.AppSettings["LoginMode"] ?? "0";
        int _intGetLoginMode = int.Parse(str_LoginMode);

        if (int_LoginMode == 0)
        {
            if (_intGetLoginMode <= 0 || _intGetLoginMode > 4)
            {
                SDCodeCheck.JSUtil.AlertCloseMsg("登入型別,請聯絡客服人員!");
                return;
            }
            else
            {
                int_LoginMode = _intGetLoginMode;
            }

        }

        if (Context.Session != null)
        {
            string str_OTP2 = Request.Form["OTP2"] ?? string.Empty;
          
            string strUserIP=string.Empty;
            
            if (_stralphaFlag == "1")
                strUserIP = _stralphaUserIP;
            else
                strUserIP = HttpContext.Current.Request.ServerVariables["Remote_Addr"];            

            if (str_OTP2 != "")//有OTP2
            {
                string strUserAgent=Request.UserAgent ?? string.Empty;
                string strGashVersion=Request.Form["gashVersion"] ??"30";
                GASHv35LoginWS ws= new GASHv35LoginWS();
                
                WSResult _wsResult = new WSResult();
                int _intSecResult = 0;

                try
                {
                    _wsResult = ws.CreateSecretCode(str_OTP2, strUserIP, strUserAgent, strGashVersion);
                    if (_wsResult.ResultCode == 0)
                    {
                        string strSecCode = _wsResult.ResultDesc;
                        WSResult_UserData wsUserData = ws.GetUserData(str_OTP2, strSecCode);
                        if (wsUserData.ResultCode == 0)
                        {
                            _intSecResult = 1;                            
                            str_GashAccount = wsUserData.MainAccountID;
                            str_GameAccount = wsUserData.ServiceAccountID;
                            str_GashRegion = wsUserData.Region;
                            if (CommonGashUserData == null)
                            {
                                CommonGashUserData = new GashUserData();
                                this.CommonGashUserData.MainAccountID = wsUserData.MainAccountID;
                                this.CommonGashUserData.ServiceAccountID = wsUserData.ServiceAccountID;
                                this.CommonGashUserData.MainAccountSN = wsUserData.MainAccountSN;
                                this.CommonGashUserData.ServiceAccountSN = wsUserData.ServiceAccountSN;
                                this.CommonGashUserData.ServiceAccountDisplayName = wsUserData.ServiceAccountDisplayName;
                                this.CommonGashUserData.GashVersion = wsUserData.GashVersion;
                                this.CommonGashUserData.ServerIndex = wsUserData.ServerIndex;
                                this.CommonGashUserData.AuthType = wsUserData.AuthType;
                                this.CommonGashUserData.MainAccountType = wsUserData.MainAccountType;
                                this.CommonGashUserData.Region = wsUserData.Region;
                            }
                            
                        }
                        else
                        {
                            _intSecResult = -2;
                        }

                    }
                    else
                        _intSecResult = -1;
                }
                catch
                {
                    _intSecResult = -3;
                }
                ws.Dispose();

                if (_intSecResult == 1)
                {
                    Session["GashAccount"] = str_GashAccount;
                    Session["GameAccount"] = str_GameAccount;
                    Session["GashRegion"] = str_GashRegion;
                    Session["CommonGashUserData"] = CommonGashUserData;
                }
                else
                {
                    SDCodeCheck.JSUtil.AlertSussTranscation("登入驗證失敗,請重新登入!", Request.Url.AbsoluteUri.ToString());
                    return;
                }
            }
            else//沒有OTP2
            {
                //判斷是否有登入
                if ((Session["GashAccount"] == null) || (Session["GameAccount"] == null) || (Session["GashRegion"] == null) || Session["CommonGashUserData"]==null)
                {
                    string schema = ConfigurationManager.AppSettings["WEB_Protocol"] ?? "http://";
                    string str_ReturnUrl = Request.Url.AbsoluteUri.ToString();
                    if(_LoginTimeTick=="1") str_ReturnUrl=str_ReturnUrl+"?TimeTickCode=" + DateTime.Now.Ticks.ToString();
                    
                    if (schema.ToLower() == "https://")
                    {
                        str_ReturnUrl = str_ReturnUrl.Replace("http://", "https://");
                    }

                    string str_UserAgent = Request.UserAgent ?? string.Empty;
                    CreateOTP(strUserIP, str_UserAgent, str_ReturnUrl, int_LoginMode, str_ServiceCode, str_ServiceRegion, isDisplayTitle);
                }
                else
                {
                    str_GashAccount = (string)Session["GashAccount"] ?? string.Empty;
                    str_GameAccount = (string)Session["GameAccount"] ?? string.Empty;
                    str_GashRegion = (string)Session["GashRegion"] ?? string.Empty;
                    CommonGashUserData = (GashUserData)Session["CommonGashUserData"];
                }
            }

            //檢查是否有限定特定地區玩家才能參加
            if (str_GashRegion != string.Empty && str_SetUserGashRegion != string.Empty)
            {
                if (str_GashRegion.ToUpper() != str_SetUserGashRegion.ToUpper())
                {
                    Session["GashAccount"] = null;
                    Session["GameAccount"] = null;
                    Session["GashRegion"] = null;
                    SDCodeCheck.JSUtil.AlertCloseMsg("您所在的地區不符合參加本活動的資格唷!");
                    return;
                }
            }

        }
        else
        {
            SDCodeCheck.JSUtil.AlertCloseMsg("系統發生不可預期的錯誤,請稍後再試!");
            return;
        }
    }

    #region 設定共同登入所需要的參數
    /// <summary>
    /// 設定共同登入所需要的參數
    /// </summary>
    /// <param name="LoginMode">登入方式 1:用Gash帳號登入 2:用Gash帳號登入後，選擇遊戲帳號 3:用遊戲帳號登入 4:用體驗帳號登入</param>    
    protected void SetLoginData(int LoginMode)
    {
        int_LoginMode = LoginMode;
    }

    /// <summary>
    /// 設定共同登入所要使用的Service Code 以及Region Code
    /// </summary>
    /// <param name="_strServiceCode">Service Code</param>
    /// <param name="_strRegionCode">Region Code</param>
    protected void SetService(string _strServiceCode , string _strRegionCode)
    {
        str_ServiceCode = _strServiceCode;
        str_ServiceRegion = _strRegionCode;
    }

    /// <summary>
    /// 設定共同登入所需要的參數
    /// </summary>    
    /// <param name="strUserRegion">使否有限制玩家所在的國家 "TW" , "HK"</param>    
    protected void SetLoginData(string strUserRegion)
    {
        str_SetUserGashRegion = strUserRegion;
    }

    /// <summary>
    /// 設定共同登入所需要的參數
    /// </summary>
    /// <param name="LoginMode">登入方式 1:用Gash帳號登入 2:用Gash帳號登入後，選擇遊戲帳號 3:用遊戲帳號登入 4:用體驗帳號登入</param>
    /// <param name="strUserRegion">使否有限制玩家所在的國家 "TW" , "HK"</param>    
    protected void SetLoginData(int LoginMode, string strUserRegion)
    {
        int_LoginMode = LoginMode;
        str_SetUserGashRegion = strUserRegion;
    }
    #endregion

    #region 清除共同登入的Session
    /// <summary>
    /// 清除共同登入的Session
    /// </summary>
    protected void ClearCommLoginSession()
    {
        Session.Remove("GashAccount");
        Session["GashAccount"] = null;
        Session.Remove("GameAccount");
        Session["GameAccount"] = null;
        Session.Remove("GashRegion");
        Session["GashRegion"] = null;
        Session.Remove("CommonGashUserData");
        Session["CommonGashUserData"] = null;
    }
    #endregion

    #region //建立OTP,如果建立成功則導回 str_ReturnUrl
    /// <summary>
    /// 建立OTP，並將相關參數與OTP，並將Client導至Gash登入頁面
    /// </summary>
    /// <param name="str_ClientIP">Client端IP</param>
    /// <param name="str_ReturnUrl">登入成功後導回的URL</param>
    /// <param name="int_LoginMode">登入方式 1:用Gash帳號登入 2:用Gash帳號登入後，選擇遊戲帳號 3:用遊戲帳號登入 4:用體驗帳號登入</param>
    /// <param name="str_ServiceCode">遊戲的Service Code</param>
    /// <param name="str_ServiceRegion">遊戲的Region Code</param>
    /// <param name="bl_isDisplayTitle">Gash登入頁面是否顯示Gamania Title</param>
    private static void CreateOTP(string str_ClientIP, string str_UserAgent, string str_ReturnUrl, int int_LoginMode, string str_ServiceCode, string str_ServiceRegion, string str_isDisplayTitle)
    {
        string str_OTP = string.Empty;

       
        //建立OTP           

        GASHv35LoginWS ws = new GASHv35LoginWS();
        WSResult struct_Result = new WSResult();

        try
        {
            string strMergeFlow = ConfigurationManager.AppSettings["BeanFun_MergeList"] ?? "0";
            if (strMergeFlow == "1")
                struct_Result = ws.CreateOTPForMergeList(str_ClientIP, str_UserAgent, str_ReturnUrl, int_LoginMode, str_ServiceCode, str_ServiceRegion);            
            else
                struct_Result = ws.CreateOTP(str_ClientIP, str_UserAgent, str_ReturnUrl, int_LoginMode, str_ServiceCode, str_ServiceRegion);

        }
        catch
        {
            SDCodeCheck.JSUtil.AlertMsg("系統忙碌中,請稍後在試! (Error(0)");
            return;
        }
        ws.Dispose();

        if (struct_Result.ResultCode == 0)
        {
            str_OTP = struct_Result.ResultDesc;

            //第二步將相關參數與OTP傳至登入頁面
            string str_ignoreChooseFlag = ConfigurationManager.AppSettings["igoreChooseFlag"] ?? "1";
            string _isalphaFlag = ConfigurationManager.AppSettings["IsaphaFlag"] ?? "0";

            string _strLoginUrl = string.Empty;
            string strGashLoginUrl=ConfigurationManager.AppSettings["GASHLoginUrl"]?? string.Empty;
            string strAlphaGashLoginUrl = ConfigurationManager.AppSettings["alphaGASHLoginUrl"] ?? strGashLoginUrl;

            string str_isRedirectTo = ConfigurationManager.AppSettings["BeanFun_RedirectFlag"] ?? string.Empty;
            if (str_isRedirectTo.ToLower() == "v30" || str_isRedirectTo.ToLower() == "v35")
                str_isRedirectTo = "&redirectto=" + str_isRedirectTo;
            else
                str_isRedirectTo = string.Empty;

            if (strGashLoginUrl == string.Empty || strAlphaGashLoginUrl == string.Empty)
                SDCodeCheck.JSUtil.Alert("系統忙碌中,請稍候再試! Error(2)");

            if (_isalphaFlag == "1")
                _strLoginUrl = strAlphaGashLoginUrl;
            else
                _strLoginUrl = strGashLoginUrl;

            string str_Url = string.Format("{0}?ClientIP={1}&OTP={2}&pageFlag={3}&ignoreChooseFlag={4}{5}", _strLoginUrl, str_ClientIP, str_OTP, str_isDisplayTitle, str_ignoreChooseFlag, str_isRedirectTo);
            HttpContext.Current.Response.Redirect(str_Url, true);
        }
        else
        {
            SDCodeCheck.JSUtil.AlertMsg("系統忙碌中,請稍候再試! Error(1)");
            return;
        }
    }
    #endregion

    #region GashUserData Class
    [Serializable]
    public class GashUserData
    {
        private string strMainAccountID = string.Empty;
        private int intMainAccountSN = -1;
        private string strMainAccountType = string.Empty;
        private string strRegion = string.Empty;
        private int intServerIndex = -1;
        private string strServiceAccountID = string.Empty;
        private string strServiceAccountDisplayName = string.Empty;
        private int intServiceAccountSN = -1;
        private string strGashVersion = string.Empty;
        private string strAuthType = string.Empty;

        /// <summary>
        /// Gash主帳號ID
        /// </summary>
        public string MainAccountID
        {
            set { this.strMainAccountID = value; }
            get { return this.strMainAccountID; }
        }

        /// <summary>
        /// Gash主帳號ID的序號，GashV35使用，若無則回傳-1
        /// </summary>
        public int MainAccountSN
        {
            set { this.intMainAccountSN = value; }
            get { return this.intMainAccountSN; }
        }

        /// <summary>
        /// Gash主帳號ID的 Server Index，若無則回傳-1
        /// </summary>
        public int ServerIndex
        {
            set { this.intServerIndex = value; }
            get { return this.intServerIndex; }
        }

        /// <summary>
        /// 會員分類， "N": 一般會員 ; "G": 體驗帳號
        /// </summary>
        public string MainAccountType
        {
            set { this.strMainAccountType = value; }
            get { return this.strMainAccountType; }
        }

        /// <summary>
        /// 遊戲的服務地區
        /// </summary>
        public string Region
        {
            set { this.strRegion = value; }
            get { return this.strRegion; }
        }

        /// <summary>
        /// 遊戲帳號，如無則回傳空值
        /// </summary>
        public string ServiceAccountID
        {
            set { this.strServiceAccountID = value; }
            get { return this.strServiceAccountID; }
        }

        /// <summary>
        /// 遊戲帳號的暱稱 For Gashv35 , Gashv30則回傳遊戲帳號
        /// </summary>
        public string ServiceAccountDisplayName
        {
            set { this.strServiceAccountDisplayName = value; }
            get { return this.strServiceAccountDisplayName; }
        }
        /// <summary>
        /// 遊戲帳號的序號，GashV35使用
        /// </summary>
        public int ServiceAccountSN
        {
            set { this.intServiceAccountSN = value; }
            get { return this.intServiceAccountSN; }
        }

        /// <summary>
        /// Gash 的版本碼， "30" or "35"
        /// </summary>
        public string GashVersion
        {
            set { this.strGashVersion = value; }
            get { return this.strGashVersion; }
        }

        /// <summary>
        /// 登入方式，"N":一般密碼 ; "G":PlaySafe-PKI ; "F":Play-Safe -DES ; "O":OTP 
        /// </summary>
        public string AuthType
        {
            set { this.strAuthType = value; }
            get { return this.strAuthType; }
        }

        public GashUserData() { }
    }
    #endregion
}

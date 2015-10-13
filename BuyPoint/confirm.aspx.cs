using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gamania.SD.DataAccess;
using Gamania.SD.Helper;
using Gamania.SD.Library;

public partial class BuyPoint_confirm : BaseGashLogin
{
    protected GashInfo gi = new GashInfo("Please login first!");
    private Gamania.Taiwan.Helper.EventInfo ei = new Gamania.Taiwan.Helper.EventInfo("EventBeginDate", "EventEndDate");
    private string strPayServiceCode = ConfigurationManager.AppSettings["ServiceCode"] ?? "610726";
    private string strPayRegion = ConfigurationManager.AppSettings["ServiceRegion"] ?? "E3";
    private int PID = Code.GetInt(ConfigurationManager.AppSettings["PID"] ?? "436", 436);
    private string strHomeUrl = Code.GetString(ConfigurationManager.AppSettings["HomeUrl"], "http://tw.beanfun.com/GT/index.aspx");
    private string strEventName = "GT_GamePoint";

    #region "check illegal characters"
    protected void Page_Error(object sender, EventArgs e)
    {
        Exception ex = Server.GetLastError();
        if (ex is HttpRequestValidationException)
        {
            Server.ClearError();
            SDCodeCheck.JSUtil.AlertMsg("Please don't type illegal character!!");
        }
    }
    #endregion

    protected void Page_Init(object sender, EventArgs e)
    {
        ei.Check(strEventName);
    }

    private void AlertThenClose(string Msg, bool ClearSession)
    {
        Web.ClearCache();
        if (ClearSession) Web.ClearSession();
        SDCodeCheck.JSUtil.AlertCloseMsg(Msg);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (this.Session["FromPage"] == null || Code.GetString(this.Session["FromPage"], string.Empty) != "Agreement")
            {
                Session.Clear();
                SDCodeCheck.JSUtil.AlertSussTranscation("Please enter from the first page", "agreement.aspx");
            }

            Set_Radio_Button();
            if (gi.GashRegion.ToUpper() == "TW")
            {
                lbl_change_ratio.Text = "1:2"; 
                lblNotice.Text = "Currently we provide 6 kind of BeanPoint exchange to GT Gold Point, they are "50 points", "150 points", "600 points", "1500 points", "3000 points", "5000 points", and you can only exchange 1 kind everytime.";
            }
            else if (gi.GashRegion.ToUpper() == "HK")
            {
                lbl_change_ratio.Text = "10:8 (HongKong BeanPoint 1.25 = 1 GT Gold Point)";
                lblNotice.Text = "Currently we provide 7 kind of HongKong BeanPoint exchange to GT Gold Point, they are "125 points", "375 points", "750 points", "1500 points", "3750 points", "7500 points", "12500 points", and you can only exchange 1 kind everytime.";
            }
            lbl_Gash_Account.Text = gi.GashAccount;
            Gamania.SD.Helper.GashService Gashs = new Gamania.SD.Helper.GashService();
            int intUserPoint = Gashs.GetUserTotalPoint(gi.GameAccount, strPayServiceCode, strPayRegion, gi.GashRegion);
            if (intUserPoint < 0)
            {
                Gamania.SD.IRS.ErrorLog.InsErrLog(216, "confirm.aspx", "Page_Load", "GetUserTotalPoint", "Fail to get gamer's beanpoint," + intUserPoint.ToString());
                AlertThenClose("Fail to get your BeanPoint, please try again！", true);
            }
            else
            { lblUserPoint.Text = intUserPoint.ToString(); }

            string errmsg = string.Empty;
            int intTest = Code.GetInt(ConfigurationManager.AppSettings["IsTest"], 1);
            DataTable ServerDT = Gamania.GT.Common.Select_Servers(intTest, out errmsg);
            if (ServerDT != null && ServerDT.Rows.Count > 0 && string.IsNullOrEmpty(errmsg))
                Code.BindToControl(ddl_Server, ServerDT, "ServerName", "ServerID");
            else if (!string.IsNullOrEmpty(errmsg))
            {
                Gamania.SD.IRS.ErrorLog.InsErrLog(216, "confirm.aspx", "Page_Load", "Select_Servers", "Fail to get DB server list:" + errmsg);
                //AlertThenClose(errmsg, true);
                AlertThenClose("Sorry! System is busy now, please try again！(4)", true);
            }

            ddl_Server.Items.Insert(0, new ListItem("Please choose your server", "-1"));
            ddl_Character.Items.Insert(0, new ListItem("Please choose your character", "-1"));
            HFNewGuid.Value = System.Guid.NewGuid().ToString();
            Session["PointConfirm"] = HFNewGuid.Value;
        }
    }

    #region 自定方法
    /// <summary>
    /// 設定轉點選項
    /// </summary>
    private void Set_Radio_Button()
    {
        DataTable dt = get_Change_Gash_List();
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                int gashPoints = 0;
                int gamePoints = 0;
                int gameBonus = 0;
                if (!Code.IsInt(dr["GashPoints"], out gashPoints) || !Code.IsInt(dr["GamePoints"], out gamePoints) || !Code.IsInt(dr["GameBonus"], out gameBonus))
                    break;
                rb_Change_Gash.Items.Add(new ListItem(gashPoints + " BaenPoint 【can exchange" + gamePoints + "Gold】", gashPoints + "," + gamePoints + "," + gameBonus));
            }
        }
        else
            AlertThenClose("Sorry! System is busy now, please try again！(1)", true);
    }
    #endregion

    #region DB相關
    /// <summary>
    /// Get the options of Gold Point exchangeable
    /// </summary>
    /// <returns></returns>
    private DataTable get_Change_Gash_List()
    {
        string strSQL = string.Empty;
        MSSqlDataAccess mss = new MSSqlDataAccess("WebCommon");
        mss.CommandType = CommandType.Text;
        mss.CommandTimeout = 90;

        SqlParameter[] spArray = new SqlParameter[]
        { 
            IDataAccess.New<SqlParameter>("@GameRegion" , gi.GashRegion.ToUpper(), ParameterDirection.Input , DbType.String,2),
            IDataAccess.New<SqlParameter>("@PID" , PID, ParameterDirection.Input , DbType.Int16),
        };

        strSQL = @"Select GashPoints,GamePoints,GameBonus from dbo.Buy_Point_Exchange with(nolock) where PID = @PID and GameRegion = @GameRegion";

        DataTable dt = mss.ExecuteReader(strSQL, spArray);
        if (mss.HasException)
        {
            dt = null;
            Gamania.SD.IRS.ErrorLog.InsErrLog(215, "confirm.aspx", "get_Change_Gash_List", "get_Change_Gash_List", "Fail to get DB server list:" + mss.ExceptionText);
        }
        return dt;
    }
    #endregion

    #region 控制項
    protected void btn_confirm_Click(object sender, ImageClickEventArgs e)
    {
        string strNewGuid = (string)Session["PointConfirm"] ?? string.Empty;
        if (strNewGuid != HFNewGuid.Value) SDCodeCheck.JSUtil.AlertCloseMsg("Please don't open another page repetitively");

        if (ddl_Server.SelectedIndex == -1)
            SDCodeCheck.JSUtil.Alert("Please choose your server.");
        if (ddl_Character.SelectedIndex == -1)
            SDCodeCheck.JSUtil.Alert("Please choose your character.");
        if (rb_Change_Gash.SelectedItem == null)
            SDCodeCheck.JSUtil.Alert("Please choose the gold amount you want.");
        string ss = rb_Change_Gash.SelectedValue;
        string[] temp_int = ss.Split(',');
        int gashPoints = 0;
        int gamePoints = 0;
        int gameBonus = 0;
        if (temp_int.Length != 3)
            AlertThenClose("Data lost, please try again！", true);
        if (!Code.IsInt(temp_int[0], out gashPoints) || !Code.IsInt(temp_int[1], out gamePoints) || !Code.IsInt(temp_int[2], out gameBonus))
            AlertThenClose("Gold amount is wrong, please try again！", true);
        if (Code.GetInt(lblUserPoint.Text, 0) < gashPoints)
            AlertThenClose("Your bean point is not enough, please buy at specific store, or buy online at tw.beanfun.com.", true);
        Dictionary<string, string> dss = new Dictionary<string, string>();
        dss.Add("ServerID", ddl_Server.SelectedValue);                                      //伺服器代碼
        dss.Add("ServerName", ddl_Server.SelectedItem.Text);                                //伺服器名稱
        dss.Add("CharacterName", ddl_Character.SelectedItem.Text);                          //角色名稱
        dss.Add("AliasAccountID", ddl_Character.SelectedValue.Split(',')[2].ToString());    //異業帳號
        dss.Add("CharacterArea", ddl_Character.SelectedValue.Split(',')[1].ToString());     //角色所在地區
        dss.Add("GashPoints", gashPoints.ToString());                                       //兌換的GASH點數
        dss.Add("GamePoints", gamePoints.ToString());                                       //兌換的遊戲點數
        dss.Add("GameBonus", gameBonus.ToString());                                         //紅利點數
        Session["ChangePointInfo"] = dss;
        Session["FromPage"] = "confirm";
        Response.Redirect("reconfirm.aspx");
    }

    protected void ddl_Server_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblCharInfo.Text = "";
        ddl_Character.Items.Clear();
        string errmsg = string.Empty;
        string aliasAccountID = "";
        if (ddl_Server.SelectedValue != "-1")
        {
            try
            {
                DataTable CH_DT = Gamania.GT.Common.Get_Character_Info(gi.GameAccount, strPayServiceCode, str_ServiceRegion, gi.GashRegion, ddl_Server.SelectedValue, out errmsg, out aliasAccountID);
                //沒有角色編號，但是呼叫加元寶的WS需要 角色所在地(qAccountArea) & 異業帳號(AliasAccountID) 參數，因此就把值變成角色名稱(qAccount)+角色所在地(qAccountArea)+異業帳號(AliasAccountID)
                if (!string.IsNullOrEmpty(errmsg))
                {
                    Gamania.SD.IRS.ErrorLog.InsErrLog(216, "confirm.aspx", "ddl_Server_SelectedIndexChanged", "Get_Character_Info", "Fail to get the WS of character info:" + aliasAccountID + ";" + errmsg);
                    //SDCodeCheck.JSUtil.Alert(errmsg + "," + aliasAccountID);
                }
                if (CH_DT != null && CH_DT.Rows.Count >= 1)
                {
                    foreach (DataRow dr in CH_DT.Rows)
                        ddl_Character.Items.Add(new ListItem(dr["qAccount"].ToString(), dr["qAccount"].ToString() + "," + dr["qAccountArea"].ToString() + "," + aliasAccountID));
                }
                else
                { lblCharInfo.Text = "You don't have character in this server, please choose another server~!!"; }
            }
            catch (Exception ex)
            {
                Gamania.SD.IRS.ErrorLog.InsErrLog(216, "confirm.aspx", "ddl_Server_SelectedIndexChanged", "", "Connect to WS fail" + ex.Message);
                Session.Clear();
                AlertThenClose("System is busy now, please try again！", true);
            }
        }
        ddl_Character.Items.Insert(0, new ListItem("Please choose your character", "-1"));
    }
    #endregion
    protected void btn_cancel_Click(object sender, ImageClickEventArgs e)
    {
        Session.Clear();
        Response.Redirect(strHomeUrl, true);
    }
}
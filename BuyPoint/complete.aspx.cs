using System;
using System.Collections.Generic;
using System.Web;
using Gamania.SD.Library;
using System.Configuration;

public partial class BuyPoint_complete : System.Web.UI.Page
{
    private Gamania.Taiwan.Helper.EventInfo ei = new Gamania.Taiwan.Helper.EventInfo("EventBeginDate", "EventEndDate");
    private string strEventName = "GT_GamePoint";
    protected Gamania.SD.Helper.GashInfo gi = new Gamania.SD.Helper.GashInfo("Please login first");
    private string strPayServiceCode = ConfigurationManager.AppSettings["ServiceCode"] ?? "610726";
    private string strPayRegion = ConfigurationManager.AppSettings["ServiceRegion"] ?? "E3";
    private string strHomeUrl = Code.GetString(ConfigurationManager.AppSettings["HomeUrl"], "http://tw.beanfun.com/GT/index.aspx");

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
            if (this.Session["FromPage"] == null || Code.GetString(this.Session["FromPage"], string.Empty) != "reconfirm")
            {
                Session.Clear();
                SDCodeCheck.JSUtil.AlertSussTranscation("Please enter from the first page", "agreement.aspx");
            }

            if (Session["ChangePointInfo"] == null)
                AlertThenClose("Data lost, please try again！", true);
            Dictionary<string, string> dss = (Dictionary<string, string>)this.Session["ChangePointInfo"];
            if (dss.Keys.Count != 5)
                AlertThenClose("Please manipulate as the normal procedure！", true);
            int GashPoints = 0;
            int GamePoints = 0;
            int GameBonus = 0;
            //預防未知的錯誤,例如欄位名稱錯誤
            try
            {
                if (!Code.IsInt(dss["GashPoints"].ToString(), out GashPoints) || !Code.IsInt(dss["GamePoints"].ToString(), out GamePoints) || !Code.IsInt(dss["GameBonus"].ToString(), out GameBonus))
                    AlertThenClose("Sorry! System is busy now, please try again！(1)", true);
                if (string.IsNullOrEmpty(dss["ServerName"].ToString()) || string.IsNullOrEmpty(dss["CharacterName"].ToString()))
                    AlertThenClose("Sorry! System is busy now, please try again！(2)", true);
            }
            catch (Exception ee)
            {
                AlertThenClose("Sorry! System is busy now, please try again！(3)", true);
            }
            lbl_GashAccount.Text = gi.GashAccount;
            lbl_Server_Name.Text = dss["ServerName"].ToString();
            lbl_Character_Name.Text = dss["CharacterName"].ToString();
            lbl_Change_Gash_Point.Text = GashPoints.ToString();
            lbl_Change_Game_Point.Text = GamePoints.ToString();
            Gamania.SD.Helper.GashService Gashs = new Gamania.SD.Helper.GashService();
            int intUserPoint = Gashs.GetUserTotalPoint(gi.GameAccount, strPayServiceCode, strPayRegion, gi.GashRegion);
            if (intUserPoint < 0)
            {
                Gamania.SD.IRS.ErrorLog.InsErrLog(216, "complete.aspx", "Page_Load", "GetUserTotalPoint", "Fail to get gamer's BeanPoint," + intUserPoint.ToString());
                AlertThenClose("Fail to get your BeanPoint, please try again！", true); 
            }
            else
            { lbl_Now_Gash.Text = intUserPoint.ToString(); }

            HFNewGuid.Value = System.Guid.NewGuid().ToString();
            Session["PointComplete"] = HFNewGuid.Value;
        }
        string strNewGuid = (string)Session["PointComplete"] ?? string.Empty;
        if (strNewGuid != HFNewGuid.Value) SDCodeCheck.JSUtil.AlertCloseMsg("Please don't open another page repetitively.");
        //Session.Clear();
    }
    protected void btn_continue_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        Response.Redirect("agreement.aspx", true);
    }
    protected void btn_index_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        Response.Redirect(strHomeUrl, true);
    }
}
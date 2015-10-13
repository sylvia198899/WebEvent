using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using Gamania.SD.DataAccess;
using Gamania.SD.Helper;
using Gamania.SD.Library;

public partial class BuyPoint_reconfirm : System.Web.UI.Page
{
    private Gamania.Taiwan.Helper.EventInfo ei = new Gamania.Taiwan.Helper.EventInfo("EventBeginDate", "EventEndDate");
    private string strEventName = "GT_GamePoint";
    protected GashInfo gi = new GashInfo("Please login first");
    private string IsTest = ConfigurationManager.AppSettings["IsTest"];
    private string strPayServiceCode = ConfigurationManager.AppSettings["ServiceCode"] ?? "610726";
    private string strPayRegion = ConfigurationManager.AppSettings["ServiceRegion"] ?? "E3";
    private int PID = Code.GetInt(ConfigurationManager.AppSettings["PID"] ?? "436", 436);
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
            if (this.Session["FromPage"] == null || Code.GetString(this.Session["FromPage"], string.Empty) != "confirm")
            {
                Session.Clear();
                SDCodeCheck.JSUtil.AlertSussTranscation("Please enter at the first page", "agreement.aspx");
            }
            if (Session["ChangePointInfo"] == null)
                AlertThenClose("Data lost, please try again！", true);
            Dictionary<string, string> dss = (Dictionary<string, string>)this.Session["ChangePointInfo"];
            if (dss.Keys.Count != 8)
                AlertThenClose("Please manipulate as the normal procedure！", true);
            int gashPoints = 0;
            int gamePoints = 0;
            int gameBonus = 0;
            //預防未知的錯誤,例如欄位名稱錯誤
            try
            {
                if (!Code.IsInt(dss["GashPoints"].ToString(), out gashPoints) || !Code.IsInt(dss["GamePoints"].ToString(), out gamePoints) || !Code.IsInt(dss["GameBonus"].ToString(), out gameBonus))
                    AlertThenClose("Sorry! System is busy now, please try again！(1)", true);
                if (string.IsNullOrEmpty(dss["ServerID"].ToString()) || string.IsNullOrEmpty(dss["ServerName"].ToString()) || string.IsNullOrEmpty(dss["CharacterName"].ToString()) || string.IsNullOrEmpty(dss["CharacterArea"].ToString()) || string.IsNullOrEmpty(dss["AliasAccountID"].ToString()))
                    AlertThenClose("Sorry! System is busy now, please try again！(2)", true);
            }
            catch (Exception ee)
            {
                AlertThenClose("Sorry! System is busy now, please try again！(3)", true);
            }
            lbl_GashAccount.Text = gi.GashAccount;
            lbl_Server_Name.Text = dss["ServerName"].ToString();
            lbl_Character_Name.Text = dss["CharacterName"].ToString();
            lbl_Change_Gash_Point.Text = gashPoints.ToString();
            lbl_Change_Game_Point.Text = gamePoints.ToString();
            Gamania.SD.Helper.GashService Gashs = new Gamania.SD.Helper.GashService();
            int intUserPoint = Gashs.GetUserTotalPoint(gi.GameAccount, strPayServiceCode, strPayRegion, gi.GashRegion);
            if (intUserPoint < 0)
            {
                Gamania.SD.IRS.ErrorLog.InsErrLog(216, "reconfirm.aspx", "Page_Load", "GetUserTotalPoint", "Fail to get gamer's BeanPoint," + intUserPoint.ToString());
                AlertThenClose("Fail to get your BeanPoint, please try again！", true); 
            }
            else
            { lbl_Now_Gash.Text = intUserPoint.ToString(); }

            HFNewGuid.Value = System.Guid.NewGuid().ToString();
            Session["PointReconfirm"] = HFNewGuid.Value;
        }
    }

    #region 控制項
    protected void btn_confirm_Click(object sender, ImageClickEventArgs e)
    {
        string strNewGuid = (string)Session["PointReconfirm"] ?? string.Empty;
        if (strNewGuid != HFNewGuid.Value) SDCodeCheck.JSUtil.AlertCloseMsg("Please don't open another page repetitively");

        Dictionary<string, string> dss = (Dictionary<string, string>)this.Session["ChangePointInfo"];
        if (dss.Keys.Count != 8)
            AlertThenClose("Please manipulate as the normal procedure！！", true);
        int gashPoints = 0; int gamePoints = 0; int gameBonus = 0;        
        string CharacterArea = ""; string AliasAccountID = ""; string ServerName = ""; string CharacterName = ""; string ServerID = "";   
        if (!Code.IsInt(dss["GashPoints"].ToString(), out gashPoints) || !Code.IsInt(dss["GamePoints"].ToString(), out gamePoints) || !Code.IsInt(dss["GameBonus"].ToString(), out gameBonus))
            AlertThenClose("Sorry! System is busy now, please try again！(4)", true);
        if (string.IsNullOrEmpty(dss["ServerID"].ToString()) || string.IsNullOrEmpty(dss["ServerName"].ToString()) || string.IsNullOrEmpty(dss["CharacterName"].ToString()) || string.IsNullOrEmpty(dss["CharacterArea"].ToString()) || string.IsNullOrEmpty(dss["AliasAccountID"].ToString()))
            AlertThenClose("Sorry! System is busy now, please try again！(5)", true);
        ServerID = dss["ServerID"].ToString();
        CharacterArea = dss["CharacterArea"].ToString();
        AliasAccountID = dss["AliasAccountID"].ToString();
        ServerName = dss["ServerName"].ToString();
        CharacterName = dss["CharacterName"].ToString();

        //check how much is the BeanPoint have to pay
        int payGash = gashPoints;
        if (IsTest == "0")
            payGash = 1;
        else if (IsTest == "2")
            payGash = 0;
        // 1.  =====get gamer's BeanPoint=====
        Gamania.SD.Helper.GashService Gashs = new Gamania.SD.Helper.GashService();
        int ownGash = Gashs.GetUserTotalPoint(gi.GameAccount, strPayServiceCode, strPayRegion, gi.GashRegion);
        if (ownGash < 0)
        {
            Gamania.SD.IRS.ErrorLog.InsErrLog(216, "reconfirm.aspx", "Page_Load", "btn_confirm_Click", "Fail to get gamer's BeanPoint," + ownGash.ToString());
            AlertThenClose("Fail to get your BeanPoint, please try again！！", true); 
        }
        else if (ownGash < gashPoints)
        { AlertThenClose("Your BeanPoint is not enough, please buy at specific store, or buy online at tw.beanfun.com.", true); }
        else
        {
            // 2.  =====呼叫USP_GT_Insert_Log=====
            int logID = 0;

            if (!this.Insert_Log(ServerID, AliasAccountID, CharacterName, payGash, gamePoints, gameBonus, out logID))
            { AlertThenClose("Sorry! System is busy now, please try again！(6)", true); }
            else if (logID == 0)
            { AlertThenClose("Sorry! System is busy now, please try again！(9)", true); }
            else
            {
                //3.  =====deduct BeanPoint if insert log successfully=====
                Gamania.SD.Helper.GashService gashws = new Gamania.SD.Helper.GashService();
                string checkDeduct = "1";
                if (payGash != 0)//don't call WS if deduct 0 BeanPoint
                    checkDeduct = gashws.Deduct_GASHPoint_ServiceAccount(this.gi.GameAccount, this.strPayServiceCode, this.strPayRegion, this.gi.GashRegion, payGash, string.Format("君臨天下轉點:{0}", logID.ToString()), SDCodeCheck.WebComm.GetUserIP());
                if (checkDeduct != "1")
                {
                    Gamania.SD.IRS.ErrorLog.InsErrLog(216, "reconfirm.aspx", "btn_confirm_Click", "Deduct_GASHPoint", "Fail to deduct BeanPoint," + checkDeduct);
                    this.UpdateLog("Fail to deduct your BeanPoint!" + gashws.OutputMsg, logID, 2);
                    AlertThenClose("Fail to deduct your BeanPoint, please try again!", true);
                }
                else
                    this.UpdateLog("Success to deduct BeanPoint, haven't given Game Point yet!" + gashws.OutputMsg, logID, 1);

                //4.  =====call WS to give Game Point if deduct BeanPoint successfully=====
                string errMsg = string.Empty;
                int returnValue = Gamania.GT.Common.Insert_Game_Coin(AliasAccountID, this.strPayServiceCode, this.strPayRegion, this.gi.GashRegion, CharacterName, CharacterArea, logID, ServerID, payGash, gamePoints, 1, out errMsg);
                if (returnValue != 1)
                {
                    Gamania.SD.IRS.ErrorLog.InsErrLog(216, "reconfirm.aspx", "btn_confirm_Click", "Insert_Game_Coin", "Success to deduct BeanPoint, fail to give Game Point：" + returnValue + ";" + errMsg);
                    this.UpdateLog("Success to deduct BeanPoint, fail to give Game Point!" + errMsg, logID, 4);
                    AlertThenClose("Sorry! We've deducted your BeanPoint successively, but fail to give you Gold Point, please contact to our customer service！" + returnValue.ToString(), true);
                }
                this.UpdateLog("Success to give Game Point!" + errMsg, logID, 3);
                //5.  =====show message if give Gold Point successfully=====
                dss = new Dictionary<string, string>();
                dss.Add("ServerName", ServerName);               //Server Name
                dss.Add("CharacterName", CharacterName);         //Character Name
                dss.Add("GashPoints", gashPoints.ToString());    //Paid BeanPoint
                dss.Add("GamePoints", gamePoints.ToString());    //Get Gold Point
                dss.Add("GameBonus", gameBonus.ToString());      //Bonus Point
                Session["ChangePointInfo"] = dss;
                Session["FromPage"] = "reconfirm";
                Response.Redirect("complete.aspx");
            }
        }
    }
    #endregion

    #region DB
    private void UpdateLog(string outputMsg, int logID, int flag)
    {
        MSSqlDataAccess mss = new MSSqlDataAccess("GTEvent");
        mss.CommandType = CommandType.Text;

        DataTable dt = new DataTable();
        string strSQL = "UPDATE dbo.BuyPoint_Log WITH(XLOCK) SET [Flag] = @Flag,[Memo] = @Memo WHERE Seq = @ID;";

        SqlParameter[] sp = new SqlParameter[]
		{ 
            IDataAccess.New<SqlParameter>("@Flag" , flag , ParameterDirection.Input, DbType.Int16),
            IDataAccess.New<SqlParameter>("@Memo" , outputMsg, ParameterDirection.Input, DbType.String,500),
            IDataAccess.New<SqlParameter>("@ID" , logID, ParameterDirection.Input, DbType.Int32)
		};
        mss.ExecuteNonQuery(strSQL, sp);
        if (mss.HasException)
        {
            Gamania.SD.IRS.ErrorLog.InsErrLog(215, "reconfirm.aspx", "UpdateLog", "UpdateLog", "UpdateLog Fail," + mss.ExceptionText); 
            AlertThenClose("Sorry! System is busy now, please try again！(8)", true);
        }
    }

    private bool Insert_Log(string ServerID, string AliasAccountID, string CharacterID, int gashPoints, int gamePoints, int gameBonus, out int logID)
    {
        logID = 0;
        MSSqlDataAccess mss = new MSSqlDataAccess("GTEvent");
        mss.CommandType = CommandType.Text;

        DataTable dt = new DataTable();
        string strSQL = @"Insert into dbo.BuyPoint_Log ([PID],[GashID],[GameID],[AliasAccountID],[ServerID],[Character],[GashPoints]
                        ,[GamePoints],[GameBonus],[GashRegion],[Flag],[Memo],[CreateDate])
                        values (@PID, @GashID,@Game_ID,@AliasAccountID,@ServerID,@Character,@GashPoints,@GamePoints,@GameBonus,@GashRegion,0,'',getdate());
                        select scope_identity();";

        SqlParameter[] sp = new SqlParameter[]
		{ 
            IDataAccess.New<SqlParameter>("@PID" , PID, ParameterDirection.Input, DbType.Int32),
            IDataAccess.New<SqlParameter>("@GashID" , gi.GashAccount, ParameterDirection.Input, DbType.String,20),
            IDataAccess.New<SqlParameter>("@Game_ID" , gi.GameAccount, ParameterDirection.Input, DbType.String,20),
            IDataAccess.New<SqlParameter>("@AliasAccountID" , AliasAccountID, ParameterDirection.Input, DbType.String,20),
            IDataAccess.New<SqlParameter>("@GashRegion" , gi.GashRegion, ParameterDirection.Input, DbType.String,2),
            IDataAccess.New<SqlParameter>("@ServerID" , ServerID, ParameterDirection.Input, DbType.String,10),
            IDataAccess.New<SqlParameter>("@Character" , CharacterID, ParameterDirection.Input, DbType.String,12),
            IDataAccess.New<SqlParameter>("@GashPoints" , gashPoints, ParameterDirection.Input, DbType.Int16),
            IDataAccess.New<SqlParameter>("@GamePoints" , gamePoints, ParameterDirection.Input, DbType.Int16),
            IDataAccess.New<SqlParameter>("@GameBonus" , gameBonus, ParameterDirection.Input, DbType.Int16)
		};

        dt = mss.ExecuteReader(strSQL, sp);
        if (mss.HasException || dt == null || dt.Rows.Count < 1 || !Code.IsInt(dt.Rows[0][0].ToString(), out logID))
        {
            Gamania.SD.IRS.ErrorLog.InsErrLog(215, "reconfirm.aspx", "Insert_Log", "Insert_Log", "Fail to InsertLog," + mss.ExceptionText); 
            return false;
        }
        else
            return true;
    }
    #endregion
    protected void btn_cancel_Click(object sender, ImageClickEventArgs e)
    {
        Session.Clear();
        Response.Redirect(strHomeUrl, true);
    }
}
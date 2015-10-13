using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BuyPoint_agreement : System.Web.UI.Page
{
    private Gamania.Taiwan.Helper.EventInfo ei = new Gamania.Taiwan.Helper.EventInfo("EventBeginDate", "EventEndDate");
    private string strEventName = "GT_GamePoint";

    protected void Page_Init(object sender, EventArgs e)
    {
        ei.Check(strEventName);
    }

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

    protected void Page_Load(object sender, EventArgs e)
    {
        Session[strEventName] = strEventName;
    }
    protected void imgbtAgree_Click(object sender, ImageClickEventArgs e)
    {
        Session["FromPage"] = "Agreement";
        Response.Redirect("confirm.aspx", true);
    }
}
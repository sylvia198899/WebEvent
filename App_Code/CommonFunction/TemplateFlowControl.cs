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
/// TemplateFlowControl 的摘要描述
/// </summary>
public class TemplateFlowControls
{
    private bool FlagNotPublic;                         // true=只對公司內部開放, false=對外開放		
    private string StrTestIP = string.Empty;            // 允許IP OA TestIP = "210.208.83." 正式內部 TestIP="172.16.13."
    private string StrUserIP = string.Empty;
    private string StrEventStartTime = string.Empty;    //活動開始時間
    private string StrEventEndTime = string.Empty;      //活動結束時間
    private int _intChkStartTime;

    public TemplateFlowControls(string _strEventStartTime, string _strEventEndTime, string _strUserIP, bool _bFlag)
    {
        FlagNotPublic = _bFlag;
        StrUserIP = _strUserIP;
        StrTestIP = ConfigurationManager.AppSettings["TestUserIP"] ?? string.Empty;
        StrEventStartTime = _strEventStartTime;
        StrEventEndTime = _strEventEndTime;

    }

    public string ChkFlowControl()
    {
        string _strResult = string.Empty;
        int _intIPLen=0;

        if (StrTestIP == null || StrUserIP == null || StrEventStartTime == null || StrEventEndTime == null)
        {
            return "本系統尚未開放!!!";
        }

        if (StrTestIP == string.Empty || StrUserIP == string.Empty || StrEventStartTime == string.Empty || StrEventEndTime == string.Empty)
        {            
            return "本系統尚未開放!!";
        }

				_intChkStartTime = CheckEventTime(StrEventStartTime, StrEventEndTime);
				
        if (StrTestIP.Length > StrUserIP.Length)
            _intIPLen = StrUserIP.Length;
        else
            _intIPLen = StrTestIP.Length;


         if (FlagNotPublic)//對內服務
         {
         		if (StrUserIP.Substring(0, _intIPLen) != StrTestIP)
         		{
         			if (_intChkStartTime ==1)  _strResult = "活動尚未開始!!\\n開始時間：" + StrEventStartTime;
         			if (_intChkStartTime ==2) _strResult = "活動已經結束!!\\n結束時間：" + StrEventEndTime;
         		
         	  }
         }
         else//對外服務
         {
         		if (_intChkStartTime ==1)  _strResult = "活動尚未開始!!\\n開始時間：" + StrEventStartTime;
         		if (_intChkStartTime ==2) _strResult = "活動已經結束!!\\n結束時間：" + StrEventEndTime;
         	
         }
        
        return _strResult;

    }

    public int CheckEventTime(string strStartTime, string strEndTime)
    {

        //判斷活動時間開始沒--尚未開始回傳1
        if (System.DateTime.Now < Convert.ToDateTime(strStartTime)) return 1;

        //判斷活動時間是否已截止--已截止回傳2
        if (System.DateTime.Now > Convert.ToDateTime(strEndTime)) return 2;

        //驗證通過回傳0													
        return 0;
    }

    public string ChkReferrer(string SourceUrl, string ChkUrl)
    {
        string _strResult = string.Empty;
        if (SourceUrl == string.Empty || ChkUrl == string.Empty)
        {
            return "請從活動首頁進入!";
        }

        if (SourceUrl.ToLower() != ChkUrl.ToLower())
        {
            _strResult = "你以不正當方式連結網頁，請從活動首頁進入!";
        }
        return _strResult;

    }
}

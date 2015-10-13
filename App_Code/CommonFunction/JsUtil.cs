using System;
using System.Web;

/// <summary>
///  SD 公用函式
/// </summary>
namespace SDCodeCheck
{
    /// <summary>
    /// JavaScript相關的共用函式
    /// </summary>
    public class JSUtil
    {
        
        #region //產生JavaScript Alert訊息並導回前一頁
        /// <summary>
        /// 產生JavaScript Alert訊息並導回前一頁
        /// </summary>
        /// <param name="query">要顯示的訊息</param>
        public static void AlertMsg(string query)
        {
            string _strJS = "<script language=javascript>\n";
            _strJS += "alert(\"" + query + "\");\n";
            _strJS += "history.back();";
            _strJS += "</script>";
            
            HttpContext.Current.Response.Write(_strJS);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        #endregion

        #region// 產生JavaScript Alert訊息並關閉視窗
        /// <summary>
        /// 產生JavaScript Alert訊息並關閉視窗
        /// </summary>
        /// <param name="query">要顯示的訊息</param>
        public static void AlertCloseMsg(string query)
        {
            string _strJS = "<script language=javascript>\n";
            _strJS += "alert(\"" + query + "\");\n";
            _strJS += "window.opener=null;\n";
            _strJS += "window.open('','_self');\n";
            _strJS += "window.close();\n";
            _strJS += "</script>";

            HttpContext.Current.Response.Write(_strJS);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        #endregion
        	
        #region// 產生JavaScript Alert訊息並關閉視窗,可指定視窗視窗Target
        /// <summary>
        /// 產生JavaScript Alert訊息並關閉視窗,可指定視窗視窗Target
        /// </summary>
        /// <param name="query">要顯示的訊息</param>
        /// <param name="query">視窗視窗Target</param>
        public static void AlertCloseMsg(string query, string Target)
        {
            string _strJS = "<script language=javascript>\n";
            _strJS += "alert(\"" + query + "\");\n";
            if(Target !="")
            {
            	_strJS += Target+".window.close();\n";
            }
            else
            {
            	_strJS += "window.opener=null;\n";
            	_strJS += "window.open('','_self');\n";
            	_strJS += "window.close();\n";
            }
            _strJS += "</script>";

            HttpContext.Current.Response.Write(_strJS);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        #endregion

        #region// 產生JavaScript Alert訊息並將視窗導往指定的Url
        /// <summary>
        /// 產生JavaScript Alert訊息並將視窗導往指定的URL
        /// </summary>
        /// <param name="query">要顯示的訊息</param>
        /// <param name="url">要導往的Url</param>
        public static void AlertSussTranscation(string query, string url)
        {
            string _strJS = "<script language=javascript>\n";
            _strJS += "alert(\"" + query + "\");\n";
            _strJS += "location.href = \"" + url + "\";\n";
            _strJS += "</script>";

            HttpContext.Current.Response.Write(_strJS);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        #endregion

        #region// 產生JavaScript Alert訊息並將視窗導往指定的Url,可指定視窗視窗Target
        /// <summary>
        /// 產生JavaScript Alert訊息並將視窗導往指定的Url,可指定視窗視窗Target
        /// </summary>
        /// <param name="query">要顯示的訊息</param>
        /// <param name="url">要導往的Url</param>
        /// <param name="Target">視窗視窗Target</param>
        public static void AlertSussTranscation(string query, string url, string Target)
        {
            string _strJS = "<script language=javascript>\n";
            _strJS += "alert(\"" + query + "\");\n";
            if (Target.Length < 1)
                _strJS += "location.href = \"" + url + "\";\n";
            else
                _strJS += Target + ".location.href = \"" + url + "\";\n";
            _strJS += "</script>";

            HttpContext.Current.Response.Write(_strJS);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        #endregion

        #region//只產生JavaScript Alert訊息,不清除畫面也不停止網頁動作
        /// <summary>
        /// 只產生JavaScript Alert訊息,不清除畫面也不停止網頁動作
        /// </summary>
        /// <param name="query">要顯示的訊息</param>
        public static void Alert(string query)
        {
            string _strJS = "<script language=javascript>\n";
            _strJS += "alert(\"" + query + "\");";
            _strJS += "</script>";

            HttpContext.Current.Response.Write(_strJS);
            HttpContext.Current.Response.Flush();
        }
        #endregion

        #region// 產生JavaScript開新視窗(簡易)
        /// <summary>
        /// 產生JavaScript開新視窗(簡易)
        /// </summary>
        /// <param name="Str_Url">要開啟的新視窗網址</param>
        /// <param name="Str_Name">視窗名稱</param>
        public static void PopUpWindow(string Str_Url, string Str_Name)
        {
            System.Web.HttpContext.Current.Response.Write("<script Language=\"JavaScript\">\n<!--\nwindow.open('" + Str_Url + "', '" + Str_Name + "','');\n-->\n</script>");
        }
        #endregion

        #region// 利用JavaScript 開新視窗
        /// <summary>
        /// 利用JavaScript 開啟新視窗
        /// </summary>
        /// <param name="Str_Url">網址</param>
        /// <param name="Str_Name">視窗名稱</param>
        /// <param name="i_Width">視窗寬</param>
        /// <param name="i_Height">視窗高</param>
        public static void PopUpWindow(string Str_Url, string Str_Name, int i_Width, int i_Height)
        {
            string Str_feature = "";
            if (i_Width == 0 && i_Height == 0)
            { Str_feature = ""; }
            else if (i_Width != 0 && i_Height == 0)
            { Str_feature = "Width=" + i_Width.ToString(); }
            else if (i_Width == 0 && i_Height != 0)
            { Str_feature = "Width=" + i_Height.ToString(); }
            else
            { Str_feature = "Width=" + i_Width.ToString() + "," + "Height=" + i_Height.ToString(); }

            System.Web.HttpContext.Current.Response.Write("<script language=\"JavaScript\" type=\"text/JavaScript\">\n<!--\nwindow.open('" + Str_Url + "','" + Str_Name + "','" + Str_feature + "');\n-->\n</script>");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.UI;
using System.Web.Configuration;
using System.Text.RegularExpressions;

namespace Gamania.SD.Library
{
    /// <summary>
    /// 2009.05.07
    /// </summary>
    public class Code
    {
        #region Control
        /// <summary>
        /// 繫結資料到 ListControl
        /// </summary>
        /// <param name="controlObject"></param>
        /// <param name="dataTable"></param>
        /// <param name="textField"></param>
        /// <param name="valueField"></param>
        public static void BindToControl(ListControl controlObject, DataTable dataTable, string textField, string valueField)
        {
            if (controlObject == null) return;

            controlObject.DataSource = dataTable;
            controlObject.DataTextField = textField;
            controlObject.DataValueField = valueField;
            controlObject.DataBind();
        }

        /// <summary>
        /// 依Value設定ListControl的SelectIndex
        /// </summary>
        /// <param name="controlObject"></param>
        /// <param name="controlValue"></param>
        public static void SetControlSelectIndexByValue(ListControl controlObject, string controlValue)
        {
            if (controlObject == null) return;

            controlObject.SelectedIndex = controlObject.Items.IndexOf(controlObject.Items.FindByValue(controlValue));
        }

        /// <summary>
        /// 依Text設定ListControl的SelectIndex
        /// </summary>
        /// <param name="controlObject"></param>
        /// <param name="controlText"></param>
        public static void SetControlSelectIndexByText(ListControl controlObject, string controlText)
        {
            if (controlObject == null) return;

            controlObject.SelectedIndex = controlObject.Items.IndexOf(controlObject.Items.FindByText(controlText));
        }
        #endregion

        #region IsXXX
        /// <summary>
        /// 是否為Int32數值,是則轉型回傳,否則回傳0
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public static bool IsInt(object sourceObject, out Int32 returnValue)
        {
            returnValue = 0;
            if (sourceObject == null) return false;
            return (int.TryParse(sourceObject.ToString(), out returnValue));
        }

        /// <summary>
        /// 是否為有值的String,是則轉型回傳,否則傳回string.Empty
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public static bool IsString(object sourceObject, out string returnValue)
        {
            returnValue = string.Empty;

            if (sourceObject == null) return false;

            if (string.IsNullOrEmpty(sourceObject.ToString()) || sourceObject.ToString().Trim() == string.Empty)
            {
                return false;
            }
            else
            {
                returnValue = sourceObject.ToString();
                return true;
            }
        }

        /// <summary>
        /// 是否為DateTime,是則轉型回傳,否則傳回string.Empty
        /// </summary>
        /// <param name="sourceDate">The source date.</param>
        /// <param name="defaultDate">The default date.</param>
        /// <returns></returns>
        public static bool IsDateTime(string sourceDate, DateTime defaultDate)
        {
            if (string.IsNullOrEmpty(sourceDate)) return false;

            return (DateTime.TryParse(sourceDate, out defaultDate)) ? true : false;
        }
        #endregion

        #region GetXXX
        /// <summary>
        /// 依object取得Int32的值,若失敗則回傳指定的預設值
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Int32 GetInt(object sourceObject, Int32 defaultValue)
        {
            if (sourceObject == null) return defaultValue;

            int _return;

            return (int.TryParse(sourceObject.ToString(), out _return)) ? _return : defaultValue;
        }

        /// <summary>
        /// 依object取得String的值,若失敗則回傳指定的預設值
        /// </summary>
        /// <param name="sourceObject">The source object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string GetString(object sourceObject, string defaultValue)
        {
            if (sourceObject != null)
                return (string.IsNullOrEmpty(sourceObject.ToString())) ? defaultValue : sourceObject.ToString();
            else
                return defaultValue;
        }

        /// <summary>
        /// 依object取得DateTime的值,若失敗則回傳指定的預設值
        /// </summary>
        /// <param name="sourceDate">The source date.</param>
        /// <param name="defaultDate">The default date.</param>
        /// <returns></returns>
        public static DateTime GetDateTime(string sourceDate, DateTime defaultDate)
        {
            DateTime dt;
            return (DateTime.TryParse(sourceDate, out dt)) ? Convert.ToDateTime(sourceDate) : defaultDate;
        }
        #endregion

        #region GetParameter
        /// <summary>
        /// 取得參數值,如果為Null則回傳DBNull.Value
        /// </summary>
        /// <param name="sourceObject">The source object.</param>
        /// <returns></returns>
        public static object GetParameter(object sourceObject)
        {
            return (sourceObject != null) ? sourceObject : DBNull.Value;
        }

        /// <summary>
        /// 取得參數值,如果為Null / Empty,則回傳DBNull.Value
        /// </summary>
        /// <param name="sourceObject">The source object.</param>
        /// <returns></returns>
        public static object GetParameter(string sourceObject)
        {
            return (string.IsNullOrEmpty(sourceObject)) ? DBNull.Value : (object)sourceObject;
        }

        /// <summary>
        /// 取得參數值,如果為0,則回傳DBNull.Value
        /// </summary>
        /// <param name="sourceObject">The source object.</param>
        /// <returns></returns>
        public static object GetParameter(int sourceObject)
        {
            return (sourceObject == 0) ? DBNull.Value : (object)sourceObject;
        }
        #endregion

        #region Session
        /// <summary>
        /// Get Session By Name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        public static T GetSession<T>(string sessionKey)
        {
            object obj = HttpContext.Current.Session[sessionKey];

            return (obj == null) ? default(T) : (T)obj;
        }

        /// <summary>
        /// Get Cache By Name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static T GetCache<T>(string cacheKey)
        {
            object obj = HttpContext.Current.Cache[cacheKey];

            return (obj == null) ? default(T) : (T)obj;
        }
        #endregion
    }

    /// <summary>
    /// 2009.05.12
    /// </summary>
    public static class Script
    {
        public static void Output(string script)
        {
            HttpContext.Current.Response.Write(script);
            HttpContext.Current.Response.End();
        }

        public static void Output(string script, bool isFlush)
        {
            HttpContext.Current.Response.Write(script);
            if ( isFlush) HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        
        public static bool IsAjaxEnable(Page page)
        {
            if (page == null) return false;
            ScriptManager sm = ScriptManager.GetCurrent(page);
            return (sm != null);
        }

        #region JavaScript 內容
        public static string Alert(string text)
        {
            string script = string.Empty;

            script += "<script language=\"javascript\" type=\"text/javascript\"> ";
            script += string.Format(" alert('{0}');", text);
            script += " </script>";

            return script;
        }

        public static string AlertThenBack(string text)
        {
            string script = string.Empty;

            script += "<script language='javascript' type='text/javascript'> ";
            script += string.Format(" alert('{0}');", text);
            script += " history.back(); ";
            script += " </script>";

            return script;
        }

        /// <summary>
        /// 產生JavaScript Alert訊息並關閉視窗
        /// </summary>
        public static string AlertThenClose(string text)
        {
            string script = string.Empty;

            script += "<script language='javascript' type='text/javascript'> ";
            script += string.Format(" alert('{0}');", text);
            script += " if (navigator.appName == 'Microsoft Internet Explorer') {  ";
            script += " window.opener=null; window.open('','_self'); window.close(); ";
            script += " } else { window.opener=null; window.open('','_self'); window.close();document.write(''); } </script>";

            return script;
        }

        /// <summary>
        /// 產生JavaScript Alert訊息並將視窗導往指定的URL
        /// </summary>
        public static string AlertThenRedirect(string text, string url)
        {
            string script = string.Empty;

            script += "<script language='javascript' type='text/javascript'> ";
            script += string.Format(" alert('{0}');", text);
            script += string.Format(" location.href ='{0}';", url);
            script += " </script>";

            return script;
        }

        /// <summary>
        /// 產生JavaScript Alert訊息並將視窗導往指定的Url,可指定視窗視窗Target
        /// </summary>
        public static string AlertThenRedirect(string text, string url, string target)
        {
            string script = string.Empty;

            script += "<script language='javascript' type='text/javascript'> ";
            script += string.Format(" alert('{0}');", text);
            script += (target.Length < 1) ? string.Format(" location.href ='{0}';", url) : string.Format("{0}.location.href ='{1}';", target, url);
            script += " </script>";

            return script;
        }
        #endregion

        #region 決定JavaScript產生方式
        public static void Alert(Page page, string text)
        {
            string script = Script.Alert(text);

            if (IsAjaxEnable(page))
            {
                ScriptManager.RegisterClientScriptBlock(page, page.Form.GetType(), page.Form.ID, script, false);
            }
            else
            {
                if (page != null)
                    page.ClientScript.RegisterClientScriptBlock(page.Form.GetType(), page.Session.SessionID, script);
                else
                    Script.Output(Script.Alert(text));
            }
        }

        public static void AlertThenBack(Page page, string text)
        {
            string script = Script.AlertThenBack(text);

            if (IsAjaxEnable(page))
            {
                ScriptManager.RegisterClientScriptBlock(page, page.Form.GetType(), page.Form.ID, script, false);
            }
            else
            {
                if (page != null)
                    page.ClientScript.RegisterClientScriptBlock(page.Form.GetType(), page.Session.SessionID, script);
                else
                    Script.Output(Script.AlertThenBack(text));
            }
        }

        /// <summary>
        /// 產生JavaScript Alert訊息並關閉視窗
        /// </summary>
        /// <param name="text">要顯示的訊息</param>
        public static void AlertThenClose(Page page, string text)
        {
            string script = Script.AlertThenClose(text);

            if (IsAjaxEnable(page))
            {
                ScriptManager.RegisterClientScriptBlock(page, page.Form.GetType(), page.Form.ID, script, false);
            }
            else
            {
                if (page != null)
                    page.ClientScript.RegisterClientScriptBlock(page.Form.GetType(), page.Session.SessionID, script);
                else
                    Script.Output(Script.AlertThenClose(text));
            }
        }

        /// <summary>
        /// 產生JavaScript Alert訊息並將視窗導往指定的URL
        /// </summary>
        /// <param name="query">要顯示的訊息</param>
        /// <param name="url">要導往的Url</param>
        public static void AlertThenRedirect(Page page, string text, string url)
        {
            string script = Script.AlertThenRedirect(text, url);

            if (IsAjaxEnable(page))
            {
                ScriptManager.RegisterClientScriptBlock(page, page.Form.GetType(), page.Form.ID, script, false);
            }
            else
            {
                if (page != null)
                    page.ClientScript.RegisterClientScriptBlock(page.Form.GetType(), page.Session.SessionID, script);
                else
                    Script.Output(Script.AlertThenRedirect(text, url));
            }
        }

        /// <summary>
        /// 產生JavaScript Alert訊息並將視窗導往指定的Url,可指定視窗視窗Target
        /// </summary>
        /// <param name="text">要顯示的訊息</param>
        /// <param name="url">要導往的Url</param>
        /// <param name="Target">視窗視窗Target</param>
        public static void AlertThenRedirect(Page page, string text, string url, string target)
        {
            string script = Script.AlertThenRedirect(text, url, target);

            if (IsAjaxEnable(page))
            {
                ScriptManager.RegisterClientScriptBlock(page, page.Form.GetType(), page.Form.ID, script, false);
            }
            else
            {
                if (page != null)
                    page.ClientScript.RegisterClientScriptBlock(page.Form.GetType(), page.Session.SessionID, script);
                else
                    Script.Output(Script.AlertThenRedirect(text, url, target));
            }
        }
        #endregion
    }

    /// <summary>
    /// 2009.05.22
    /// </summary>
    public static class Web
    {
        /// <summary>
        /// 取得使用者的IP
        /// </summary>
        /// <returns></returns>
        public static string GetUserIP()
        {
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != "" && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            else
                return HttpContext.Current.Request.ServerVariables["Remote_Addr"];
        }

        #region Cache
        /// <summary>
        /// 清除Cache
        /// </summary>
        public static void ClearCache()
        {
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddMonths(-1);
        }
        #endregion

        #region Session
        /// <summary>
        /// 清除所有Session
        /// </summary>
        public static void ClearSession()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }

        /// <summary>
        /// 清除特定Session
        /// </summary>
        /// <param name="sessionName"></param>
        public static void ClearSession(string sessionName)
        {
            HttpContext.Current.Session[sessionName] = null;
            HttpContext.Current.Session.Remove(sessionName);
        }
        #endregion

        /// <summary>
        /// 取得目前頁面名稱
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentPageName()
        {
            string url = HttpContext.Current.Request.UrlReferrer.OriginalString;
            return url.Substring(url.LastIndexOf(@"/") + 1, url.Length - url.LastIndexOf(@"/") - 1);
        }

        /// <summary>
        /// 頁面是否來自於指定的來源頁
        /// </summary>
        /// <param name="settingName">AppSetting名稱</param>
        /// <returns></returns>
        public static bool IsPageFrom(string settingName)
        {
            if (HttpContext.Current.Request.UrlReferrer == null) return false;
            if (WebConfigurationManager.AppSettings[settingName] == null) return false;

            string url = HttpContext.Current.Request.UrlReferrer.OriginalString;
            string page = url.Substring(url.LastIndexOf(@"/") + 1, url.Length - url.LastIndexOf(@"/") - 1);

            return IsPageFrom(string.Empty, url, page, settingName);
        }

        /// <summary>
        /// 頁面是否來自於指定的來源頁
        /// </summary>
        /// <param name="settingName">AppSetting名稱</param>
        /// <param name="sessionName">記錄來源頁的Session名稱</param>
        /// <returns></returns>
        public static bool IsPageFrom(string settingName, string sessionName)
        {
            if (HttpContext.Current.Session[sessionName] == null) return false;
            if (WebConfigurationManager.AppSettings[settingName] == null) return false;

            string url = HttpContext.Current.Session[sessionName].ToString();
            string page = HttpContext.Current.Session[sessionName].ToString().Substring(url.LastIndexOf(@"/") + 1, url.Length - url.LastIndexOf(@"/") - 1);

            return IsPageFrom(sessionName, url, page, settingName);
        }

        public static bool IsPageFrom(string sessionName, string url, string page, string settingName)
        {
            string setting = WebConfigurationManager.AppSettings[settingName].ToString();

            string[] pageArray = setting.Split(',');

            for (int i = 0; i < pageArray.Length; i++)
            {
                if (page.ToLower() == pageArray[i].ToLower())
                {
                    return true;
                }
            }

            return false;
        }

        #region 檢查來源頁(一層,在AJAX下無效)
        /// <summary>
        /// 切換至指定頁
        /// </summary>
        /// <param name="targetPageName">目的頁名稱</param>
        public static void TransferPage(string targetPageName)
        {
            HttpContext.Current.Items["_sourcePage"] = HttpContext.Current.Request.FilePath.Substring(HttpContext.Current.Request.FilePath.LastIndexOf('/') + 1, HttpContext.Current.Request.FilePath.Length - HttpContext.Current.Request.FilePath.LastIndexOf('/') - 1);
            HttpContext.Current.Server.Transfer(targetPageName, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetPageName">目的頁名稱</param>
        /// <param name="dictionary">傳送至目的頁的物件</param>
        public static void TransferPage(string targetPageName, Dictionary<string, object> dictionary)
        {
            HttpContext.Current.Items["_eventValue"] = dictionary;
            HttpContext.Current.Items["_sourcePage"] = HttpContext.Current.Request.FilePath.Substring(HttpContext.Current.Request.FilePath.LastIndexOf('/') + 1, HttpContext.Current.Request.FilePath.Length - HttpContext.Current.Request.FilePath.LastIndexOf('/') - 1);
            HttpContext.Current.Server.Transfer(targetPageName, true);
        }

        /// <summary>
        /// 檢查是否來自指定頁
        /// </summary>
        /// <param name="sourcePageName">來源頁名稱</param>
        /// <returns></returns>
        public static bool IsFromPage(string sourcePageName)
        {
            if (HttpContext.Current.Items.Count == 0) return false;
            if (HttpContext.Current.Items["_sourcePage"] == null) return false;
            if (HttpContext.Current.Items["_sourcePage"].ToString() != sourcePageName) return false;

            return true;
        }

        /// <summary>
        /// 檢查是否來自指定頁
        /// </summary>
        /// <param name="sourcePageName">來源頁名稱</param>
        /// <param name="dictionary">來源頁傳送來的物件</param>
        /// <returns></returns>
        public static bool IsFromPage(string sourcePageName, out Dictionary<string, object> dictionary)
        {
            dictionary = (HttpContext.Current.Items["_eventValue"] == null) ? new Dictionary<string, object>() : (Dictionary<string, object>)HttpContext.Current.Items["_eventList"];

            if (HttpContext.Current.Items.Count == 0) return false;
            if (HttpContext.Current.Items["_sourcePage"] == null) return false;
            if (HttpContext.Current.Items["_sourcePage"].ToString() != sourcePageName) return false;

            return true;
        }
        #endregion
    }
}

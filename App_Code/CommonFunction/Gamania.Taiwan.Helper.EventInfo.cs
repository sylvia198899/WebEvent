using System;
using System.Configuration;
using System.Web;

namespace Gamania.Taiwan.Helper
{
    /// <summary>
    /// EventInfo 2010.08.31
    /// </summary>
    public class EventInfo
    {
        private string beginName = string.Empty;
        private string endName = string.Empty;
        private string beginMessage = string.Empty;
        private string endMessage = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public DateTime BeginDate
        {
            get { return this.GetAppSetting(this.beginName, "開始時間格式有誤"); }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime EndDate
        {
            get { return this.GetAppSetting(this.endName, "結束時間格式有誤");  }
        }

        /// <summary>
        /// 使用預設值建立 ( EventBeginDate / EventEndDate )
        /// </summary>
        public EventInfo()
        {
            this.beginName = "EventBeginDate";
            this.endName = "EventEndDate";

            this.beginMessage = string.Format("目前活動尚未開始({0})", this.BeginDate.ToString());
            this.endMessage = string.Format("目前活動已經結束({0})", this.EndDate.ToString());
        }

        /// <summary>
        /// 使用指定的AppSetting值建立
        /// </summary>
        /// <param name="beginName">開始日期</param>
        /// <param name="endName">結束日期</param>
        public EventInfo(string beginName, string endName)
        {
            this.beginName = beginName;
            this.endName = endName;

            this.beginMessage = string.Format("目前活動尚未開始({0})", this.BeginDate.ToString());
            this.endMessage = string.Format("目前活動已經結束({0})", this.EndDate.ToString());
        }

        /// <summary>
        /// 使用指定的AppSetting值建立
        /// </summary>
        /// <param name="beginName">開始日期</param>
        /// <param name="endName">結束日期</param>
        /// <param name="beginMessage">開始日期錯誤訊息</param>
        /// <param name="endMessage">結束日期錯誤訊息</param>
        public EventInfo(string beginName, string endName, string beginMessage, string endMessage)
        {
            this.beginName = beginName;
            this.endName = endName;

            this.beginMessage = beginMessage;
            this.endMessage = endMessage;
        }     
        
        /// <summary>
        /// 檢查活動設定
        /// </summary>
        /// <param name="eventName">活動SESSION名</param>
        public void Check(string eventName)
        {
            this.Check(eventName, this.BeginDate, this.EndDate );
        }
        
        /// <summary>
        /// 檢查活動設定(手動指定日期)
        /// </summary>
        /// <param name="eventName">活動SESSION名</param>
        /// <param name="beginDate">開始日期</param>
        /// <param name="endingDate">結束日期</param>
        protected void Check(string eventName, DateTime beginDate, DateTime endingDate)
        {
            // 是否暫停活動,若有值則顯示後關閉視窗
            string pauseText = (string)ConfigurationManager.AppSettings["PauseEventText"] ?? string.Empty;

            if (pauseText != string.Empty) AlertThenClose(pauseText);
                
            if (IsPublicSite())
            {
                // 若非指定的測試IP區段,則檢查起迄日期
                if (!IsTester())
                {
                    if (beginDate > DateTime.Now) AlertThenClose(this.beginMessage);
                    if (endingDate < DateTime.Now) AlertThenClose(this.endMessage);
                }
                else
                {
                    if (endingDate < DateTime.Now) AlertThenClose(this.endMessage);
                }
            }
            else
            {
                if (endingDate < DateTime.Now) AlertThenClose(this.endMessage);
            }

            // 檢查活動專屬 Flag 是否存在, 否則清 Session
            string sessionText = (string)HttpContext.Current.Session[eventName] ?? string.Empty;

            if (sessionText != eventName)
            {
                ClearSession();
            }
        }

        /// <summary>
        /// Alert Then Close Script
        /// </summary>
        /// <param name="text">Script</param>
        protected static void AlertThenClose(string text)
        {
            string script = string.Empty;

            script += "<script language='javascript' type='text/javascript'> ";
            script += string.Format("alert('{0}');", HttpContext.Current.Server.HtmlEncode(text));
            script += " if (navigator.appName == 'Microsoft Internet Explorer') {  ";
            script += " window.opener=null; window.open('','_self'); window.close(); ";
            script += " } else { window.opener=null; window.open('','_self'); window.close();document.write(''); } </script>";

            HttpContext.Current.Response.Write(script);
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// Alert Then Redirect Script
        /// </summary>
        /// <param name="text">Script</param>
        /// <param name="url">Redirect URL</param>
        protected static void AlertThenRedirect(string text, string url)
        {
            string script = string.Empty;

            script += "<script language='javascript' type='text/javascript'> ";
            script += string.Format(" alert('{0}');", HttpContext.Current.Server.HtmlEncode(text));
            script += string.Format(" location.href ='{0}';", url);
            script += " </script>";

            HttpContext.Current.Response.Write(script);
            HttpContext.Current.Response.End();
        }
 
        /// <summary>
        /// Get Web.Config AppSetting By DateTime
        /// </summary>
        /// <param name="settingName">Setting Name</param>
        /// <param name="errorMessage">Error Message</param>
        /// <returns></returns>
        protected DateTime GetAppSetting(string settingName, string errorMessage)
        {
            string settingValue = (string)ConfigurationManager.AppSettings[settingName] ?? string.Empty;

            if (string.IsNullOrEmpty(settingValue))
            {
                HttpContext.Current.Response.Write(errorMessage);
                HttpContext.Current.Response.End();
            }

            DateTime dt = DateTime.Now;

            if (!DateTime.TryParse(settingValue, out dt))
            {
                HttpContext.Current.Response.Write(errorMessage);
                HttpContext.Current.Response.End();
            }

            return dt;
        }

        /// <summary>
        /// Clear Gash Session
        /// </summary>
        protected static void ClearSession()
        {
            HttpContext.Current.Session.Remove("GashAccount");
            HttpContext.Current.Session["GashAccount"] = null;
            HttpContext.Current.Session.Remove("GameAccount");
            HttpContext.Current.Session["GameAccount"] = null;
            HttpContext.Current.Session.Remove("GashRegion");
            HttpContext.Current.Session["GashRegion"] = null;
            HttpContext.Current.Session.Remove("CommonGashUserData");
            HttpContext.Current.Session["CommonGashUserData"] = null;
        }

        /// <summary>
        /// 取得GASH點數值(Web.config)
        /// </summary>
        /// <param name="sectionName">Section Name</param>
        /// <returns></returns>
        public static int TestGashPoint(string sectionName)
        {
            string point = ConfigurationManager.AppSettings[sectionName] ?? string.Empty;

            if (point == string.Empty) return 1;

            return int.Parse(point);
        }

        /// <summary>
        /// 是否在上線時間內
        /// </summary>
        /// <returns></returns>
        public bool IsOnline()
        {
            if ((this.BeginDate > DateTime.Now) || ( this.EndDate < DateTime.Now)) return false;

            return true;
        }

        /// <summary>
        /// 是否為正式網址
        /// </summary>
        /// <returns></returns>
        public static bool IsPublicSite()
        {
            if (HttpContext.Current.Request.Url.DnsSafeHost.IndexOf("avtw.event.gamania.com") == -1) return true;

            return false;
        }

        /// <summary>
        /// 是否為測試者
        /// </summary>
        /// <returns></returns>
        public static bool IsTester()
        {
            if (HttpContext.Current.Request.Url.DnsSafeHost == "localhost") return true;

            string ip = GetUserIP();

            if (IsAddressMatch(ip, "210.208.83")) return true;  // TW
            if (IsAddressMatch(ip, "125.215.234")) return true;  // HK

            return false;
        }

        /// <summary>
        /// IP位址是否符合
        /// </summary>
        /// <param name="userIP">User IP Address</param>
        /// <param name="testIP">Tester IP Address</param>
        /// <returns></returns>
        protected static bool IsAddressMatch(string userIP, string testIP)
        {
            if (userIP.Trim().Length == 0) return false;
            if (testIP.Trim().Length == 0) return false;

            int length = (testIP.Length > userIP.Length) ? userIP.Length : testIP.Length;

            return (userIP.Substring(0, length) == testIP) ? true : false;
        }

        /// <summary>
        /// 取得使用者IP
        /// </summary>
        /// <returns></returns>
        protected static string GetUserIP()
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]) && !string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            else
                return HttpContext.Current.Request.ServerVariables["Remote_Addr"];
        }
    }
}
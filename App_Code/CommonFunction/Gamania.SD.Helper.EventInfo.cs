using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using Gamania.SD.Library;
using System.Web.UI;

namespace Gamania.SD.Helper
{
    /// <summary>
    /// EventInfo 2009.11.11
    /// </summary>
    public class EventInfo
    {
        public static Page Page;

        /// <summary>
        /// 檢查是否已有此活動的識別用Session 
        /// </summary>
        /// <param name="eventName"></param>
        public static void InitCheck(string eventName)
        {
            if (HttpContext.Current.Session[eventName] == null)
            {
                ClearSession();
            }
        }

        /// <summary>
        /// 設置活動識別用的Session 
        /// </summary>
        /// <param name="eventName"></param>
        public static void SetSession(string eventName)
        {
            HttpContext.Current.Session[eventName] = eventName;
        }

        /// <summary>
        /// 檢查活動設定(預設指定日期),舊方式不建議使用
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="checkEventFlag"></param>
        public static void Check(string eventName, bool checkEventFlag)
        {
            string eventStartTime = GetAppSetting("EventBeginDate", "開始時間格式有誤").ToString();
            string eventEndTime = GetAppSetting("EventEndDate", "結束時間格式有誤").ToString();
            string pauseText = (string)ConfigurationManager.AppSettings["PauseEventText"] ?? string.Empty;
            string testIP = (string)ConfigurationManager.AppSettings["TestUserIP"] ?? string.Empty;

            bool flagNotPublic = bool.Parse(ConfigurationManager.AppSettings["FlagNotPublic"].ToString());

            // 是否暫停活動,若有值則顯示後關閉視窗
            if (pauseText != string.Empty) Script.AlertThenClose(Page,pauseText);

            // 檢查活動期間
            if (flagNotPublic) 
            {
                // 若為測試環境
                // 若非指定的測試IP區段,則檢查起迄日期
                if (!IsMatchSourceIP(Web.GetUserIP(), testIP))
                {
                    if (Convert.ToDateTime(eventStartTime) > DateTime.Now) Script.AlertThenClose(Page, string.Format("目前活動尚未開始({0})", eventStartTime));
                    if (Convert.ToDateTime(eventEndTime) < DateTime.Now) Script.AlertThenClose(Page, string.Format("目前活動時間已結束({0})", eventEndTime));
                }
            }
            else
            {
                // 若為正式環境
                if (Convert.ToDateTime(eventStartTime) > DateTime.Now) Script.AlertThenClose(Page, string.Format("目前活動尚未開始({0})", eventStartTime));
                if (Convert.ToDateTime(eventEndTime) < DateTime.Now) Script.AlertThenClose(Page, string.Format("目前活動時間已結束({0})", eventEndTime));
            }

            // 檢查活動專屬 Flag 是否存在, 否則清 Session
            if (checkEventFlag)
            {
                string eventSessionText = (string)HttpContext.Current.Session[eventName] ?? string.Empty;
                if (eventSessionText != eventName)
                {
                    ClearSession();
                }
            }
        }

        /// <summary>
        /// 檢查活動設定(預設Web.config設定值)
        /// </summary>
        /// <param name="eventName"></param>
        public static void Check(string eventName)
        {
            DateTime dtBegin = EventInfo.GetAppSetting("EventBeginDate", "開始時間格式有誤");
            DateTime dtEnding = EventInfo.GetAppSetting("EventEndDate", "結束時間格式有誤");

            EventInfo.Check(eventName, dtBegin, dtEnding, string.Format("目前活動尚未開始({0})", dtBegin.ToString()), string.Format("目前活動時間已結束({0})", dtEnding.ToString()));
        }

        /// <summary>
        /// 檢查活動設定(自訂Web.config設定值)
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="beginDateName"></param>
        /// <param name="endingDateName"></param>
        public static void Check(string eventName, string beginDateName, string endingDateName)
        {
            DateTime dtBegin = EventInfo.GetAppSetting(beginDateName, "開始時間格式有誤");
            DateTime dtEnding = EventInfo.GetAppSetting(endingDateName, "結束時間格式有誤");

            EventInfo.Check(eventName, dtBegin, dtEnding, string.Format("目前活動尚未開始({0})", dtBegin.ToString()), string.Format("目前活動時間已結束({0})", dtEnding.ToString()));
        }

        /// <summary>
        /// 檢查活動設定和錯誤訊息(自訂Web.config設定值)
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="beginDateName"></param>
        /// <param name="endingDateName"></param>
        /// <param name="beginMessage"></param>
        /// <param name="endingMessage"></param>
        public static void Check(string eventName, string beginDateName, string endingDateName, string beginMessage, string endingMessage)
        {
            DateTime dtBegin = EventInfo.GetAppSetting(beginDateName, beginMessage);
            DateTime dtEnding = EventInfo.GetAppSetting(endingDateName, endingMessage);

            EventInfo.Check(eventName, dtBegin, dtEnding, beginMessage, endingMessage);
        }

        /// <summary>
        /// 檢查活動設定(手動指定日期)
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="beginDate"></param>
        /// <param name="endingDate"></param>
        public static void Check(string eventName, DateTime beginDate, DateTime endingDate)
        {
            EventInfo.Check(eventName, beginDate, endingDate, string.Format("目前活動尚未開始({0})", beginDate.ToString()), string.Format("目前活動時間已結束({0})", endingDate.ToString()));
        }

        /// <summary>
        /// 檢查活動設定(手動指定日期)
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="beginDate"></param>
        /// <param name="endingDate"></param>
        /// <param name="beginMessage"></param>
        /// <param name="endingMessage"></param>
        public static void Check(string eventName, DateTime beginDate, DateTime endingDate, string beginMessage, string endingMessage)
        {
            string pauseText = (string)ConfigurationManager.AppSettings["PauseEventText"] ?? string.Empty;
            string testIP = (string)ConfigurationManager.AppSettings["TestUserIP"] ?? string.Empty;

            bool flagNotPublic = bool.Parse(ConfigurationManager.AppSettings["FlagNotPublic"].ToString());
            
            // 是否暫停活動,若有值則顯示後關閉視窗
            if (pauseText != string.Empty) Script.AlertThenClose(Page,pauseText);
                
            // 檢查活動期間
            if (flagNotPublic)
            {
                // 若為測試環境
                // 若非指定的測試IP區段,則檢查起迄日期
                if (!IsMatchSourceIP(Web.GetUserIP(), testIP))
                {
                    if (beginDate > DateTime.Now)  Script.AlertThenClose(Page,beginMessage);
                    if (endingDate < DateTime.Now)  Script.AlertThenClose(Page,endingMessage);
                }
            }
            else
            {
                // 若為正式環境
                if (beginDate > DateTime.Now) Script.AlertThenClose(Page, beginMessage);
                if (endingDate < DateTime.Now) Script.AlertThenClose(Page, endingMessage);
            }

            // 檢查活動專屬 Flag 是否存在, 否則清 Session
            string eventSessionText = (string)HttpContext.Current.Session[eventName] ?? string.Empty;
            if (eventSessionText != eventName)
            {
                ClearSession();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userIP"></param>
        /// <param name="testIP"></param>
        /// <returns></returns>
        protected static bool IsMatchSourceIP(string userIP, string testIP)
        {
            if (userIP.Trim().Length == 0) return false;
            if (testIP.Trim().Length == 0) return false;

            int length = (testIP.Length > userIP.Length) ? userIP.Length : testIP.Length;

            return (userIP.Substring(0, length) == testIP) ? true : false;
        }
 
        /// <summary>
        /// 依日期型式取得Web.Config設定值
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        protected static DateTime GetAppSetting(string settingName, string errorMessage)
        {
            string settingValue = (string)ConfigurationManager.AppSettings[settingName] ?? string.Empty;

            if (settingValue == string.Empty)
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
    }
}
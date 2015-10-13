using System;
using System.Collections.Generic;
using System.Web;

namespace Gamania.SD.IRS
{
    /// <summary>
    /// Gamania 的摘要描述
    /// </summary>
    public class ErrorLog
    {
        #region "呼叫IRS寫入異常紀錄"
        /// <summary>
        /// 呼叫IRS寫入異常紀錄
        /// </summary>
        /// <param name="intRSVID">系統編號</param>
        /// <param name="strURL">URL</param>
        /// <param name="strFuncName">程式名稱</param>
        /// <param name="strFuncID">辨識碼</param>
        /// <param name="strErrMsg">錯誤訊息</param>
        public static void InsErrLog(int intRSVID, string strURL, string strFuncName, string strFuncID, string strErrMsg)
        {
            IRSWS IRSws = new IRSWS();
            sInsErr InsErr = new sInsErr();
            IRSws.Timeout = 90;
            try
            { InsErr = IRSws.InsErr(intRSVID, strURL, strFuncName, strFuncID, strErrMsg); }
            catch { }
            IRSws.Dispose();
        }
        #endregion
    }
}
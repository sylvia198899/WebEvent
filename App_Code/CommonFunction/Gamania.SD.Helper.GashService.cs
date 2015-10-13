using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;

namespace Gamania.SD.Helper
{

    /// <summary>
    /// GashService 的摘要描述
    /// 2009.11.23
    /// </summary>
    public class GashService
    {
        // WS output message
        public string OutputMsg = string.Empty;
        // WS output value
        public string OutputResult = string.Empty;
        public Exception WSException = null;
 
        /// <summary>
        /// 檢查是否已開啟線上交易的服務;
        /// 此功能會自動產生服務帳號密碼，因此僅限用於未來無需變更密碼的服務
        /// </summary>
        /// <param name="GashAccount"></param>
        /// <param name="ServiceCode"></param>
        /// <param name="ServiceRegion"></param>
        /// <param name="GashRegion"></param>
        /// <returns>1成功;0失敗;-1例外錯誤</returns>
        public int CheckOpenEventService(string GashAccount, string ServiceCode, string ServiceRegion, string GashRegion)
        {
            int Result = -1;
            string wsResult = string.Empty;
            string[] aryResult;

            using (ServiceAccount ws = new ServiceAccount())
            {
                ws.Url = GetGashWSUrl("ServiceAccount", GashRegion.ToUpper());
                try
                {
                    //WS
                    //intResult: (1 Success, 0 Failed, -1 Error)
                    //ex:"0;ServiceAccountID_Exists","0;DisplayName_Exists","0;Over_MaxAccount"
                    wsResult = ws.ServiceAccount_CreateSimple(GashAccount, ServiceCode, ServiceRegion, GashAccount);
                    aryResult = wsResult.Split(";".ToCharArray());
                    Result = (aryResult[0] == "0" || aryResult[0] == "1") ? 1 : 0;
                    OutputResult = aryResult[0];
                }
                catch(Exception ex)
                {
                    Result = -1;
                    WSException = ex;
                    aryResult = new string[] { "" };
                }
            }
            
            OutputMsg = (aryResult!=null && aryResult.Length > 1) ? aryResult[1] : wsResult;
            
            return Result;

        }

        /// <summary>
        /// 取得玩家擁有gash點數
        /// </summary>
        /// <param name="GashAccount"></param>
        /// <param name="GashRegion"></param>
        /// <returns>-1 失敗; else Point</returns>
        public int GetUserGashPoint(string GashAccount, string GashRegion)
        {
            int Result = -1;
            string wsResult = string.Empty;
            string[] aryResult;

            using (MainAccount ws = new MainAccount())
            {
                ws.Url = GetGashWSUrl("MainAccount", GashRegion.ToUpper());
                try
                {
                    //WS return: intResult;Outstring
                    //intResult: (1  Success, 0  Failed, -1  Error)
                    //Outstring: will be RemainingPoints when intResult is 1
                    wsResult = ws.MainAccount_GetRemainingPoints(GashAccount);
                    aryResult = wsResult.Split(";".ToCharArray());
                    if(aryResult[0] == "1") int.TryParse(aryResult[1], out Result);
                    OutputResult = aryResult[0];
                }
                catch (Exception ex)
                {
                    Result = -1;
                    WSException = ex;
                    aryResult = new string[] { "" };
                }
            }

            OutputMsg = (aryResult != null && aryResult.Length > 1) ? aryResult[1] : wsResult;

            return Result;

        }

        /// <summary>
        /// 取得玩家擁有gash點數+遊戲/服務專用點數;
        /// 需帶入專用點數所屬的線上服務代碼;
        /// 適用於已開啟此線上服務的服務帳號
        /// </summary>
        /// <param name="ServiceAccount"></param>
        /// <param name="ServiceCode"></param>
        /// <param name="ServiceRegion"></param>
        /// <param name="GashRegion"></param>
        /// <returns></returns>
        public int GetUserTotalPoint(string ServiceAccount,string ServiceCode,string ServiceRegion, string GashRegion)
        {
            int Result = 0;
            string wsResult = string.Empty;
            string[] aryResult;

            using (ServiceAccount ws = new ServiceAccount())
            {
                ws.Url = GetGashWSUrl("ServiceAccount", GashRegion.ToUpper());
                try
                {
                    //WS return: intResult;Outstring
                    //intResult: (1  Success)
                    //Outstring: will be RemainingPoints when intResult is 1
                    //未開啟此線上服務:"-1;Check_ServiceAccount_Failed"
                    wsResult = ws.ServiceAccount_GetRemainingPoints(ServiceCode, ServiceRegion, ServiceAccount);
                    aryResult = wsResult.Split(";".ToCharArray());
                    if (aryResult[0] == "1") int.TryParse(aryResult[1], out Result);
                    OutputResult = aryResult[0];
                }
                catch (Exception ex)
                {
                    Result = -1;
                    WSException = ex;
                    aryResult = new string[] { "" };
                }
            }

            OutputMsg = (aryResult != null && aryResult.Length > 1) ? aryResult[1] : wsResult;

            return Result;

        }

        /// <summary>
        /// GASH主帳號自由點數扣點;
        /// 若此服務有專用點數會先扣專用專用點數;
        /// 此功能只適用於限開單一服務帳號的服務/遊戲
        /// </summary>
        /// <param name="GashAccount"></param>
        /// <param name="GashRegion"></param>
        /// <returns>"1":成功; 	"0":失敗</returns>
        public string Deduct_GASHPoint(string GashAccount, string ServiceCode, string ServiceRegion, string GashRegion, int ExpectPont, string Memo, string IP)
        {
            string Result = "0";
            string wsResult = string.Empty;
            string[] aryResult;
            string nowDate = DateTime.Now.ToString("yyyy/MM/dd") + " " + DateTime.Now.Hour.ToString() + DateTime.Now.ToString(":mm:ss");    //避免上下午混用，轉成24H制

            using (AAA ws = new AAA())
            {
                ws.Url = GetGashWSUrl("AAA", GashRegion.ToUpper());
                try
                {
                    //WS return: intResult;Outstring
                    //intResult: (1  Success, 0  Failed, -1  Error)
                    wsResult = ws.MainAccount_AccountingFreePoint(GashAccount, ServiceCode, ServiceRegion, nowDate, ExpectPont, Memo, IP);
                    aryResult = wsResult.Split(";".ToCharArray());
                    Result = (aryResult[0] == "1") ? "1" : "0";
                    OutputResult = aryResult[0];
                }
                catch (Exception ex)
                {
                    Result = "0";
                    WSException = ex;
                    aryResult = new string[] { "" };
                }
            }

            OutputMsg = (aryResult != null && aryResult.Length > 1) ? aryResult[1] : wsResult;

            return Result;

        }

        /// <summary>
        /// 服務帳號自由點數扣點;
        /// </summary>
        /// <param name="GashAccount"></param>
        /// <param name="GashRegion"></param>
        /// <returns>"1":成功; 	"0":失敗</returns>
        public string Deduct_GASHPoint_ServiceAccount(string ServiceAccount, string ServiceCode, string ServiceRegion, string GashRegion, int ExpectPont, string Memo, string IP)
        {
            string Result = "0";
            string wsResult = string.Empty;
            string[] aryResult;
            string nowDate = DateTime.Now.ToString("yyyy/MM/dd") + " " + DateTime.Now.Hour.ToString() + DateTime.Now.ToString(":mm:ss");    //避免上下午混用，轉成24H制

            using (AAA ws = new AAA())
            {
                ws.Url = GetGashWSUrl("AAA", GashRegion.ToUpper());
                try
                {
                    //WS return: intResult;Outstring
                    //intResult: (1  Success, 0  Failed, -1  Error)
                    wsResult = ws.ServiceAccount_AccountingFreePoint(ServiceCode, ServiceRegion,ServiceAccount, nowDate, ExpectPont, Memo, IP);
                    aryResult = wsResult.Split(";".ToCharArray());
                    Result = (aryResult[0] == "1") ? "1" : "0";
                    OutputResult = aryResult[0];
                }
                catch (Exception ex)
                {
                    Result = "0";
                    WSException = ex;
                    aryResult = new string[] { "" };
                }
            }

            OutputMsg = (aryResult != null && aryResult.Length > 1) ? aryResult[1] : wsResult;

            return Result;

        }
        /// <summary>
        /// Nothing
        /// </summary>
        public GashService()
        {
        }

        public static string GetGashWSUrl(string GashWS, string GashRegion)
        {
            if (GashRegion == "TW")
                return (string)ConfigurationManager.AppSettings["GASHv30FWS_" + GashWS] ?? "";
            else if (GashRegion == "HK")
                return (string)ConfigurationManager.AppSettings["GASHv30FWS_" + GashWS + "_HK"] ?? "";
            else
                return "";

        }
    }
}

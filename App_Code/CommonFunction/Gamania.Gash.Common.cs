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
/// Gamania 的摘要描述
/// </summary>
namespace Gamania.Gash
{
    /// <summary>
    /// 呼叫Gash WebService的共用函式
    /// By Kodofish
    /// </summary>
    public class Common
    {
        public Common()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }


        /// <summary>
        /// 注意：2007/8/27以後改使用MainAccount_Authentication_New
        /// 呼叫Gash Web Service 進行 GASH主帳號認證
        /// </summary>
        /// <param name="Str_GashAccount">Gash帳號</param>
        /// <param name="Str_GashPassWord">Gash密碼</param>
        /// <returns>(1 = Success, 0 = Failed, -1 = Error, -2;ErrorMessage = 呼叫Web Service發生錯誤,ErrorMessage為錯誤訊息)</returns>
        public static string MainAccount_Authentication(string Str_GashAccount, string Str_GashPassWord)
        {
            string Str_Result="";
            MainAccount ws = new MainAccount();
            
            try
            {
                Str_Result = ws.MainAccount_Authentication(Str_GashAccount, Str_GashPassWord);
            }
            catch (Exception ex)
            {
                Str_Result = "-2;" +ex.Message;
            }

            ws.Dispose();
            return Str_Result;
        }

        /// <summary>
        /// 呼叫Gash Web Service 取得此GASH主帳號所開啟的該服務所有帳號
        /// </summary>
        /// <param name="Str_GashAccount">Gash帳號</param>
        /// <param name="Str_ServiceCode">服務代碼</param>
        /// <param name="Str_Region">服務區域代碼</param>
        /// <returns>
        /// - ＜DataSet xmlns="http://GASHv30FWS/Service/"＞
        /// - ＜xs:schema id="NewDataSet" xmlns="" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata"＞
        /// - ＜xs:element name="NewDataSet" msdata:IsDataSet="true"＞
        /// - ＜xs:complexType＞
        /// - ＜xs:choice maxOccurs="unbounded"＞
        /// - ＜xs:element name="Result"＞
        /// - ＜xs:complexType＞
        /// - ＜xs:sequence＞
        ///  ＜xs:element name="ServiceAccountID" type="xs:string" minOccurs="0" /＞ 
        ///  ＜/xs:sequence＞
        ///  ＜/xs:complexType＞
        ///  ＜/xs:element＞
        ///  ＜/xs:choice＞
        ///  ＜/xs:complexType＞
        ///  ＜/xs:element＞
        ///  ＜/xs:schema＞
        ///  DataSet will be null when failed
        /// </returns>
        public static DataSet Service_GetAllServiceAccounts(string Str_GashAccount, string Str_ServiceCode, string Str_Region)
        {
            DataSet ds = new DataSet();
            Service ws = new Service();

            try
            {
                ds = ws.Service_GetAllServiceAccounts(Str_GashAccount, Str_ServiceCode, Str_Region);
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
            ws.Dispose();
            return ds;
        
        }
        //****由遊戲帳號找出gash帳號****   by  joda
        public static string GetMainAccountID(string strGame_ID, string Str_ServiceCode, string Str_Region)
        {

            string strgash = "";
            ServiceAccount ws = new ServiceAccount();

            try
            {
                //strgash = Str_ServiceCode + ";" + Str_Region + ";" + strGame_ID;
                strgash = ws.ServiceAccount_GetMainAccountID(Str_ServiceCode, Str_Region, strGame_ID);
                if (strgash.Split(";".ToCharArray())[0] != "1")
                {
                    strgash = "0";
                }
                else
                {
                    strgash = strgash.Split(";".ToCharArray())[1].ToString();
                }
            }
            catch (Exception ex)
            {
                strgash = "-1";
            }

            ws.Dispose();
            return strgash;

        }

        /// <summary>     
        //		/*'==================================================================================================
        //		'程式說明：檢查是否已開啟線上交易的服務
        //		'傳入參數：	1._strMainAccountID ：Gash帳號
        //		'		2.strServiceCode ： 線上交易服務代碼
        //		'		3._strServiceRegion：服務區域代碼
        //		'		4._strServiceAccountID：服務帳號
        //		'傳回結果：	0; Output message -- 尚未開啟,則自動替User開啟
        //		'		1; Output message -- 已開啟
        //		'		-1; Output message -- Error
        //		'==================================================================================================*/
        public static int CheckOpenEventService(string tmpGash, string tmpServiceCode, string tmpRegion, string strGashRegion)
        {
            try
            {
                ServiceAccount sp = new ServiceAccount();

                switch (strGashRegion.ToUpper())
                {
                    case "TW":
                        sp.Url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["GASHv30FWS_ServiceAccount"] ?? "");
                        break;
                    case "HK":
                        sp.Url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["GASHv30FWS_ServiceAccount_HK"] ?? "");
                        break;
                    default:
                        sp.Url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["GASHv30FWS_ServiceAccount"] ?? "");
                        break;
                }


                if (tmpGash.Length <= 0)
                { return 0; }
                else
                {
                    string myresult = sp.ServiceAccount_CreateSimple(tmpGash, tmpServiceCode, tmpRegion, tmpGash);
                    string myresult1;
                    string myresult2;

                    if (myresult.IndexOf("0;ServiceAccountID_Exists", 0, myresult.Length) >= 0 || myresult.IndexOf("0;DisplayName_Exists", 0, myresult.Length) >= 0)
                        myresult1 = "1";
                    else
                        myresult1 = "0";

                    if (myresult.IndexOf("0;Over_MaxAccount", 0, myresult.Length) >= 0)
                        myresult2 = "1";
                    else
                        myresult2 = "0";

                    myresult = myresult.Substring(0, 1);

                    if (myresult == "1" || myresult1 == "1" || myresult2 == "1")
                    { return 1; }
                    else
                    { return 0; }
                }
            }
            catch
            {
                return 0;
            }
        }

       

        /// <summary>
        /// 呼叫Gash Web Service 取得 玩家擁有gash+專用點數  //edit 2009/11/10
        /// </summary>
        /// <param name="tmpGash">Gash帳號</param>
        /// <returns>(成功:玩家的點數 失敗:-1)</returns>
        public static int GetUserGashPoint(string tmpGash, string str_Region)
        {
            string ServiceCode = (string)ConfigurationManager.AppSettings["PayServiceCode"]??"";
            string ServiceRegion = (string) ConfigurationManager.AppSettings["PayServiceRegion"] ?? "";
            string x = string.Empty;
            try
            {
                ServiceAccount Gash_sp = new ServiceAccount();
                MainAccount sp = new MainAccount();
                switch (str_Region.ToUpper())
                {
                    case "TW":
                        Gash_sp.Url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["GASHv30FWS_ServiceAccount"] ?? "");
                        sp.Url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["GASHv30FWS_MainAccount"] ?? "");
                        break;
                    case "HK":
                        Gash_sp.Url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["GASHv30FWS_ServiceAccount_HK"] ?? "");
                        sp.Url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["GASHv30FWS_MainAccount_HK"] ?? "");
                        ServiceRegion = (string) ConfigurationManager.AppSettings["PayServiceRegion_HK"] ?? "";
                        break;
                    default:
                        Gash_sp.Url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["GASHv30FWS_ServiceAccount"] ?? "");
                        sp.Url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["GASHv30FWS_MainAccount"] ?? "");
                        break;
                }

                if (tmpGash.Length <= 0)
                { return -1; }
                else
                {
                    string myresult = Gash_sp.ServiceAccount_GetRemainingPoints(ServiceCode, ServiceRegion, tmpGash);
                    Gash_sp.Dispose();

                    if (myresult.Length > 0)
                    {
                        if (myresult.Substring(0, 2).ToString() == "1;")
                        {
                            x = myresult.Substring(2, myresult.Length - 2);
                            return System.Int32.Parse(x);
                        }
                        else if (myresult == "-1;Check_ServiceAccount_Failed")
                        {
                            
                            myresult = sp.MainAccount_GetRemainingPoints(tmpGash);
                            if (myresult.Length > 0)
                            {
                                if (myresult.Substring(0, 2).ToString() == "1;")
                                {
                                    x = myresult.Substring(2, myresult.Length - 2);
                                    return System.Int32.Parse(x);
                                }
                                else
                                    return -1;
                            }
                            else
                                return -1;

                        }
                        else
                            return -1;
                    }
                    else
                        return -1;
                }
            }
            catch
            {
                return -1;
            }

        }
        ///// <summary>
        ///// 呼叫Gash Web Service 取得 玩家擁有gash點數
        ///// </summary>
        ///// <param name="tmpGash">Gash帳號</param>
        ///// <returns>(成功:玩家的點數 失敗:-1)</returns>
        public static int GetUserGashPoint_old(string tmpGash, string str_Region)
        {
                    string ServiceCode = (string)ConfigurationManager.AppSettings["PayServiceCode"] ?? "";
                    string ServiceRegion = (string)ConfigurationManager.AppSettings["PayServiceRegion"] ?? "";

            try
            {
                MainAccount Gash_sp = new MainAccount();
                switch (str_Region.ToUpper())
                {
                    case "TW":
                        Gash_sp.Url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["GASHv30FWS_MainAccount"] ?? "");
                        break;
                    case "HK":
                        Gash_sp.Url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["GASHv30FWS_MainAccount_HK"] ?? "");
                        ServiceRegion = (string)ConfigurationManager.AppSettings["PayServiceRegion_HK"] ?? "";

                        break;
                    default:
                        Gash_sp.Url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["GASHv30FWS_MainAccount"] ?? "");
                        break;
                }


                if (tmpGash.Length <= 0)
                { return -1; }
                else
                {
                    //取得遊戲專用點數
                    int iServicePoints = 0;
                    //if (ConfigurationManager.AppSettings["Gash_HaveServicePoints"] == "true" && str_Region.ToUpper() == "TW")
                    //{
                        using (GashGetServicePoint.Service Gash_sp2 = new GashGetServicePoint.Service())
                        {
                            DataSet ds = new DataSet();
                            ds = Gash_sp2.Service_GetInfo(tmpGash, ServiceCode, ServiceRegion);
                            if (ds.Tables.Count > 0)
                                iServicePoints = Convert.ToInt32(ds.Tables[0].Rows[0][2].ToString());
                            ds.Dispose();
                        }
                    //}

                    string myresult = Gash_sp.MainAccount_GetRemainingPoints(tmpGash);
                    if (myresult.Length > 0)
                    {
                        if (myresult.Substring(0, 2).ToString() == "1;")
                        {
                            string x = myresult.Substring(2, myresult.Length - 2);
                            return System.Int32.Parse(x) + iServicePoints;
                        }
                        else
                            return -1;
                    }
                    else
                        return -1;
                }
            }
            catch
            {
                return -1;
            }

        }
        //}
        /// <summary>
        //		/*=============================================================================================
        //		'程式說明：扣玩家Gash點數
        //		'傳入參數：	MainAccountID : GASH_ID
        //		'		Memo:內容("橘子柑仔店競標")
        //		'		ServiceCode : 服務代碼(如:"999995")
        //		'		ServiceRegion : 服務區域代碼 (如:"T0")
        //		'		Time : 消費時間
        //		'		Point : 此次消費點數
        //		'		Memo : 附註
        //		'		IPAddress : IP位址
        //		'傳回結果：	"0":失敗
        //		'		"1":成功
        //		=============================================================================================*/
        public static string Deduct_GASHPoint_ServiceAccount(string tmpGash, string tempService, string tmpRegion, string str_Region, int ExpectPont, string tmpMemo, string tmpIP)
        {

            try
            {
                AAA Gash_WS = new AAA();
                switch (str_Region.ToUpper())
                {
                    case "TW":
                        Gash_WS.Url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["GASHv30FWS_AAA"] ?? "");
                        break;
                    case "HK":
                        Gash_WS.Url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["GASHv30FWS_AAA_HK"] ?? "");
                        break;
                    default:
                        Gash_WS.Url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["GASHv30FWS_AAA"] ?? "");
                        break;
                }


                if (tmpGash.Length <= 0 || ExpectPont <= 0)
                { return "0;點數錯誤(111)"; }
                else
                {
                    string nowDate = DateTime.Now.ToString("yyyy/MM/dd") + " " + DateTime.Now.Hour.ToString() + DateTime.Now.ToString(":mm:ss");    //避免上下午混用，轉成24H制
                    string myresult = Gash_WS.ServiceAccount_AccountingFreePoint(tempService, tmpRegion, tmpGash, nowDate, ExpectPont, tmpMemo, tmpIP);
                    if (myresult.Length > 0)
                    {
                        if (myresult.Substring(0, 1) == "1")
                        { return "1"; }
                        else
                        { return myresult; }
                    }
                    else
                    {
                        return "0;扣點回傳錯誤(沒有回傳值)(111)";
                    }
                }

            }
            catch
            {
                return "0;扣點發生不明錯誤(111)";
            }
        }
        /// <summary>
        //		/*=============================================================================================
        //		'程式說明：扣玩家Gash點數
        //		'傳入參數：	MainAccountID : GASH_ID
        //		'		Memo:內容("橘子柑仔店競標")
        //		'		ServiceCode : 服務代碼(如:"999995")
        //		'		ServiceRegion : 服務區域代碼 (如:"T0")
        //		'		Time : 消費時間
        //		'		Point : 此次消費點數
        //		'		Memo : 附註
        //		'		IPAddress : IP位址
        //		'傳回結果：	"0":失敗
        //		'		"1":成功
        //		=============================================================================================*/
        public static string Deduct_GASHPoint(string tmpGash, string tempService, string tmpRegion, string str_Region, int ExpectPont, string tmpMemo, string tmpIP)
        {

            try
            {
                AAA Gash_WS = new AAA();
                switch (str_Region.ToUpper())
                {
                    case "TW":
                        Gash_WS.Url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["GASHv30FWS_AAA"] ?? "");
                        break;
                    case "HK":
                        Gash_WS.Url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["GASHv30FWS_AAA_HK"] ?? "");
                        break;
                    default:
                        Gash_WS.Url = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["GASHv30FWS_AAA"] ?? "");
                        break;
                }


                if (tmpGash.Length <= 0 || ExpectPont <= 0)
                { return "0;點數錯誤(111)"; }
                else
                {
                    string nowDate = DateTime.Now.ToString("yyyy/MM/dd") + " " + DateTime.Now.Hour.ToString() + DateTime.Now.ToString(":mm:ss");    //避免上下午混用，轉成24H制
                    string myresult = Gash_WS.MainAccount_AccountingFreePoint(tmpGash, tempService, tmpRegion, nowDate, ExpectPont, tmpMemo, tmpIP);

                    if (myresult.Length > 0)
                    {
                        if (myresult.Substring(0, 1) == "1")
                        { return "1"; }
                        else
                        { return myresult; }
                    }
                    else
                    {
                        return "0;扣點回傳錯誤(沒有回傳值)(111)";
                    }
                }

            }
            catch
            {
                return "0;扣點發生不明錯誤(111)";
            }
        }
        
        /// <summary>
    /// 呼叫Gash Web Service 進行 GASH主帳號認證
    /// </summary>
    /// <param name="Str_GashAccount">Gash帳號</param>
    /// <param name="Str_GashPassWord">Gash密碼</param>
    /// <returns>(1 = Success, 0 = Failed, -1 = Error, -2;ErrorMessage = 呼叫Web Service發生錯誤,ErrorMessage為錯誤訊息)</returns>
    public static string MainAccount_Authentication_New(string Str_GashAccount, string Str_GashPassWord)
    {
        try
        {
            MainAccount ws = new MainAccount();
            if (Str_GashAccount.Length <= 0 || Str_GashPassWord.Length <= 0)
            { return "0"; }
            else
            {
                string myresult = ws.MainAccount_Authentication(Str_GashAccount, Str_GashPassWord);
                string myresult1 = myresult;

                myresult = myresult.Substring(0, 1);

                if (myresult == "1")
                { return "1"; }
                else if (myresult1.IndexOf("0;Too_Many_Error_Try", 0, myresult1.Length) > -1)
                { return "2"; }
                else if (myresult1.IndexOf("0;MainAccount_is_Locked", 0, myresult1.Length) > -1)
                { return "3"; }
                else if (myresult1.IndexOf("-1;System Error:", 0, myresult1.Length) > -1)
                { return "0"; }
                else
                { return "0"; }
            }

        }
        catch
        {
            return "0";
        }
    }   

/// <summary>
        /// 呼叫Gash Web Service 	取得此GASH主帳號所開啟的該服務所有帳號與狀態
        /// </summary>
        /// <param name="Str_GashAccount">Gash帳號</param>
        /// <param name="Str_ServiceCode">服務代碼</param>
        /// <param name="Str_Region">服務區域代碼</param>
        /// <returns>
        /// <DataSet xmlns="http://GASHv30FWS/Service/">
				/// <xs:schema id="NewDataSet" xmlns="" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata">
				/// <xs:element name="NewDataSet" msdata:IsDataSet="true">
				/// <xs:complexType>
				/// <xs:choice maxOccurs="unbounded">
				/// <xs:element name="Result">
				/// <xs:complexType>
				/// <xs:sequence>
				/// <xs:element name="ServiceAccountID" type="xs:string" minOccurs="0" /> 
				/// <xs:element name="Type" type="xs:string" minOccurs="0" /> 
        /// <xs:element name="ChargeRule" type="xs:string" minOccurs="0" /> 
        /// <xs:element name="ExpirationTime" type="xs:dateTime" minOccurs="0" /> 
  		  /// <xs:element name="UsedPoints" type="xs:int" minOccurs="0" /> 
        /// <xs:element name="NewAccountFlag" type="xs:boolean" minOccurs="0" /> 
  			/// <xs:element name="Creator" type="xs:string" minOccurs="0" /> 
  			/// <xs:element name="CreateTime" type="xs:dateTime" minOccurs="0" /> 
  			/// </xs:sequence>
        /// </xs:complexType>
        /// </xs:element>
        /// </xs:choice>
        /// </xs:complexType>
        /// </xs:element>
        /// </xs:schema>
				/// DataSet will be null when failed
        /// </returns>
        public static DataSet Service_GetAllServiceAccountsStatus(string Str_GashAccount, string Str_ServiceCode, string Str_Region)
        {
            DataSet ds = new DataSet();
            Service ws = new Service();

            try
            {
                ds = ws.Service_GetAllServiceAccountsStatus(Str_GashAccount, Str_ServiceCode, Str_Region);
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
            ws.Dispose();
            return ds;        
        }

           
        /// <summary>
        /// 遊戲帳號7日驗證
        /// </summary>
        /// <param name="tmpGameAccount">遊戲帳號</param>
        /// <param name="tmpServiceCode">線上交易服務代碼</param>
        /// <param name="tmpRegion">服務區域代碼</param>
        /// <param name="tmpDays">檢查天數</param>
        /// <returns>2:>7日 1:<=7日 0:參數錯誤 -1:查尋失敗</returns>
        public static string CheckExperience7Days(string tmpGameAccount, string tmpServiceCode, string tmpRegion,int tmpDays)
        {
            string strResult = "-1";
            DateTime datCondiction;
            GashService.Execute_Result exeResult = new GashService.Execute_Result();
            if (tmpGameAccount.Length < 8 || tmpGameAccount.Length > 20)
                strResult = "0";
            else
            {
                GashService.Service ws = new GashService.Service();
                try
                {
                    exeResult = ws.ServiceAccount_GetCreateTime(tmpServiceCode, tmpRegion, tmpGameAccount);

                    if (exeResult.Result == 1)
                    {
                        datCondiction = Convert.ToDateTime(exeResult.OutString);
                        datCondiction = datCondiction.AddDays(tmpDays);

                        if (System.DateTime.Now > datCondiction)
                            strResult = "2";
                        else
                            strResult = "1";

                    }
                    else if (exeResult.Result == 0)
                    {
                        strResult = "0";
                    }
                    else
                    {
                        strResult = "-1";
                    }
                    
                }
                catch
                {
                    strResult = "-1";
                }
            }
            return strResult;

        }

  

    }

}
using System;
using System.Data;
using System.Web;
using System.Text.RegularExpressions;

/// <summary>
///  SD 公用函式
/// </summary>
namespace SDCodeCheck
{
    /// <summary>
    /// 驗證類型的共用函式
    /// </summary>
    public class Valid
    {
        #region //isNumeric// 檢查輸入的String使否為 Int32的數字
        /// <summary>
        /// 檢查輸入的String使否為 Int32的數字
        /// </summary>
        /// <param name="val">輸入值</param>
        /// <returns></returns>
        public static bool isNumeric(string val)
        {
            System.Globalization.NumberStyles NumberStyle = System.Globalization.NumberStyles.Integer;
            int result;
            return Int32.TryParse(val, NumberStyle, System.Globalization.CultureInfo.CurrentCulture, out result);
        }
        #endregion

        #region //isNumericLong// 檢查輸入的String使否為 Int64的數字
        /// <summary>
        /// 檢查輸入的String使否為 Int64 的數字
        /// </summary>
        /// <param name="val">輸入值</param>
        /// <returns></returns>
        public static bool isNumericLong(string val)
        {
            System.Globalization.NumberStyles NumberStyle = System.Globalization.NumberStyles.Integer;
            long result;
            return Int64.TryParse(val, NumberStyle, System.Globalization.CultureInfo.CurrentCulture, out result);

        }
        #endregion

        #region //isDate// 檢查輸入的String使否為日期型態
        /// <summary>
        /// 檢查輸入的String使否為日期型態
        /// </summary>
        /// <param name="val">輸入值</param>
        /// <returns></returns>
        public static bool isDate(string val)
        {
            DateTime btime;
            return  DateTime.TryParse(val,out btime);
        }
        #endregion

        #region //CheckStrInputLen//  驗證string是否符合長度限制
        /// <summary>
        /// 驗證string是否符合長度限制
        /// </summary>
        /// <param name="strInput">輸入值</param>
        /// <param name="nMin">字串最小長度</param>
        /// <param name="nMax">字串最大長度</param>
        public static bool CheckStrInputLen(string strInput, int nMin, int nMax)
        {
            if (strInput.Trim() != "" && strInput.Length >= nMin && strInput.Length <= nMax)
                return true;
            else
                return false;
        }
        #endregion

        #region //IsValidEngNum// 檢查輸入的String使否為英數字
        /// <summary>
        ///檢查輸入的String使否為英數字
        /// </summary>
        /// <param name="val">輸入值</param>
        public static bool IsValidEngNum(string val)
        {
            Regex re = new Regex(@"^[a-zA-Z0-9]+$");
            Match m = re.Match(val);
            return m.Success;
        }
        #endregion

        #region //IsValidEngNumLen// 檢查輸入的String使否為英數字,且限制String長度
        /// <summary>
        ///檢查輸入的String使否為英數字,且限制String長度
        /// </summary>
        /// <param name="val">輸入值</param>
        /// <param name="nMin">字串最小長度</param>
        /// <param name="nMax">字串最大長度</param>
        public static bool IsValidEngNumLen(string val,int nMin,int nMax)
        {
            if (IsValidEngNum(val) && CheckStrInputLen(val, nMin, nMax))
                return true;
            else
                return false;
        }
        #endregion

        #region //IfNoRequest// 簡易的SQL Injection 過濾
        /// <summary>
        /// 簡易的 SQL Injection 過濾
        /// </summary>
        /// <param name="RequestValue">過濾前的值</param>
        /// <param name="ThisValue">過濾後的值</param>
        /// <returns></returns>
        public static string IfNoRequest(string RequestValue, string ThisValue)
        {
            string ConvertValue = ThisValue;
            if (RequestValue != "" && RequestValue != null && RequestValue.Length > 0)
            {
                ConvertValue = RequestValue.ToString().Trim();
                ConvertValue = ConvertValue.Replace("'", "&quot;");
                ConvertValue = ConvertValue.Replace("<", "&lt;");
                ConvertValue = ConvertValue.Replace(">", "&gt;");
                ConvertValue = ConvertValue.Replace(";", "；");
                ConvertValue = ConvertValue.Replace("&", "&amp;");
                ConvertValue = ConvertValue.Replace("-", "－");
                ConvertValue = ConvertValue.Replace("iframe", "");
                ConvertValue = ConvertValue.Replace("　", "");
            }
            return ConvertValue;
        }
        #endregion

        #region //ChkSQLInjectWord// 檢查輸入的值是否有包含 SQL Injection Word
        /// <summary>
        /// 檢查輸入的值是否有包含 SQL Injection Word
        /// </summary>
        /// <param name="InputStr">要檢查的字串</param>
        /// <returns></returns>        
        public static bool ChkSQLInjectWord(string InputStr)
        {
            int chkcount = 0;
            if (InputStr.IndexOf("'", 0, InputStr.Length) > -1)
                chkcount++;
            if (InputStr.IndexOf(";", 0, InputStr.Length) > -1)
                chkcount++;
            if (InputStr.IndexOf("/*", 0, InputStr.Length) > -1)
                chkcount++;
            if (InputStr.IndexOf("*/", 0, InputStr.Length) > -1)
                chkcount++;

            if (chkcount > 0)
                return false;
            else
                return true;
        }
        #endregion

        #region //EMailCheck// 驗證 EMail Address 是否允許使用
        /// <summary>
        /// 驗證 EMail Address 是否允許使用
        /// </summary>
        /// <param name="EMail">要檢查的Email字串</param>  
        /// <returns></returns>
        public static bool EMailCheck(string EMail)
        {            
            int atIndex;
            string TempEMail;
            string AccountName;
            string DomainName;
            string[] DomainArray;
            bool CheckAllow;
            string AccountAllowChar;
            string DomainAllowChar;
            string TempChar;
            bool NotFound;
            int I;
            int J;

            CheckAllow = false;
            AccountAllowChar = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ~%&-_+=\\/.:";
            DomainAllowChar = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_";
            TempEMail = EMail;

            atIndex = TempEMail.IndexOf("@");
            if (atIndex != -1)
            {
                // 解析帳號與 Domain, 同時針對 Domain 分割 
                AccountName = TempEMail.Substring(0, atIndex);
                DomainName = TempEMail.Substring(atIndex + 1);
                DomainArray = DomainName.Split('.');

                char[] AccountNameChar= AccountName.ToCharArray();
                
                // 驗證帳號正確性 
                NotFound = false;
                if (AccountName != string.Empty)
                {
                    
                    for (I = 0; I <= AccountNameChar.Length - 1; I++)
                    {
                        TempChar = AccountNameChar[I].ToString();
                        if (AccountAllowChar.IndexOf(TempChar) == -1)
                        {
                            NotFound = true;
                            break; // TODO: might not be correct. Was : Exit For 
                        }
                    }

                    if (NotFound == false)
                    {
                        // 驗證 Domain 正確性 
                        if ((DomainArray == null) == false)
                        {
                            if (DomainArray.Length >= 0)
                            {
                                for (I = 0; I <= DomainArray.Length - 1; I++)
                                {
                                    if (DomainArray[I].Length > 0)
                                    {
                                        for (J = 0; J <= DomainArray[I].Length - 1; J++)
                                        {
                                            char[] DomainCharArray = DomainArray[I].ToCharArray();
                                            TempChar = DomainCharArray[J].ToString();
                                            if (DomainAllowChar.IndexOf(TempChar) == -1)
                                            {
                                                NotFound = true;
                                                break; // TODO: might not be correct. Was : Exit For 
                                            }
                                        }
                                    }
                                    else
                                    {
                                        NotFound = true;
                                    }
                                }
                            }
                            else
                            {
                                NotFound = true;
                            }
                        }
                        else
                        {
                            NotFound = true;
                        }

                        if (NotFound == false)
                        {
                            CheckAllow = true;
                        }
                    }
                }
            }

            return CheckAllow;
        }
        #endregion

        #region //PIDCheck// 驗證 身分證是否正確
        /// <summary>
        ///  驗證身分證是否正確
        /// </summary>
        /// <param name="PID">身分證 String</param>   
        /// <returns></returns>   
        public static bool PIDCheck(string PID)
        {          
            int[] PIDValue;
            string[] PIDHead;
            int PIDPosition;
            string HeadStr;
            int HeadHigh;
            int HeadLow;
            int CheckSum;
            int SumValue;
            bool CheckPass;
            int I;
            char[] PIDCharArray = PID.ToCharArray();

            CheckPass = false;
            if (PID.Length == 10)
            {
                PIDValue = new int[] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35 };
                PIDHead = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "X", "Y", "W", "Z", "I", "O" };

                HeadStr = PIDCharArray[0].ToString();
                string strCheckSum = PID.Substring(PID.Length - 1, 1);
                if (isNumeric(strCheckSum))
                    CheckSum = Convert.ToInt32(strCheckSum);
                else
                    return false;
                PIDPosition = -1;
                for (I = 0; I <= PIDHead.Length - 1; I++)
                {
                    string tempHead = PIDHead[I].ToString();
                    if (tempHead == HeadStr.ToUpper())
                    {
                        PIDPosition = I;
                        break; // TODO: might not be correct. Was : Exit For 
                    }
                }

                if (PIDPosition > -1)
                {
                    HeadHigh = PIDValue[PIDPosition] / 10;
                    HeadLow = PIDValue[PIDPosition] % 10;

                    SumValue = HeadHigh + 9 * HeadLow;
                    for (I = 1; I <= 8; I++)
                    {
                        if (isNumeric(PIDCharArray[I].ToString()))
                            SumValue = SumValue + ((9 - I) * Convert.ToInt32(PIDCharArray[I].ToString()));
                        else
                            return false; 
                    }

                    SumValue = SumValue + CheckSum;
                    if ((SumValue % 10) == 0)
                    {
                        CheckPass = true;
                    }
                }
            }

            return CheckPass;
        }
        #endregion

        #region //DateDiff 回傳兩個日期時間相差的數字
        /// <summary>
        /// DateDiff 回傳兩個日期時間相差的數字
        /// </summary>
        /// <param name="howtocompare">比較的單位 年:year,月:month,日:day,時:hour,分:minute,秒:second</param>
        /// <param name="startDate">開始時間</param>
        /// <param name="endDate">結束時間</param>
        /// <returns></returns>
        public static double DateDiff(string howtocompare, System.DateTime startDate, System.DateTime endDate) 
			{ 
  	 		double diff=0; 
   			System.TimeSpan TS = new System.TimeSpan(endDate.Ticks-startDate.Ticks);    
   
  			switch (howtocompare.ToLower()) 
   				{ 
    			case "year": 
     				diff = Convert.ToDouble(TS.TotalDays/365); 
     				break; 
    			case "month": 
     				diff = Convert.ToDouble((TS.TotalDays/365)*12); 
     				break; 
    			case "day":
     				diff = Convert.ToDouble(TS.TotalDays); 
     				break; 
    			case "hour": 
     				diff = Convert.ToDouble(TS.TotalHours); 
     				break; 
    			case "minute": 
     				diff = Convert.ToDouble(TS.TotalMinutes); 
     				break; 
    			case "second": 
     				diff = Convert.ToDouble(TS.TotalSeconds); 
     				break; 
   				}   
   			return diff;
        }
        #endregion
    }
}

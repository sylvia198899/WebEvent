using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using System.Net;
using System.Text;

/// <summary>
///  SD 公用函式
/// </summary>
namespace SDCodeCheck
{
    /// <summary>
    /// Web Util
    /// </summary>
	public class WebComm
    {
        #region //GetUserIP// 取得使用者的IP    
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
        #endregion

        #region //GetRamPassword// 產生亂數的英數字密碼
        /// <summary>
        ///  產生亂數的英數字密碼
        /// </summary>
        /// <param name="_intPwdLength">要產生的亂數長度</param>
        /// <returns></returns>
        public static string GetRamPassword(int _intPwdLength)
        {
            int charindex = 0;
            string strReturn = string.Empty;
            string _strPassword = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            char[] _charArrayPassword = _strPassword.ToCharArray();
            Random Rnd = new Random();

            for (int i = 1; i <= _intPwdLength ; i++)
            {
                charindex = Rnd.Next(0, _charArrayPassword.Length);
                strReturn += _charArrayPassword[charindex].ToString();
            }

            Rnd = null;
            return strReturn;            
        }
        #endregion

        #region //GetUserIP// 取得遠端網頁
        /// <summary>
        /// 取得遠端網頁
        /// </summary>
        /// <param name="sUrl">要呼叫的網址</param>
        /// <param name="para">呼叫網址所需的參數</param>
        /// <param name="Method">呼叫網址的方法(GET,POST)</param>
        /// <param name="PageEncoding">遠端網頁的編碼方式</param>
        /// <returns></returns>
        public string GetXMLHttp(string sUrl, string para, string Method, System.Text.Encoding PageEncoding)
        {
            string strResult = string.Empty;
            byte[] SendBytes;

            if (Method.ToUpper() != "POST")
                Method = "GET";
            else
                Method = "POST";

            if (PageEncoding == null)
                PageEncoding = System.Text.Encoding.UTF8;

            try
            {
                // for .Net 1.0 (Accept Use HTTPS (SSL)
                //ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                
                HttpWebRequest hq;
                if (Method=="GET")
                    hq = (HttpWebRequest)WebRequest.Create(sUrl+"?"+para);
                else
                    hq = (HttpWebRequest)WebRequest.Create(sUrl);

                hq.Method = Method;
                hq.Accept = "*/*";
                hq.UserAgent = "Sender";                
                hq.Timeout = 20000;
                hq.ReadWriteTimeout = 30000;
                hq.KeepAlive = false;

                if (Method == "POST")
                {
                        SendBytes=System.Text.Encoding.Default.GetBytes(para);
                        hq.ContentType="application/x-www-form-urlencoded";
                        hq.ContentLength=SendBytes.Length;
                        hq.GetRequestStream().Write(SendBytes, 0, SendBytes.Length);
                }

                HttpWebResponse sRequest = (HttpWebResponse)hq.GetResponse();
                StreamReader sr = new StreamReader(sRequest.GetResponseStream(), PageEncoding);
                string _result = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
                sRequest.Close();

                strResult = _result;
                hq = null;

            }
            catch (Exception ex)
            {
                strResult = "";
            }            
            return strResult;
        }
        #endregion

        //-------------------------------------------------------------------------------------
        #region //用來解決用HttpWebRequest來取得Https(SSL)時異常的問題 for.Net 1.0
        //private class TrustAllCertificatePolicy : System.Net.ICertificatePolicy
        //{
        //    public TrustAllCertificatePolicy()
        //    { }
        //    public bool CheckValidationResult(ServicePoint sp, System.Security.Cryptography.X509Certificates.X509Certificate cert, WebRequest req, int problem)
        //    {
        //        return true;
        //    }
        //}
        #endregion

        #region //用來解決用HttpWebRequest來取得Https(SSL)時異常的問題 for.Net 2.0
        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors errors)
        {
            //Always Accept
            return true;
        }
        private bool CheckValidationResult(ServicePoint sPoint, System.Security.Cryptography.X509Certificates.X509Certificate cert, WebRequest wRequest, int certProb)
        { 
            //Always Accetp
            return true;
        }
        #endregion
        //-------------------------------------------------------------------------------------

        /// <summary>
        /// 透過程式來產生Post網頁
        /// </summary>
        public class RemotePost
        {
            private System.Collections.Specialized.NameValueCollection Inputs = new System.Collections.Specialized.NameValueCollection();

            public string Url = "";
            public string Method = "post";
            public string FormName = "form1";

            #region //Add //增加隱藏欄位
            /// <summary>
            /// 增加隱藏欄位
            /// </summary>
            /// <param name="name">參數名稱</param>
            /// <param name="value">參數值</param>
            public void Add(string name, string value)
            {
                Inputs.Add(name, value);
            }

            #endregion

            #region //Post Page
            /// <summary>
            /// Post Page 
            /// </summary>
            public void Post()
            {
                System.Web.HttpContext.Current.Response.Clear();

                System.Web.HttpContext.Current.Response.Write("<html><head>");

                System.Web.HttpContext.Current.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
                System.Web.HttpContext.Current.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, Url));
                for (int i = 0; i < Inputs.Keys.Count; i++)
                {
                    System.Web.HttpContext.Current.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
                }
                System.Web.HttpContext.Current.Response.Write("</form>");
                System.Web.HttpContext.Current.Response.Write("</body></html>");

                System.Web.HttpContext.Current.Response.End();
            }
            #endregion
        }

        #region //CSVExplor// 將DataTable 輸出成CSV格式
        /// <summary>
        /// 將DataTable 輸出成CSV格式
        /// </summary>
        /// <param name="tb">資料來源DataTable</param>
        /// <param name="strSymbol">可自訂分隔符號 String.Empty=> , </param>
        /// <param name="blTitle">是否要輸出標頭 是:true 否:false</param>
        /// <returns></returns>
        public static string CSVExplor(DataTable tb, string strSymbol,bool blTitle)
        {
            StringBuilder sb_Header = new StringBuilder();
            StringBuilder sb_Output = new StringBuilder();

            if (strSymbol == string.Empty ) { strSymbol = ","; }
            try
            {
                if (blTitle)
                {
                    //標題描述
                    for (int i = 0; i <= tb.Columns.Count - 1; i++)
                    {
                        sb_Header.Append(tb.Columns[i].ColumnName);
                        sb_Header.Append(strSymbol);
                    }
                    sb_Header.Remove(sb_Header.Length - 1, 1); //移除最後一個字元
                    sb_Header.Append("\r\n");
                }

                //資料內容
                foreach (DataRow row in tb.Rows)
                {
                    foreach (DataColumn column in tb.Columns)
                    {
                        if (column.DataType == System.Type.GetType("System.DateTime"))
                        {

                            sb_Output.Append(Convert.ToDateTime(row[column].ToString()).ToString("yyyy/MM/dd HH:mm:ss"));                            
                        }
                        else
                        {
                            sb_Output.Append(row[column].ToString());
                         
                        }
                        sb_Output.Append(strSymbol);
                    }
                    sb_Output.Remove(sb_Output.Length - 1, 1);//移除最後一個字元
                    sb_Output.Append("\r\n");
                }
           


            }
            catch
            {
                return string.Empty;
            }

            string strResult = sb_Header.ToString() + sb_Output.ToString();

            return strResult;

        }
        #endregion

    }
}

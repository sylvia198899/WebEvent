using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using Gamania.SD.Library;

namespace Gamania.SD.Gash
{

}

namespace Gamania.SD.Helper
{
    /// <summary>
    /// GashInfo 2009.12.22
    /// </summary>
    public class GashInfo
    {
        public Page Page;
        protected int alertMode = 0;
        protected string messageText = string.Empty;
        protected string targetUrl = string.Empty;

        private int gameAccountMinLength = 8;
        private int gameAccountMaxLength = 20;
        private int gashAccountMinLength = 8;
        private int gashAccountMaxLength = 20;

        private string gashRegion = string.Empty;
        private string gashAccount = string.Empty;
        private string gameAccount = string.Empty;

        public int GameAccountMinLength
        {
            set { this.gameAccountMinLength = value; }
        }

        public int GameAccountMaxLength
        {
            set { this.gameAccountMaxLength = value; }
        }

        public int GashAccountMinLength
        {
            set { this.gashAccountMinLength = value; }
        }

        public int GashAccountMaxLength
        {
            set { this.gashAccountMaxLength = value; }
        }

        public List<string> GashRegionCountry = new List<string>();

        /// <summary>
        /// Nothing
        /// </summary>
        public GashInfo()
        {
        }

        /// <summary>
        /// Alert
        /// </summary>
        /// <param name="messageText"></param>
        public GashInfo(string messageText)
        {
            this.alertMode = 1;
            this.messageText = messageText;
        }

        /// <summary>
        /// Redirect or Close
        /// </summary>
        /// <param name="messageText"></param>
        /// <param name="targetUrl"></param>
        public GashInfo(string messageText, string targetUrl)
        {
            this.alertMode = 2;
            this.messageText = messageText;
            this.targetUrl = targetUrl;
        }

        private string AlertOrRedirect()
        {
            if (this.alertMode == 1) Script.AlertThenClose(this.Page,this.messageText);

            if (this.alertMode == 2)
            {
                if (string.IsNullOrEmpty(targetUrl))
                    Script.AlertThenClose(this.Page, messageText);
                else
                    Script.AlertThenRedirect(this.Page, messageText, targetUrl);
            }

            return string.Empty;
        }

        /// <summary>
        /// Game Account
        /// </summary>
        public string GameAccount
        {
            get
            {
                if (this.GashUserClass.ServiceAccountID.Length > this.gameAccountMaxLength) this.AlertOrRedirect();
                if (this.GashUserClass.ServiceAccountID.Length < this.gameAccountMinLength) this.AlertOrRedirect();

                return this.GashUserClass.ServiceAccountID;
            }
            set
            {
                this.GashUserClass.ServiceAccountID = value;
            }
        }

        /// <summary>
        /// Gash Account
        /// </summary>
        public string GashAccount
        {
            get
            {
                if (this.GashUserClass.MainAccountID.Length > this.gashAccountMaxLength) this.AlertOrRedirect();
                if (this.GashUserClass.MainAccountID.Length < this.gashAccountMinLength) this.AlertOrRedirect();

                return this.GashUserClass.MainAccountID;
            }
            set
            {
                this.GashUserClass.MainAccountID = value;
            }
        }

        /// <summary>
        /// Gash Region
        /// </summary>
        public string GashRegion
        {
            get
            {
                if (this.GashUserClass.Region.Length != 2) this.AlertOrRedirect();

                bool isMatch = (this.GashRegionCountry.Count == 0) ? true : false;

                foreach (string s in this.GashRegionCountry)
                {
                    if (this.GashUserClass.Region.ToLower() == s.ToLower()) isMatch = true;
                }

                if (isMatch == false) this.AlertOrRedirect();

                return this.GashUserClass.Region;
            }

            set
            {
                this.GashUserClass.Region = value;
            }
        }

        /// <summary>
        /// Service Code
        /// </summary>
        public string ServiceCode
        {
            get
            {
                string result = (string)ConfigurationManager.AppSettings["ServiceCode"] ?? string.Empty;

                if (string.IsNullOrEmpty(result))
                {
                    HttpContext.Current.Response.Write("ServiceCode設定有誤");
                    HttpContext.Current.Response.End();
                }

                return result;
            }
        }

        /// <summary>
        /// Service Region
        /// </summary>
        public string ServiceRegion
        {
            get
            {
                string result = (string)ConfigurationManager.AppSettings["ServiceRegion"] ?? string.Empty;

                if (string.IsNullOrEmpty(result))
                {
                    HttpContext.Current.Response.Write("ServiceRegion設定有誤");
                    HttpContext.Current.Response.End();
                }

                return result;
            }
        }

        /// <summary>
        /// GashV3.5 User Object
        /// </summary>
        public BaseGashLogin.GashUserData GashUserClass
        {
            get
            {
                if (Code.GetSession<BaseGashLogin.GashUserData>("CommonGashUserData") == null) this.AlertOrRedirect();
                return Code.GetSession<BaseGashLogin.GashUserData>("CommonGashUserData");
            }
        }

        /// <summary>
        /// GashV3.5 Game Account DisplayName
        /// </summary>
        public string GameAccountDisplayName
        {
            get
            {
                return this.GashUserClass.ServiceAccountDisplayName;
            }
        }

        /// <summary>
        /// 該遊戲帳號是否已登入過
        /// </summary>
        /// <param name="gameAccount"></param>
        /// <returns></returns>
        public static bool IsAccountExist(string gameAccount)
        {
            if (HttpContext.Current.Session["_ExistGameAccount"] == null)
            {
                HttpContext.Current.Session["_ExistGameAccount"] = gameAccount;
                return false;
            }

            return (HttpContext.Current.Session["_ExistGameAccount"].ToString().ToLower() == gameAccount.ToLower()) ? false : true;
        }
    }
}
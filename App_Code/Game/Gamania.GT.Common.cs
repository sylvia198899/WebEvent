using System;
using System.Configuration;
using System.Data;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web;
using Gamania.SD.DataAccess;
using Gamania.SD.Library;

namespace Gamania.GT
{
    /// <summary>
    /// Gamania 的摘要描述
    /// </summary>
    public class Common
    {
        /// <summary>
        /// 伺服器列表
        /// </summary>
        [DataContract]
        private class GameServerList
        {
            /// <summary>
            /// 伺服器的節點
            /// </summary>
            [DataMember(Order = 0)]
            public GameServer[] gameArea { get; set; }
            /// <summary>
            /// 回傳值
            /// </summary>
            [DataMember(Order = 1)]
            public int ret { get; set; }
        }

        /// <summary>
        /// 伺服器資料
        /// </summary>
        [DataContract]
        private class GameServer
        {
            /// <summary>
            /// 開始遊戲的URL
            /// </summary>
            [DataMember(Order = 0)]
            public string gameUrl { get; set; }
            /// <summary>
            /// 伺服器gameserver的內網IP
            /// </summary>
            [DataMember(Order = 1)]
            public string ip { get; set; }
            /// <summary>
            /// 伺服器名稱
            /// </summary>
            [DataMember(Order = 2)]
            public string name { get; set; }
            /// <summary>
            /// 伺服器的開始時間
            /// </summary>
            [DataMember(Order = 3)]
            public string openDate { get; set; }
            /// <summary>
            /// 伺服器的排序
            /// </summary>
            [DataMember(Order = 4)]
            public int sortNum { get; set; }
        }

        /// <summary>
        /// 角色資料
        /// </summary>
        [DataContract]
        private class GameCharacterInfo
        {
            /// <summary>
            /// 伺服器名稱
            /// </summary>
            [DataMember(Order = 0)]
            public string gameArea { get; set; }
            /// <summary>
            /// 角色名稱
            /// </summary>
            [DataMember(Order = 1)]
            public string qAccount { get; set; }
            /// <summary>
            /// 角色所屬區(台港澳)
            /// </summary>
            [DataMember(Order = 2)]
            public string qAccountArea { get; set; }
            /// <summary>
            /// 回傳值
            /// </summary>
            [DataMember(Order = 3)]
            public int ret { get; set; }
            /// <summary>
            /// 平台帳號ID
            /// </summary>
            [DataMember(Order = 4)]
            public string serviceAccountId { get; set; }
        }

        /// <summary>
        /// 儲值回傳資料
        /// </summary>
        [DataContract]
        private class InsertGameCoinInfo
        {
            /// <summary>
            /// 平台扣點數量
            /// </summary>
            [DataMember(Order = 0)]
            public int bfGold { get; set; }
            /// <summary>
            /// 營運商平台扣點流水ID
            /// </summary>
            [DataMember(Order = 1)]
            public int bfOrderNo { get; set; }
            /// <summary>
            /// 角色所在遊戲區
            /// </summary>
            [DataMember(Order = 2)]
            public string gameArea { get; set; }
            /// <summary>
            /// 帳號名稱
            /// </summary>
            [DataMember(Order = 3)]
            public string qAccount { get; set; }
            /// <summary>
            /// 帳號區域
            /// </summary>
            [DataMember(Order = 4)]
            public string qAccountArea { get; set; }
            /// <summary>
            /// 加值元寶數
            /// </summary>
            [DataMember(Order = 5)]
            public int qGold { get; set; }
            /// <summary>
            /// 加值元寶流水ID
            /// </summary>
            [DataMember(Order = 6)]
            public int qOrderNo { get; set; }
            /// <summary>
            /// 回傳值
            /// </summary>
            [DataMember(Order = 7)]
            public int ret { get; set; }
            /// <summary>
            /// 平台帳號ID
            /// </summary>
            [DataMember(Order = 8)]
            public string serviceAccountId { get; set; }
        }

        [DataContract]
        private class AddResourceInfo
        {
            /// <summary> 調用訊息 </summary>
            [DataMember(Order = 0)]
            public string msg { get; set; }

            /// <summary> 返回值 </summary>
            [DataMember(Order = 1)]
            public int ret { get; set; }
        }

        //所需的參數
        private static string WebIP = Code.GetString(ConfigurationManager.AppSettings["Md5WebIP"], "10.100.1.141");
        private static string thirdId = Code.GetString(ConfigurationManager.AppSettings["ThirdPartyID"], "AliasProvider00004");
        private static string key = Code.GetString(ConfigurationManager.AppSettings["Key"], "G4LWFvniSFact4sd7Q15dfP0T6D5");

        /// <summary>
        /// 選擇伺服器
        /// </summary>
        /// <returns>Table格式：ServerName,ServerID</returns>
        public static DataTable Select_Servers(int intTest, out string errorMessage)
        {
            errorMessage = string.Empty;
            DataTable dt = new DataTable();
            WebClient CVSclient = new WebClient();
            //string strCVSUrl = string.Format("http://10.100.52.105:80/bf_ws/bfservice?action=getGameAreaList&hmacStr={0}", MD5encode(thirdId + key + WebIP));
            string strGTUrl = getGTUrl();
            string strCVSUrl = string.Format(strGTUrl + "action=getGameAreaList&hmacStr={0}", MD5encode(thirdId + key + WebIP));
            try
            {
                byte[] bResult = CVSclient.DownloadData(strCVSUrl);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(bResult);
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GameServerList));
                GameServerList temp = (GameServerList)serializer.ReadObject(ms);
                if (temp.ret == 1 && temp.gameArea.Length > 0)
                {
                    dt.Columns.Add("ServerName", System.Type.GetType("System.String"));
                    dt.Columns.Add("ServerID", System.Type.GetType("System.String"));
                    //取得DB的伺服器資料
                    DataTable server_DT = Get_Server_Name_From_DB(intTest, out errorMessage);
                    if (string.IsNullOrEmpty(errorMessage) && server_DT.Rows.Count > 0)
                    {
                        foreach (GameServer gs in temp.gameArea)
                        {
                            DataRow[] temp_DR = server_DT.Select("ServerID ='" + gs.name + "'");
                            if (temp_DR.Length > 0)
                            {
                                DataRow dr = dt.NewRow();
                                dr["ServerName"] = temp_DR[0]["ServerName"].ToString();
                                dr["ServerID"] = gs.name;
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                }
                else
                {
                    errorMessage = "呼叫WS失敗:" + thirdId + key + WebIP + "，WS:" + strCVSUrl;
                    dt = null;
                }
            }
            catch (Exception ex)
            {
                dt = null;
                errorMessage = ex.Message + strCVSUrl.ToString();
            }
            finally
            {
                CVSclient.Dispose();
            }

            return dt;
        }

        /// <summary>
        /// 取得角色資料
        /// </summary>
        /// <param name="GameAccount"></param>
        /// <param name="serviceCode"></param>
        /// <param name="serviceRegion"></param>
        /// <param name="GashRegion"></param>
        /// <param name="ServerName"></param>
        /// <param name="errorMessage"></param>
        /// <returns>qAccount:角色名稱;qAccountArea:角色所在區</returns>
        public static DataTable Get_Character_Info(string GameAccount, string serviceCode, string serviceRegion, string GashRegion, string ServerID, out string errorMessage, out string AliasAccountID)
        {
            AliasAccountID = string.Empty;
            errorMessage = string.Empty;
            DataTable dt = new DataTable();
            if (GashRegion.ToUpper() == "TW") AliasAccountID = Get_AliasAccountID_TW(GameAccount, serviceCode, serviceRegion, out errorMessage);
            else if (GashRegion.ToUpper() == "HK") AliasAccountID = Get_AliasAccountID_HK(GameAccount, serviceCode, serviceRegion, out errorMessage);
            if (string.IsNullOrEmpty(AliasAccountID) || !string.IsNullOrEmpty(errorMessage))
                return dt;
            WebClient CVSclient = new WebClient();
            //string strCVSUrl = string.Format("http://10.100.52.105:80/bf_ws/bfservice?action=getAccountInfo&serviceAccountId={0}&&gameArea={1}&hmacStr={2}", AliasAccountID, ServerID, MD5encode(AliasAccountID + ServerID + thirdId + key + WebIP));
            string strGTUrl = getGTUrl();
            string strCVSUrl = string.Format(strGTUrl + "action=getAccountInfo&serviceAccountId={0}&gameArea={1}&hmacStr={2}", AliasAccountID, ServerID, MD5encode(AliasAccountID + ServerID + thirdId + key + WebIP));
            try
            {
                byte[] bResult = CVSclient.DownloadData(strCVSUrl);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(bResult);
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GameCharacterInfo));
                GameCharacterInfo temp = serializer.ReadObject(ms) as GameCharacterInfo;
                if (temp.ret == 1 && temp != null)
                {
                    dt.Columns.Add("qAccount", System.Type.GetType("System.String"));
                    dt.Columns.Add("qAccountArea", System.Type.GetType("System.String"));
                    DataRow dr = dt.NewRow();
                    dr["qAccount"] = temp.qAccount;
                    dr["qAccountArea"] = temp.qAccountArea;
                    dt.Rows.Add(dr);
                }
                else
                {
                    //errorMessage = "呼叫WS失敗，URL:" + strCVSUrl;
                    dt = null;
                }
            }
            catch (Exception ex)
            {
                dt = null;
                errorMessage = ex.Message;
            }
            finally
            {
                CVSclient.Dispose();
            }

            return dt;
        }

        /// <summary>
        /// 存入遊戲幣
        /// </summary>
        /// <param name="aliasAccountID">異業帳號</param>
        /// <param name="serviceCode"></param>
        /// <param name="serviceRegion"></param>
        /// <param name="GashRegion">玩家地區</param>
        /// <param name="qAccount">角色名稱</param>
        /// <param name="qAccountArea">玩家地區</param>
        /// <param name="bfOrderNo">Log流水編號</param>
        /// <param name="ServerName">伺服器ID</param>
        /// <param name="bfGold">Gash扣點數</param>
        /// <param name="qGold">元寶增加數</param>
        /// <param name="imprestType">轉點格式：1=正常轉點,2=活動送的元寶,3=後台添加</param>
        /// <param name="errorMessage"></param>
        /// <returns>1=成功, 0=WS回傳失敗, -1=其他失敗, -2=儲值與回傳值不同, -3=取得異業遊戲帳號失敗</returns>
        public static int Insert_Game_Coin(string aliasAccountID, string serviceCode, string serviceRegion, string GashRegion, string qAccount, string qAccountArea, int bfOrderNo, string ServerID, int bfGold, int qGold, int imprestType, out string errorMessage)
        {
            int _intreturn = -1;
            errorMessage = string.Empty;
            WebClient CVSclient = new WebClient();
            object[] o = { aliasAccountID, HttpUtility.UrlEncode(qAccount, System.Text.Encoding.UTF8), qAccountArea, bfOrderNo, ServerID, bfGold, qGold, imprestType, MD5encode(aliasAccountID + qAccount + qAccountArea + bfOrderNo + ServerID + bfGold + qGold + imprestType + thirdId + key + WebIP) };
            //string strCVSUrl = string.Format("http://10.100.52.105:80/bf_ws/bfservice?action=imprestAccount&serviceAccountId={0}&qAccount={1}&qAccountArea={2}&bfOrderNo={3}&gameArea={4}&bfGold={5}&qGold={6}&imprestType={7}&hmacStr={8}", o);
            string strGTUrl = getGTUrl();
            string strCVSUrl = string.Format(strGTUrl + "action=imprestAccount&serviceAccountId={0}&qAccount={1}&qAccountArea={2}&bfOrderNo={3}&gameArea={4}&bfGold={5}&qGold={6}&imprestType={7}&hmacStr={8}", o);
            try
            {
                byte[] bResult = CVSclient.DownloadData(strCVSUrl);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(bResult);
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(InsertGameCoinInfo));
                InsertGameCoinInfo temp = (InsertGameCoinInfo)serializer.ReadObject(ms);
                if (temp.ret == 1)
                {
                    if (bfGold == temp.bfGold && qGold == temp.qGold)
                        _intreturn = temp.ret;
                    else
                    {
                        errorMessage = "儲值與回傳值不同。傳回值:" + temp.ret;
                        _intreturn = -2;
                    }
                }
                else
                {
                    errorMessage = "呼叫WS失敗，傳回" + temp.ret + strCVSUrl + ";" + aliasAccountID + qAccount + qAccountArea + bfOrderNo + ServerID + bfGold + qGold + imprestType + thirdId + key + WebIP;
                    _intreturn = temp.ret;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            finally
            {
                CVSclient.Dispose();
            }

            return _intreturn;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        private static string MD5encode(string inputString)
        {
            byte[] data = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(inputString);
            return BitConverter.ToString(new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(data)).Replace("-", "").ToLower();
        }

        /// <summary>
        /// 取得異業遊戲帳號(台灣)
        /// </summary>
        /// <param name="gameAccount"></param>
        /// <param name="serviceCode"></param>
        /// <param name="serviceRegion"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private static string Get_AliasAccountID_TW(string gameAccount, string serviceCode, string serviceRegion, out string errMsg)
        {
            errMsg = string.Empty;
            GameAdapter ga_WS = new GameAdapter();
            Result result = new Result();
            result = ga_WS.ThirdParty_GetAliasAccountID(gameAccount, serviceCode, serviceRegion);
            if (Code.GetInt(result.Code, 0) == 1000)
                return result.Message;
            else
            {
                errMsg = result.Message;
                return "";
            }
        }

        private static string Get_AliasAccountID_HK(string gameAccount, string serviceCode, string serviceRegion, out string errMsg)
        {
            errMsg = string.Empty;
            HKGameAdapter.GameAdapter ga_WS = new HKGameAdapter.GameAdapter();
            HKGameAdapter.Result result = new HKGameAdapter.Result();
            result = ga_WS.ThirdParty_GetAliasAccountID(gameAccount, serviceCode, serviceRegion);
            if (Code.GetInt(result.Code, 0) == 1000)
                return result.Message;
            else
            {
                errMsg = result.Message;
                return "";
            }
        }

        private static DataTable Get_Server_Name_From_DB(int intTest, out string errMsg)
        {
            errMsg = string.Empty;
            string strSQL = string.Empty;
            MSSqlDataAccess mss = new MSSqlDataAccess("GTEvent");
            mss.CommandType = CommandType.Text;
            mss.CommandTimeout = 90;
            strSQL = @"SELECT ServerID,ServerName FROM dbo.ServerList WITH(NOLOCK)";
            if (intTest == 1)
            { strSQL += " WHERE Flag = 1"; }

            DataTable dt = mss.ExecuteReader(strSQL);
            if (mss.HasException)
                errMsg = mss.ExceptionText;
            return dt;
        }

        /// <summary> 取得異業遊戲帳號 (台灣/香港)</summary>
        /// <param name="GashRegion"></param>
        /// <param name="gameAccount"></param>
        /// <param name="serviceCode"></param>
        /// <param name="serviceRegion"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static string Get_AliasAccountID(string sGashRegion, string sGameAccount, string sServiceCode, string sServiceRegion, out string oErrMsg)
        {
            string retValue = string.Empty;
            oErrMsg = string.Empty;
            switch (sGashRegion)
            {
                case "TW":
                    retValue = Get_AliasAccountID_TW(sGameAccount, sServiceCode, sServiceRegion, out oErrMsg);
                    break;
                case "HK":
                    retValue = Get_AliasAccountID_HK(sGameAccount, sServiceCode, sServiceRegion, out oErrMsg);
                    break;
                default:
                    retValue = "";
                    oErrMsg = "ServiceRegion錯誤";
                    break;
            }
            return retValue;
        }

        #region 配寶-資源類
        /// <summary> 配寶-資源類 </summary>
        /// <param name="sRoleNames">玩家角色名称，支持多角色，以分号;分隔</param>
        /// <param name="iMoney">金币 配給數量</param>
        /// <param name="iFood">粮食 配給數量</param>
        /// <param name="iWood">木材 配給數量</param>
        /// <param name="iGold">元宝 配給數量</param>
        /// <param name="iBattleCommand">虎符 配給數量</param>
        /// <param name="iSoulAmount">功勋 配給數量</param>
        /// <param name="iRecruitCount">兵数 配給數量</param>
        /// <param name="iRep">声望 配給數量</param>
        /// <param name="iVipPoint">积分 配給數量</param>
        /// <param name="sServerID">伺服器</param>
        /// <param name="iLogID">LogID</param>
        /// <param name="sSource">配寶來源</param>
        /// <param name="sErrMsg">錯誤訊息</param>
        /// <returns> 1:配寶成功 ; 0:配寶失敗 ; -1:例外情況 </returns>
        public static int Add_ResourceToRoles(string sRoleNames, int iMoney, int iFood, int iWood, int iGold, int iBattleCommand, int iSoulAmount, int iRecruitCount, int iRep, int iVipPoint, string sServerID, int iLogID, string sSource, out string sErrMsg)
        {
            int iReturn = 0;
            sErrMsg = string.Empty;
            string sEncodeRoleName = HttpUtility.UrlEncode(sRoleNames, System.Text.Encoding.UTF8);
            string sHmacStr = MD5encode(sRoleNames + iMoney + iFood + iWood + iGold + iBattleCommand + iSoulAmount + iRecruitCount + iRep + iVipPoint + sServerID + thirdId + key + WebIP);

            WebClient CVSclient = new WebClient();
            object[] o = { sEncodeRoleName, iMoney, iFood, iWood, iGold, iBattleCommand, iSoulAmount, iRecruitCount, iRep, iVipPoint, sServerID, iLogID, sSource, sHmacStr };
            string strGTUrl = getGTUrl();
            string strCVSUrl = string.Format(strGTUrl + "action=addResource&roleNames={0}&money={1}&food={2}&wood={3}&gold={4}&battleCommand={5}&soulAmount={6}&recruitCount={7}&rep={8}&vipPoint={9}&gameArea={10}&runningNum={11}&source={12}&hmacStr={13}", o);
            try
            {
                byte[] bResult = CVSclient.DownloadData(strCVSUrl);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(bResult);
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AddResourceInfo));
                AddResourceInfo temp = (AddResourceInfo)serializer.ReadObject(ms);
                //回傳值僅：1=调用成功 | 0=调用失败
                iReturn = temp.ret;

                if (temp.ret != 1)
                    sErrMsg = "呼叫配資源WS失敗";
            }
            catch (Exception ex)
            {
                iReturn = -1;
                sErrMsg = "呼叫WS失敗，Exception:" + ex.Message;
            }
            finally { CVSclient.Dispose(); }

            return iReturn;
        }
        #endregion

        #region 配寶-道具類
        /// <summary> 配寶-道具類 </summary>
        /// <param name="sRoleNames">玩家角色名称，支持多角色，以分号;分隔</param>
        /// <param name="sItemInfo">道具信息，格式为 道具ID,道具数量 添加多个的话，则以分号分隔</param>
        /// <param name="sServerID">伺服器ID</param>
        /// <param name="iLogID">LogID</param>
        /// <param name="sSource">配寶來源</param>
        /// <param name="sErrMsg">錯誤訊息</param>
        /// <returns> 1:配寶成功 ; 0:配寶失敗 ; -1:例外情況 </returns>
        public static int Add_ItemToRoles(string sRoleNames, string sItemInfo, string sServerID, int iLogID, string sSource, out string sErrMsg)
        {
            int iReturn = 0;
            sErrMsg = string.Empty;
            string sEncodeRoleName = HttpUtility.UrlEncode(sRoleNames, System.Text.Encoding.UTF8);
            string sHmacStr = MD5encode(sRoleNames + sItemInfo + sServerID + thirdId + key + WebIP);

            WebClient CVSclient = new WebClient();
            object[] o = { sEncodeRoleName, sItemInfo, sServerID, iLogID, sSource, sHmacStr };
            string strGTUrl = getGTUrl();
            string strCVSUrl = string.Format(strGTUrl + "action=addItem&roleNames={0}&itemInfo={1}&gameArea={2}&runningNum={3}&source={4}&hmacStr={5}", o);
            try
            {
                byte[] bResult = CVSclient.DownloadData(strCVSUrl);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(bResult);
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AddResourceInfo));
                AddResourceInfo temp = (AddResourceInfo)serializer.ReadObject(ms);

                //回傳值僅：1=调用成功 | 0=调用失败
                iReturn = temp.ret;

                if (temp.ret != 1)
                    sErrMsg = "呼叫配道具WS失敗";
            }
            catch (Exception ex)
            {
                iReturn = -1;
                sErrMsg = "呼叫WS失敗，Exception:" + ex.Message;
            }
            finally { CVSclient.Dispose(); }

            return iReturn;
        }
        #endregion

        /// <summary> 取得(正式/測試)url </summary>
        /// <returns></returns>
        private static string getGTUrl()
        {
            string sGTUrl = string.Empty;
            if (ConfigurationManager.AppSettings["IsaphaFlag"].ToString() == "0")
                sGTUrl = "http://10.100.52.105:80/bf_ws/bfservice?";
            else
                sGTUrl = "http://210.208.90.46:8080/bf_ws/bfservice?";

            return sGTUrl;
        }
    }
}
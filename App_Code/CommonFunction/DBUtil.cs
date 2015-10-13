using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;


/// <summary>
/// SD 公用函式
/// </summary>
namespace SDCodeCheck
{
    /// <summary>
    /// 跟DB操作相關的函式
    /// </summary>
    public class DBUtil
    {
        #region //回傳Web.Config的ConnectionString
        /// <summary>
        /// 回傳Web.Config的ConnectionString
        /// </summary>
        /// <param name="strConnstrName">Connection String的Name</param>        
        /// <returns></returns>
        public static string GetConnStr(string strConnstrName)
        {
            return ConfigurationManager.ConnectionStrings[strConnstrName].ConnectionString;
        }
        #endregion

        #region //GetRows// 輸入的SqlDataReader,用ArrayList 方式取回全部資料
        /// <summary>
        /// 輸入的SqlDataReader,用ArrayList 方式取回全部資料
        /// </summary>
        /// <param name="SDR">SqlDataReader Source</param>        
        /// <returns></returns>
        public static ArrayList GetRows(SqlDataReader SDR)
        {

            ArrayList myResults = new ArrayList();
            object[] objOneRow;
            while (SDR.Read())
            {
                objOneRow = (object[])Array.CreateInstance(typeof(object), SDR.FieldCount);
                SDR.GetValues(objOneRow);
                myResults.Add(objOneRow);
            }
            return myResults;

        }
        #endregion
    }
}

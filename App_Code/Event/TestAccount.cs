using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

/// <summary>
/// TestAccount 的摘要描述
/// </summary>
public class TestAccount
{

    public static string TestAccountDT(string str_Guid)
    {
        string RetValue = "";
        using (SqlConnection _conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TestAccountConnectionString"].ToString()))
        {
            SqlCommand _cmd = new SqlCommand();
            _cmd.Connection = _conn;
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "[QueryTestAccountDT]";
            _cmd.Parameters.Clear();
            _cmd.Parameters.Add("@Guid", SqlDbType.VarChar).Value = str_Guid;
            _cmd.Parameters.Add("@StartDT", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output;
            _cmd.Parameters.Add("@EndDT", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output;
            _cmd.Parameters.Add("@ReturnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
            try
            {
                if (_cmd.Connection.State != ConnectionState.Open) { _cmd.Connection.Open(); }
                _cmd.ExecuteNonQuery();
                RetValue = _cmd.Parameters["@ReturnValue"].Value.ToString();// 1:成功 -1:失敗
                if (RetValue == "1")
                {
                    RetValue = "1;" + _cmd.Parameters["@StartDT"].Value.ToString() + ";" + _cmd.Parameters["@EndDT"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);

                HttpContext.Current.Response.End();
            }
            finally
            {
                if (_cmd.Connection.State != ConnectionState.Closed) { _cmd.Connection.Close(); }
                _cmd.Dispose();
                _conn.Close();
                _conn.Dispose();
            }
        }
        return RetValue;
    }

    //*****取得帳號密碼****//
    public static string GetTestAccount(string area)
    {
        string str_Guid = ConfigurationManager.AppSettings["TestAccountGuid"];
        string RetValue = "";
        using (SqlConnection _conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TAConnectionString"].ToString()))
        {
            SqlCommand _cmd = new SqlCommand();
            _cmd.Connection = _conn;
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "[SP_TestAccount]";
            _cmd.Parameters.Clear();
            _cmd.Parameters.Add("@Guid", SqlDbType.VarChar).Value = str_Guid;
            _cmd.Parameters.Add("@area", SqlDbType.VarChar).Value = area;
            _cmd.Parameters.Add("@testaccount", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output;
            _cmd.Parameters.Add("@testpwd", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output;
            _cmd.Parameters.Add("@ReturnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
            try
            {
                if (_cmd.Connection.State != ConnectionState.Open) { _cmd.Connection.Open(); }
                _cmd.ExecuteNonQuery();
                RetValue = _cmd.Parameters["@ReturnValue"].Value.ToString();
                if (RetValue == "1")
                {
                    RetValue = "1;" + _cmd.Parameters["@testaccount"].Value.ToString() + ";" + _cmd.Parameters["@testpwd"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);

                HttpContext.Current.Response.End();
            }
            finally
            {
                if (_cmd.Connection.State != ConnectionState.Closed) { _cmd.Connection.Close(); }
                _cmd.Dispose();
                _conn.Close();
                _conn.Dispose();
            }
        }
        return RetValue;
    }


    //*****取得帳號密碼(有EMail條件)****//
    public static string GetTestAccountEMail(string area, string EMail)
    {
        string str_Guid = ConfigurationManager.AppSettings["TestAccountGuid"];
        string RetValue = "";
        using (SqlConnection _conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TAConnectionString"].ToString()))
        {
            SqlCommand _cmd = new SqlCommand();
            _cmd.Connection = _conn;
            _cmd.CommandType = CommandType.StoredProcedure;
            _cmd.CommandText = "[SP_TestAccountEMail]";
            _cmd.Parameters.Clear();
            _cmd.Parameters.Add("@Guid", SqlDbType.VarChar).Value = str_Guid;
            _cmd.Parameters.Add("@EMail", SqlDbType.VarChar).Value = EMail;
            _cmd.Parameters.Add("@area", SqlDbType.VarChar).Value = area;
            _cmd.Parameters.Add("@testaccount", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output;
            _cmd.Parameters.Add("@testpwd", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output;
            _cmd.Parameters.Add("@ReturnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
            try
            {
                if (_cmd.Connection.State != ConnectionState.Open) { _cmd.Connection.Open(); }
                _cmd.ExecuteNonQuery();
                RetValue = _cmd.Parameters["@ReturnValue"].Value.ToString();
                if (RetValue == "1")
                {
                    RetValue = "1;" + _cmd.Parameters["@testaccount"].Value.ToString() + ";" + _cmd.Parameters["@testpwd"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);

                HttpContext.Current.Response.End();
            }
            finally
            {
                if (_cmd.Connection.State != ConnectionState.Closed) { _cmd.Connection.Close(); }
                _cmd.Dispose();
                _conn.Close();
                _conn.Dispose();
            }
        }
        return RetValue;
    }

    //*****取得帳號密碼(有EMail、居住縣市條件)****//
    public static string GetTestAccountEMailCity(string area, string EMail,string RealName,string City)
    {
        string str_Guid = ConfigurationManager.AppSettings["TestAccountGuid"];
        string RetValue = "";
        using (SqlConnection _conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TAConnectionString"].ToString()))
        {
            SqlCommand _cmd = new SqlCommand();
            _cmd.Connection = _conn;
            _cmd.CommandType = CommandType.StoredProcedure;
           // _cmd.CommandText = "[SP_TestAccountEMail]";
            _cmd.CommandText = "[SP_TestAccountEMailCity]";
            _cmd.Parameters.Clear();
            _cmd.Parameters.Add("@Guid", SqlDbType.VarChar).Value = str_Guid;
            _cmd.Parameters.Add("@EMail", SqlDbType.VarChar).Value = EMail;
            _cmd.Parameters.Add("@area", SqlDbType.VarChar).Value = area;
            _cmd.Parameters.Add("@RealName", SqlDbType.NVarChar, 20).Value = RealName;
            _cmd.Parameters.Add("@City", SqlDbType.VarChar,3).Value = City;
            _cmd.Parameters.Add("@testaccount", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output;
            _cmd.Parameters.Add("@testpwd", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output;
            _cmd.Parameters.Add("@ReturnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
            try
            {
                if (_cmd.Connection.State != ConnectionState.Open) { _cmd.Connection.Open(); }
                _cmd.ExecuteNonQuery();
                RetValue = _cmd.Parameters["@ReturnValue"].Value.ToString(); // 1:成功 2:帳號已發完 3:帳號密為空白 4:已兌換過　9:寫入失敗
                if (RetValue == "1")
                {
                    RetValue = "1;" + _cmd.Parameters["@testaccount"].Value.ToString() + ";" + _cmd.Parameters["@testpwd"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);

                HttpContext.Current.Response.End();
            }
            finally
            {
                if (_cmd.Connection.State != ConnectionState.Closed) { _cmd.Connection.Close(); }
                _cmd.Dispose();
                _conn.Close();
                _conn.Dispose();
            }
        }
        return RetValue;
    }
}

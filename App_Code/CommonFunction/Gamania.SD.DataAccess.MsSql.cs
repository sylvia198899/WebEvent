using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Configuration;

namespace Gamania.SD.DataAccess
{
    /// <summary>
    /// MsSql DataAccess
    /// </summary>
    /// <remarks>2009.04.08 version</remarks>
    public class MSSqlDataAccess : IDataAccess
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MSSqlDataAccess"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string. 1.connection key name 2.connectin text </param>
        public MSSqlDataAccess(string connectionString)
        {
            this.CommandType = CommandType.Text;

            if (WebConfigurationManager.ConnectionStrings[connectionString] != null)
                this.ConnectionString = WebConfigurationManager.ConnectionStrings[connectionString].ToString();
            else
                this.ConnectionString = connectionString;
        }

        /// <summary>
        /// ExecuteDataSet
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
        #region
        public override DataSet ExecuteDataSet(string commandText)
        {
            return this.ExecuteDataSet(commandText, new SqlParameter[0]);
        }
        #endregion

        /// <summary>
        /// ExecuteDataSet
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameterArray">The parameter array.</param>
        /// <returns></returns>
        #region
        public DataSet ExecuteDataSet(string commandText, SqlParameter[] parameterArray)
        {
            DataSet ds = new DataSet();
            ds.Locale = base.cultureInfo;

            SqlConnection cn = null;
            SqlCommand cmd = null;
            SqlDataAdapter sda =  null;

            try
            {
                cn = new SqlConnection(this.connectionString);
                cmd = new SqlCommand(commandText, cn);
                cmd.CommandType = this.CommandType;
                cmd.CommandTimeout = this.CommandTimeout;
                cmd.Parameters.AddRange(parameterArray);

                sda = new SqlDataAdapter(cmd);
                sda.Fill(ds, "gamania");
            }
            catch (SqlException ex)
            {
                this.CatchException(ex);
            }
            catch (InvalidOperationException ex)
            {
                base.CatchException(ex);
            }
            catch (Exception ex)
            {
                base.CatchException(ex);
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (cn != null) cn.Close();
                if (sda != null) sda.Dispose();
            }

            return ds;
        }
        #endregion

        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameterArray">The parameter array.</param>
        /// <returns></returns>
        #region
        public DataTable ExecuteReader(string commandText, SqlParameter[] parameterArray)
        {
            DataTable dt = new DataTable();
            dt.Locale = base.cultureInfo;

            SqlConnection cn = null;
            SqlCommand cmd = null;

            try
            {
                cn = new SqlConnection(this.connectionString);
                cmd = new SqlCommand(commandText, cn);
                cmd.CommandType = this.CommandType;
                cmd.CommandTimeout = this.CommandTimeout;
                cmd.Parameters.AddRange(parameterArray);

                cn.Open();
                dt.Load(cmd.ExecuteReader(CommandBehavior.CloseConnection));
            }
            catch (SqlException ex)
            {
                this.CatchException(ex);
            }
            catch (InvalidOperationException ex)
            {
                base.CatchException(ex);
            }
            catch (Exception ex)
            {
                base.CatchException(ex);
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (cn != null) cn.Close();
            }

            return dt;
        }
        #endregion

        #region
        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameterArray">The parameter array.</param>
        /// <param name="returnParameter">The return array</param>
        /// <returns></returns>
        public DataTable ExecuteReader(string commandText, SqlParameter[] parameterArray, out SqlParameterCollection returnParameter)
        {
            DataTable dt = new DataTable();
            dt.Locale = base.cultureInfo;

            SqlConnection cn = null;
            SqlCommand cmd = null;

            returnParameter = null;

            try
            {
                cn = new SqlConnection(this.connectionString);
                cmd = new SqlCommand(commandText, cn);
                cmd.CommandType = this.CommandType;
                cmd.CommandTimeout = this.CommandTimeout;
                cmd.Parameters.AddRange(parameterArray);

                cn.Open();
                dt.Load(cmd.ExecuteReader(CommandBehavior.CloseConnection));

                returnParameter = cmd.Parameters;
            }
            catch (SqlException ex)
            {
                this.CatchException(ex);
            }
            catch (InvalidOperationException ex)
            {
                base.CatchException(ex);
            }
            catch (Exception ex)
            {
                base.CatchException(ex);
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (cn != null) cn.Close();
            }

            return dt;
        }
        #endregion

        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
        #region
        public override DataTable ExecuteReader(string commandText)
        {
            return this.ExecuteReader(commandText, new SqlParameter[0]);
        }
        #endregion

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameterArray">The parameter array.</param>
        /// <returns></returns>
        #region
        public SqlParameterCollection ExecuteNonQuery(string commandText, SqlParameter[] parameterArray)
        {
            SqlParameterCollection spc = null;
            SqlConnection cn = null;
            SqlCommand cmd = null;

            try
            {
                cn = new SqlConnection(this.connectionString);
                cmd = new SqlCommand(commandText, cn);
                cmd.CommandType = this.CommandType;
                cmd.CommandTimeout = this.CommandTimeout;
                cmd.Parameters.AddRange(parameterArray);

                cn.Open();
                cmd.ExecuteNonQuery();
                spc = cmd.Parameters;
            }
            catch (SqlException ex)
            {
                this.CatchException(ex);
            }
            catch (InvalidOperationException ex)
            {
                base.CatchException(ex);
            }
            catch (Exception ex)
            {
                base.CatchException(ex);
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (cn != null) cn.Close();
            }

            return spc;
        }
        #endregion

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
        #region
        public override int ExecuteNonQuery(string commandText)
        {
            int _return = 0;
            SqlConnection cn = null;
            SqlCommand cmd = null;

            try
            {
                cn = new SqlConnection(this.connectionString);
                cmd = new SqlCommand(commandText, cn);
                cmd.CommandType = this.CommandType;
                cmd.CommandTimeout = this.CommandTimeout;

                cn.Open();
                _return = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                this.CatchException(ex);
            }
            catch (InvalidOperationException ex)
            {
                base.CatchException(ex);
            }
            catch (Exception ex)
            {
                base.CatchException(ex);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (cn != null)
                    cn.Close();
            }

            return _return;
        }
        #endregion

        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameterArray">The parameter array.</param>
        /// <returns></returns>
        #region
        public Object ExecuteScalar(string commandText, SqlParameter[] parameterArray)
        {
            object _return = null;

            SqlConnection cn = null;
            SqlCommand cmd = null;

            try
            {
                cn = new SqlConnection(this.connectionString);
                cmd = new SqlCommand(commandText, cn);
                cmd.CommandType = this.CommandType;
                cmd.CommandTimeout = this.CommandTimeout;
                cmd.Parameters.AddRange(parameterArray);

                cn.Open();
                _return = cmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                this.CatchException(ex);
            }
            catch (InvalidOperationException ex)
            {
                base.CatchException(ex);
            }
            catch (Exception ex)
            {
                base.CatchException(ex);
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (cn != null) cn.Close();
            }

            return _return;
        }
        #endregion

        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <returns></returns>
        #region
        public override Object ExecuteScalar(string commandText)
        {
            return this.ExecuteScalar(commandText, new SqlParameter[0]);
        }
        #endregion

        /// <summary>
        /// 處理例外狀況
        /// </summary>
        /// <param name="exception">The exception.</param>
        #region
        protected void CatchException(SqlException exception)
        {
            base.ExceptionType = exception.GetType();
            base.ExceptionText = exception.Message.ToString();
            base.HasException = true;
        }
        #endregion
    }
}

namespace Gamania.SD.DataAccess
{
    /// <summary>
    /// DataAccess Abstract Class
    /// </summary>
    /// <remarks>2009.03.03 version</remarks>
    public abstract class IDataAccess
    {
        /// <summary>
        /// Realease Version
        /// </summary>
        public static string Version = "2009.03.03";

        /// <summary>
        /// DataTable CultureInfo
        /// </summary>
        internal CultureInfo cultureInfo = CultureInfo.InvariantCulture;

        /// <summary>
        /// Command Type
        /// </summary>
        internal CommandType commandType = CommandType.Text;

        /// <summary>
        /// WebConfig Connection KeyName / Connection String
        /// </summary>
        internal string connectionString = string.Empty;

        /// <summary>
        /// Command Text
        /// </summary>
        internal string commandText = string.Empty;

        /// <summary>
        /// Exception Text
        /// </summary>
        internal string exceptionText = string.Empty;

        /// <summary>
        /// Exception Type
        /// </summary>
        internal Type exceptionType;

        /// <summary>
        /// Has Exception
        /// </summary>
        internal bool hasException;

        /// <summary>
        /// Command Timeout
        /// </summary>
        internal int commandTimeout;

        /// <summary>
        /// 命令類型
        /// Command Type
        /// </summary>
        public CommandType CommandType
        {
            get { return this.commandType; }
            set { this.commandType = value; }
        }

        /// <summary>
        /// 連線Key名稱 / 連線字串
        /// WebConfig Connection KeyName / Connection String
        /// </summary>
        public string ConnectionString
        {
            get { return this.connectionString; }
            set { this.connectionString = value; }
        }

        /// <summary>
        /// 命令語言/預儲程序名
        /// Command Text / StoreProcedure Name        
        /// </summary>
        public string CommandText
        {
            get { return this.commandText; }
            set { this.commandText = value; }
        }

        /// <summary>
        /// 是否有例外狀況
        /// Has Exception
        /// </summary>
        public bool HasException
        {
            get { return this.hasException; }
            set { this.hasException = value; }
        }

        /// <summary>
        /// 例外狀況訊息
        /// Exception Text
        /// </summary>
        public string ExceptionText
        {
            get { return this.exceptionText; }
            set { this.exceptionText = value; }
        }

        /// <summary>
        /// 例外狀況型別
        /// Exception Type
        /// </summary>
        public Type ExceptionType
        {
            get { return this.exceptionType; }
            set { this.exceptionType = value; }
        }
        
        /// <summary>
        /// Command Timeout
        /// </summary>
        public int CommandTimeout
        {
            get { return this.commandTimeout; }
            set { this.commandTimeout = value; }
        }

        /// <summary>
        /// ExecuteDataSet
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public abstract DataSet ExecuteDataSet(string commandText);

        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public abstract DataTable ExecuteReader(string commandText);

        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public abstract object ExecuteScalar(string commandText);

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public abstract int ExecuteNonQuery(string commandText);

        /// <summary>
        /// 處理例外狀況
        /// </summary>
        /// <param name="exception">The exception.</param>
        #region
        protected void CatchException(Exception exception)
        {
            this.ExceptionType = exception.GetType();
            this.ExceptionText = exception.Message.ToString();
            this.HasException = true;
        }
        #endregion

        /// <summary>
        /// 處理例外狀況
        /// </summary>
        /// <param name="exception">The exception.</param>
        #region
        protected void CatchException(InvalidOperationException exception)
        {
            this.ExceptionType = exception.GetType();
            this.ExceptionText = exception.Message.ToString();
            this.HasException = true;
        }
        #endregion

        /// <summary>
        /// 建立Parameter物件,適用於輸入型別的Parameter
        /// Create Input Parameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="type">The type.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static T New<T>(string name, object value, ParameterDirection direction, DbType type, int size) where T : IDbDataParameter, IDataParameter, new()
        {
            T t = new T();
            t.ParameterName = name;
            t.Value = value;
            t.DbType = type;
            t.Size = size;
            t.Direction = direction;

            return t;
        }

        /// <summary>
        /// 建立Parameter物件,適用於輸入型別的Parameter
        /// Create Input Parameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static T New<T>(string name, object value, ParameterDirection direction, DbType type) where T : IDbDataParameter, IDataParameter, new()
        {
            T t = new T();
            t.ParameterName = name;
            t.Value = value;
            t.DbType = type;
            t.Direction = direction;

            return t;
        }

        /// <summary>
        /// 建立Parameter物件,適用於輸出或回傳型別的Parameter
        /// Create Output or Return Parameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="type">The type.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static T New<T>(string name, ParameterDirection direction, DbType type, int size) where T : IDbDataParameter, IDataParameter, new()
        {
            T t = new T();
            t.ParameterName = name;
            t.DbType = type;
            t.Size = size;
            t.Direction = direction;

            return t;
        }

        /// <summary>
        /// 建立Parameter物件,適用於輸出或回傳型別的Parameter
        /// Create Output or Return Parameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static T New<T>(string name, ParameterDirection direction, DbType type) where T : IDbDataParameter, IDataParameter, new()
        {
            T t = new T();
            t.ParameterName = name;
            t.DbType = type;
            t.Direction = direction;

            return t;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Data
    {
        /// <summary>
        /// 動態產生自訂的DataTable,欄位值為欄位名加上序號
        /// </summary>
        /// <param name="stringList">欄位名集合</param>
        /// <param name="rowTotal">資料筆數</param>
        /// <returns></returns>
        public static DataTable GetDataTable(List<string> stringList, int rowTotal)
        {
            DataTable dt = new DataTable("EMPTY");

            for (int i = -1; i < stringList.Count; i++)
            {
                if (i == -1)
                    dt.Columns.Add(new DataColumn("PKey", typeof(string)));
                else
                    dt.Columns.Add(new DataColumn(stringList[i], typeof(string)));
            }

            DataRow dr = null;

            for (int i = 1; i < rowTotal; i++)
            {
                dr = dt.NewRow();

                for (int j = 0; j <= stringList.Count; j++)
                {
                    dr[j] = (j == 0) ? i.ToString() : stringList[j - 1].ToString() + i.ToString();
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        /// <summary>
        /// 動態產生自訂的DataTable
        /// </summary>
        /// <param name="fieldList">欄位名集合</param>
        /// <param name="valueList">欄位值集合</param>
        /// <param name="rowTotal">資料筆數</param>
        /// <returns></returns>
        public static DataTable GetDataTable(List<string> fieldList, List<string> valueList, int rowTotal)
        {
            DataTable dt = new DataTable("EMPTY");

            for (int i = -1; i < fieldList.Count; i++)
            {
                if (i == -1)
                    dt.Columns.Add(new DataColumn("PKey", typeof(string)));
                else
                    dt.Columns.Add(new DataColumn(fieldList[i], typeof(string)));
            }

            DataRow dr = null;

            for (int i = 0; i < rowTotal; i++)
            {
                dr = dt.NewRow();

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    dr[j] = (j == 0) ? (i + 1).ToString() : valueList[j - 1].ToString();
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }
    }
}
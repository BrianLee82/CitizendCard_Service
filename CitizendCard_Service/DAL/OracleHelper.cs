using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OracleClient;

namespace CitizendCard_Service.DAL
{
    public class OracleHelper
    {
        private string myConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        public OracleConnection myConnection = null;
        /// <summary>
        /// 数据库操作事务对象
        /// </summary>
        public OracleTransaction myTrans = null;

        /// <summary>
        /// OracleHelper 类构造函数
        /// </summary>
        public OracleHelper()
        {
        }
        /// <summary>
        /// OracleHelper 类构造函数
        /// </summary>
        /// <param name="connectStr">指定数据库连接字符串</param>
        public OracleHelper(string connectStr)
        {
            myConnectionString = connectStr;
        }
        /// <summary>
        /// OracleHelper 类构造函数
        /// </summary>
        /// <param name="connectStr">指定数据库连接字符串</param>
        /// <param name="createConnect">是否创建 SqlConnection 对象</param>
        public OracleHelper(string connectStr, bool createConnect)
        {
            myConnectionString = connectStr;
            if (createConnect)
                myConnection = new OracleConnection(myConnectionString);
        }
        /// <summary>
        /// 打开数据库连接对象
        /// </summary>
        public void Open()
        {
            if (myConnection != null && myConnection.State != ConnectionState.Open)
                myConnection.Open();
        }
        /// <summary>
        /// 关闭数据库连接对象
        /// </summary>
        public void Close()
        {
            myConnection.Close();
        }
        /// <summary>
        /// 创建数据库连接对象
        /// </summary>
        public OracleConnection CreateConnection()
        {
            try
            {
                if (myConnection == null)
                {
                    myConnection = new OracleConnection(myConnectionString);
                }
            }
            catch (OracleException e)
            {
                //In case of:   ORA-00028(session kill)
                //              ORA-02396(exceed idle time)
                //            ORA-01012(not logon)
                //            ORA-12535(timeout)
                #region 以前的代码
                //if (e.Code == 2396 || e.Code == 1012 || e.Code == 28 || e.Code == 12535)
                //{

                //    OracleConnection.ClearPool(myConnection);
                //    var tt = e.Message;
                //    StringBuilder errorStr = new StringBuilder();
                //    errorStr.Append("错误信息:" + e.Message + "\r\n");
                //    errorStr.Append("错误应用程序或对象名称:" + e.Source + "\r\n");
                //    errorStr.Append("在堆栈中表现形式" + e.StackTrace + "\r\n");
                //    errorStr.Append("引发异常的方法:" + e.TargetSite + "\r\n");
                //    errorStr.Append("e.Code:" + e.Code);
                //    UnifyCode.Common.WriteLog("", DateTime.Now.ToString("yyyy-MM-dd"), errorStr.ToString());
                //}
                #endregion
                throw e;//抛出，异常由外层catch处理 20151202 cjt
            }

            return myConnection;
        }
        /// <summary>
        /// 创建操作事务对象
        /// </summary>
        /// <returns></returns>
        public OracleTransaction CreateTransaction()
        {
            if (myConnection == null)
            {
                myConnection = new OracleConnection(myConnectionString);
            }
            Open();
            myTrans = myConnection.BeginTransaction();
            return myTrans;
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>返回此次执行影响行数</returns>
        public int ExecSQLNonQuery(string sql)
        {
            int count = 0;
            try
            {
                myConnection = CreateConnection();
                using (myConnection)
                {
                    if (myConnection.State == ConnectionState.Closed)
                    {
                        myConnection.Open();
                    }
                    OracleCommand cmd = new OracleCommand(sql, myConnection);
                    count = cmd.ExecuteNonQuery();
                }
            }
            catch (System.Exception ex)
            {
                count = 0;
                throw ex;
                //UnifyCode.Common.WriteLog("", DateTime.Now.ToString("yyyy-MM-dd"), ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace + "\r\n" + ex.TargetSite+"\r\n"+sql);
            }
            // myConnection.Close();
            return count;
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">parameter参数集合</param>
        /// <returns></returns>
        public int ExecSQLNonQuery(string sql, OracleParameter[] param)
        {
            int count = 0;
            try
            {
                myConnection = CreateConnection();
                using (myConnection)
                {
                    OracleCommand cmd = new OracleCommand(sql, myConnection);
                    cmd.Parameters.AddRange(param);
                    count = cmd.ExecuteNonQuery();
                }
            }
            catch (System.Exception ex)
            {
                count = 0;
                throw ex;
            }
            // myConnection.Close();
            return count;
        }
        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>返回此次执行 影响行数</returns>
        public int ExecSQLTransaction(string sql)
        {
            int count = 0;
            CreateTransaction();
            myConnection = CreateConnection();
            using (myConnection)
            {
                using (myTrans)
                {
                    try
                    {
                        OracleCommand cmd = new OracleCommand();
                        cmd.CommandText = sql;
                        cmd.Connection = myConnection;
                        cmd.Transaction = myTrans;
                        count = cmd.ExecuteNonQuery();
                        if (count < 1)
                        {
                            myTrans.Rollback();
                        }
                        if (count > 0)
                        {
                            myTrans.Commit();
                        }
                    }
                    catch (System.Exception ex)
                    {
                        myTrans.Rollback();
                        throw ex;
                        //UnifyCode.Common.WriteLog("", DateTime.Now.ToString("yyyy-MM-dd"), ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace + "\r\n" + ex.TargetSite+"\r\n"+sql);
                    }

                }
            }
            //myConnection.Close();
            return count;
        }
        /// <summary>
        /// 执行SQL返回DataSet
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>返回DataSet</returns>
        public DataSet ExecSQLDataSet(string sql)
        {
            DataSet Ds = new DataSet();
            try
            {
                myConnection = CreateConnection();
                using (myConnection)
                {
                    OracleDataAdapter adp = new OracleDataAdapter(sql, myConnection);
                    adp.Fill(Ds);
                }

            }
            catch (System.Exception ex)
            {
                //var tt = ex.Message;
                //StringBuilder errorStr = new StringBuilder();
                //errorStr.Append("错误信息:" + ex.Message+"\r\n");
                //errorStr.Append("错误应用程序或对象名称:" + ex.Source+"\r\n");
                //errorStr.Append("在堆栈中表现形式" + ex.StackTrace+"\r\n");
                //errorStr.Append("引发异常的方法:" + ex.TargetSite + "\r\n");
                //errorStr.Append("sql:" + sql);
                //UnifyCode.Common.WriteLog("", DateTime.Now.ToString("yyyy-MM-dd"), errorStr.ToString());
                throw ex;
            }
            // myConnection.Close();
            return Ds;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CitizendCard_Service.Models;
using System.Data;
using CitizendCard_Service.DAL;
using System.Configuration;
using System.Text;

namespace CitizendCard_Service.BLL
{
    public class OrderService
    {
        private static int NLEVELID =Convert.ToInt32(ConfigurationManager.AppSettings["NLEVELID"].ToString());
        private static string TerminalID = ConfigurationManager.AppSettings["TerminalID"].ToString();
        /// <summary>
        /// Haves the same barcode.
        /// 检查条码是否重复
        /// </summary>
        /// <param name="barcode">The barcode.</param>
        /// <returns></returns>
        /// <remarks>Created At Time: [ 2017-2-21 21:28 ], By User:lishuai, On Machine:Brian-NB</remarks>
        public static bool HaveSameBarcode(string barcode) 
        {
            string sql = "select count(sbarcode) as cn from GS_F_ACCESS_CITIZENCARD where sbarcode='{0}'";
            sql = string.Format(sql, barcode);
            OracleHelper db = new OracleHelper();
            DataSet ds= db.ExecSQLDataSet(sql);
            if (Convert.ToInt16(ds.Tables[0].Rows[0]["cn"])>0)
            {
                return true;
            }
            else
            {
                return false;
            }
           
        }

        /// <summary>
        /// Haves the order.
        /// 验证市民卡的交易号是否存在
        /// </summary>
        /// <param name="orderno">The orderno.</param>
        /// <param name="barcode">The barcode.</param>
        /// <returns></returns>
        /// <remarks>Created At Time: [ 2017-2-21 23:44 ], By User:lishuai, On Machine:Brian-NB</remarks>
        public static bool HaveTheOrder(string orderno, string barcode) 
        {
            string sql = "select Count(A.NTICKETID) as cn from GS_T_TICKETBASEINFO A,GS_T_TICKETLEVEL B,GS_F_ACCESS_CITIZENCARD c ";
                   sql+=" WHERE A.NTICKETLEVEL=B.NLEVELID AND B.NLEVELID={0} ";
                   sql+=" and a.nticketid=c.nticketid and c.ndealid={1} and c.nstatus=0 ";
                   sql = string.Format(sql, NLEVELID, orderno);
                   OracleHelper db = new OracleHelper();
                   DataSet ds = db.ExecSQLDataSet(sql);
                   if (Convert.ToInt32(ds.Tables[0].Rows[0]["cn"])>0)
                   {
                       return true;
                   }
                   else
                   {
                       return false;
                   }
        }

        /// <summary>
        /// 判断对方给的交易号和环企系统中的交易号是否重复
        /// </summary>
        /// <param name="orderno">The orderno.</param>
        /// <returns></returns>
        /// <remarks>Created At Time: [ 2017-2-22 16:36 ], By User:lishuai, On Machine:Brian-NB</remarks>
        public static bool Already_Existing(string orderno)
        {
            string sql = "SELECT count(*) as cn FROM GS_F_ACCESS_CITIZENCARD where NDEALID={0}";
          
            sql = string.Format(sql,Convert.ToInt32(orderno));
            OracleHelper db = new OracleHelper();
            DataSet ds = db.ExecSQLDataSet(sql);
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["cn"]) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取门票基础信息，构造过闸检票数据
        /// </summary>
        /// <param name="co">The co.</param>
        /// <returns></returns>
        /// <remarks>Created At Time: [ 2017-2-22 1:32 ], By User:lishuai, On Machine:Brian-NB</remarks>
        public static Gs_f_Access GetAccess(CitizenOrder co) 
        {
            //根据票种co.PRODUCTID获取Access相关数据 
            
            Gs_f_Access gs_f_access = new Gs_f_Access();
            gs_f_access.NTICKETID = co.PRODUCTID.ToString();
            gs_f_access.NDEALID = co.OUTORDERNO;
            gs_f_access.SBARCODE = co.BARCODE;
            gs_f_access.SSELLDATE = DateTime.Now.ToString("G");
            gs_f_access.STERMINAL = TerminalID;
            gs_f_access.NINCOUNT = 0; //默认是没有进入
           // gs_f_access.NTYPE = 1;
            gs_f_access.NSTATUS = 0;
            gs_f_access.NISDELAY = 0;
            gs_f_access.NISREFUND = 0;
            gs_f_access.NISREPRINT = 0;
            gs_f_access.NOUTCOUNT = 0;
            gs_f_access.NIVALIDDAYSCOUNT = 1;
            gs_f_access.DVSTARTDATE=DateTime.Now.ToString("d");
            gs_f_access.DVENDDATE=DateTime.Now.ToString("d");

            DataSet ds_base = new DataSet();
            ds_base = GetTicketBaseInfo(co.PRODUCTID);
            if (ds_base==null)
            {
                return null;
            }
            else
            {
                gs_f_access.NPRICE =Convert.ToDecimal(ds_base.Tables[0].Rows[0]["NGENERALPRICE"]);
                gs_f_access.NPRINTPRICE =Convert.ToDecimal(ds_base.Tables[0].Rows[0]["NPRINTPRICE"]);
                gs_f_access.NSALDAYS = 0;
                gs_f_access.STICKETTYPE = ds_base.Tables[0].Rows[0]["NTICKETTYPE"].ToString();
                gs_f_access.NTIMES = 999;

                
            }
            DataSet dsInparkCount = new DataSet();
            dsInparkCount = GetInParkCount(co.PRODUCTID);
            if (dsInparkCount==null)
            {
                return null;
            }
            else
            {
                gs_f_access.NALLPARKCOUNT = Convert.ToInt32(dsInparkCount.Tables[0].Rows[0]["NALLPARKCOUNT"]);
                gs_f_access.NENTRANCECOUNT = Convert.ToInt32(dsInparkCount.Tables[0].Rows[0]["NINPARKCOUNT"]);
            }

            DataSet ds_validity = new DataSet();
            ds_validity = GetValidity(co.PRODUCTID);
            if (ds_validity==null)
            {
                return null;
            }
            else
            {   
                gs_f_access.NTIMES=Convert.ToInt32(ds_validity.Tables[0].Rows[0]["NTIMES"]);
                //gs_f_access.NINCOUNT = gs_f_access.NTIMES;
                gs_f_access.NSALDAYS = Convert.ToInt32(ds_validity.Tables[0].Rows[0]["NSALEDAYS"]);
                gs_f_access.NTYPE = Convert.ToInt32(ds_validity.Tables[0].Rows[0]["NTYPE"]);
                //DbNull.Value SSTARTTIME SENDTIME SSTARTDATE SENDDATE
                gs_f_access.DVSTARTTIME = ds_validity.Tables[0].Rows[0]["SSTARTTIME"].ToString() == "" ? "06:00:00" : ds_validity.Tables[0].Rows[0]["SSTARTTIME"].ToString();
                gs_f_access.DVENDTIME = ds_validity.Tables[0].Rows[0]["SENDTIME"].ToString() == "" ? "22:00:00" : ds_validity.Tables[0].Rows[0]["SENDTIME"].ToString();

            }

            return gs_f_access;
        }

        /// <summary>
        /// 根据票种ID获取基础门票信息
        /// </summary>
        /// <param name="ticketid">The ticketid.</param>
        /// <returns></returns>
        /// <remarks>Created At Time: [ 2017-2-22 0:23 ], By User:lishuai, On Machine:Brian-NB</remarks>
        public static DataSet GetTicketBaseInfo(int ticketid) 
        {
            string sql = "select * from GS_T_TICKETBASEINFO where nticketid={0}";
            sql = string.Format(sql, ticketid);
            
            OracleHelper db = new OracleHelper();
            DataSet ds = db.ExecSQLDataSet(sql);
            if (Convert.ToInt32(ds.Tables[0].Rows.Count) > 0)
            {
                return ds;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the in park count.
        /// 根据门票票种获取可以入园数量信息
        /// </summary>
        /// <param name="ticketid">The ticketid.</param>
        /// <returns></returns>
        /// <remarks>Created At Time: [ 2017-2-22 0:57 ], By User:lishuai, On Machine:Brian-NB</remarks>
        public static DataSet GetInParkCount(int ticketid) 
        {
            string sql = "select * from GS_T_InParkCount where nTciektID={0}";
            sql = string.Format(sql, ticketid);

            OracleHelper db = new OracleHelper();
            DataSet ds = db.ExecSQLDataSet(sql);
            if (Convert.ToInt32(ds.Tables[0].Rows.Count) > 0)
            {
                return ds;
            }
            else
            {
                return null;
            }
        
        }

        /// <summary>
        /// 根据票种设定，获取门票有效期信息
        /// </summary>
        /// <param name="ticketid">The ticketid.</param>
        /// <returns></returns>
        /// <remarks>Created At Time: [ 2017-2-22 1:08 ], By User:lishuai, On Machine:Brian-NB</remarks>
        public static DataSet GetValidity(int ticketid) 
        {
           
            string sql = "select * from GS_T_Validity where nTicketID={0}";
            sql = string.Format(sql, ticketid);

            OracleHelper db = new OracleHelper();
            DataSet ds = db.ExecSQLDataSet(sql);
            if (Convert.ToInt32(ds.Tables[0].Rows.Count) > 0)
            {
                return ds;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Checks the is ok product.
        /// 检查是否是廋西湖允许售卖的市民卡产品
        /// </summary>
        /// <param name="ticketid">The ticketid.</param>
        /// <returns></returns>
        /// <remarks>Created At Time: [ 2017-2-22 11:40 ], By User:lishuai, On Machine:Brian-NB</remarks>
        public static bool CheckIsOkProduct(int ticketid)
        {

            string sql = "select count(A.NTICKETID) as cn from GS_T_TICKETBASEINFO A,GS_T_TICKETLEVEL B WHERE A.NTICKETLEVEL=B.NLEVELID AND B.NLEVELID={0} and A.NTICKETID={1}";
            sql = string.Format(sql,NLEVELID, ticketid);

            OracleHelper db = new OracleHelper();
            DataSet ds = db.ExecSQLDataSet(sql);
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["cn"]) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据市民卡提供的订单号 获取对应上一次的BARCODE
        /// </summary>
        /// <param name="orderno">The orderno.</param>
        /// <returns></returns>
        /// <remarks>Created At Time: [ 2017-2-22 17:13 ], By User:lishuai, On Machine:Brian-NB</remarks>
        public static string GetLastBarcode(string orderno)
        {


            string sql = "select SBARCODE from ( ";
            sql+=  "select c.SBARCODE from GS_T_TICKETBASEINFO A,GS_T_TICKETLEVEL B,GS_F_ACCESS_CITIZENCARD c ";
            sql += "WHERE A.NTICKETLEVEL=B.NLEVELID AND B.NLEVELID={0} and a.nticketid=c.nticketid and c.ndealid='{1}' and c.nstatus=0 ";
            sql += " order by c.sselldate desc";
            sql+=") where rownum<=1";
           
            sql = string.Format(sql, NLEVELID, orderno);

            OracleHelper db = new OracleHelper();
            DataSet ds = db.ExecSQLDataSet(sql);
            if (Convert.ToInt32(ds.Tables[0].Rows.Count) > 0)
            {
                return ds.Tables[0].Rows[0]["SBARCODE"].ToString();
            }
            else
            {
                return "";
            }
        }


       //执行新增交易
        public static bool ExecInsertTrans(CitizenOrder co,Gs_f_Access gf) 
        {
            //insert into CITIZENORDER
            //insert into gs_f_access
           
            StringBuilder sql = new StringBuilder();
            sql.Append(" begin ");

            #region insert into CITIZENORDER
            sql.Append(@"insert into CITIZENORDER(");
            sql.Append("OUTORDERNO,NAME,ICNO,PRODUCTID,BARCODE,TRANSDATE)");
            sql.Append(" values(");
            sql.Append(string.Format(@"'{0}',N'{1}','{2}',{3},'{4}','{5}'", co.OUTORDERNO, co.NAME, co.ICNO, co.PRODUCTID, co.BARCODE,co.TRANSDATE));
  
            sql.Append(");");


            #endregion

            #region insert into GS_F_ACCESS_CITIZENCARD

            sql.Append(@"insert into GS_F_ACCESS_CITIZENCARD(");
            sql.Append("ndealid,sbarcode,nticketid,nstatus,sselldate,stickettype,STERMINAL,NINCOUNT,");
            sql.Append("NOUTCOUNT,NENTRANCECOUNT,NALLPARKCOUNT,DVSTARTDATE,DVENDDATE,NISREFUND,NISREPRINT,");
            sql.Append("NISDELAY,NTYPE,NSALDAYS,NTIMES,NPRICE,NIVALIDDAYSCOUNT,NPRINTPRICE)");
            sql.Append(" values(");
            sql.Append(string.Format(@"'{0}','{1}',{2},{3},to_date('{4}','YYYY-MM-DD HH24:MI:SS'),'{5}','{6}',{7},", gf.NDEALID, gf.SBARCODE, Convert.ToInt32(gf.NTICKETID), gf.NSTATUS, gf.SSELLDATE,gf.STICKETTYPE, gf.STERMINAL, gf.NINCOUNT));
            sql.Append(string.Format(@"{0},{1},{2},to_date('{3}','yyyy-MM-dd'),to_date('{4}','yyyy-MM-dd'),{5},{6},", gf.NOUTCOUNT,gf.NENTRANCECOUNT,gf.NALLPARKCOUNT,gf.DVSTARTDATE,gf.DVENDDATE,gf.NISREFUND,gf.NISREPRINT));
            sql.Append(string.Format(@"{0},{1},{2},{3},{4},{5},{6}", gf.NISDELAY,gf.NTYPE,gf.NSALDAYS,gf.NTIMES,gf.NPRICE,gf.NIVALIDDAYSCOUNT,gf.NPRINTPRICE));
           
            sql.Append(");");

            #endregion

            sql.Append(" end;");

            OracleHelper db = new OracleHelper();
            return db.ExecSQLTransaction(sql.ToString())>0?true:false;
           
        }

        //执行Update Barcode交易
        public static bool ExecUpdateTrans(CitizenOrder co, Gs_f_Access g) 
        {
            //insert into CITIZENORDER
            //update GS_F_ACCESS_CITIZENCARD
            StringBuilder sql = new StringBuilder();
            sql.Append(" begin ");

            #region insert into CITIZENORDER
            sql.Append(@"insert into CITIZENORDER(");
            sql.Append("OUTORDERNO,NAME,ICNO,PRODUCTID,BARCODE,TRANSDATE)");
            sql.Append(" values(");
            sql.Append(string.Format(@"'{0}',N'{1}','{2}',{3},'{4}','{5}'", co.OUTORDERNO, co.NAME, co.ICNO, co.PRODUCTID, co.BARCODE, co.TRANSDATE));

            sql.Append(");");


            #endregion

            //update GS_F_ACCESS_CITIZENCARD set sbarcode='140101112236'  where SBARCODE='151018100368'
            string last_Barcode=OrderService.GetLastBarcode(co.OUTORDERNO);
            if (last_Barcode=="" || string.IsNullOrEmpty(last_Barcode))
	        {
		        throw  new Exception("市民卡订单号异常，此单中断,请市民卡确认！");
	        }

            #region update access
            sql.Append(" update GS_F_ACCESS_CITIZENCARD ");
            sql.AppendFormat(" set sbarcode = '{0}'", co.BARCODE);
            sql.AppendFormat(" where SBARCODE = '{0}'",last_Barcode); //订单号

            sql.Append(";");
            #endregion

            sql.Append(" end;");

            OracleHelper db = new OracleHelper();
            return db.ExecSQLTransaction(sql.ToString()) > 0 ? true : false;

        }

    }
}
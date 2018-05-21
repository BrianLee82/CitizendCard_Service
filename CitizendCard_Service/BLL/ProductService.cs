using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CitizendCard_Service.Models;
using System.Data;
using CitizendCard_Service.DAL;
using System.Configuration;

namespace CitizendCard_Service.BLL
{
    public class ProductService
    {
        private static int NLEVELID = Convert.ToInt32(ConfigurationManager.AppSettings["NLEVELID"].ToString());
        public static List<Product> GetProducts() 
        {

            List<Product> lsProducts = new List<Product>();
            DataSet ds = GetProductData();
            if (Common.CheckDataSet(ds))
                return ConvertProductInnerList(ds);
            return null;
          
        }
        public static DataSet GetProductData()
        {
            string sql = "select A.NTICKETID,A.STICKETNAMECH from GS_T_TICKETBASEINFO A,GS_T_TICKETLEVEL B WHERE A.NTICKETLEVEL=B.NLEVELID AND B.NLEVELID={0}";
            OracleHelper db = new OracleHelper();
            sql = string.Format(sql, NLEVELID);
            return db.ExecSQLDataSet(sql);
        }

        public static Product ConvertProductInner(DataRow dr)
        {
            if (dr != null)
            {
                Product _product = new Product();

                if (dr.Table.Columns.Contains("NTICKETID") && dr["NTICKETID"] != DBNull.Value)
                    _product.ProductID = Convert.ToInt32(dr["NTICKETID"]);

                if (dr.Table.Columns.Contains("STICKETNAMECH") && dr["STICKETNAMECH"] != DBNull.Value)
                    _product.ProductName = Convert.ToString(dr["STICKETNAMECH"]);
               
 
                return _product;
            }
            return null;
        }
        public static List<Product> ConvertProductInnerList(DataSet ds)
        {
            List<Product> _products = new List<Product>();
            if (Common.CheckDataSet(ds))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Product _product = ConvertProductInner(dr);
                    if (_product != null)
                        _products.Add(_product);
                }
            }
            return _products;
        }
    }


}
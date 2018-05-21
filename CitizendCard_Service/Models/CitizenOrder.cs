using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CitizendCard_Service.Models
{
    public class CitizenOrder
    {
        //外部订单号        
        /// <summary>
        /// Gets or sets the outorderno.外部订单号 
        /// </summary>
        /// <value>
        /// The outorderno.
        /// </value>
        /// <remarks>Created At Time: [ 2017-2-21 22:42 ], By User:lishuai, On Machine:Brian-NB</remarks>
        public string OUTORDERNO { get; set; }

        //      
        /// <summary>
        /// Gets or sets the name.姓名  
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        /// <remarks>Created At Time: [ 2017-2-21 22:42 ], By User:lishuai, On Machine:Brian-NB</remarks>
        public string NAME { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        /// <value>
        /// The icno.
        /// </value>
        /// <remarks>Created At Time: [ 2017-2-21 22:42 ], By User:lishuai, On Machine:Brian-NB</remarks>
        public string ICNO { get; set; }

        /// <summary>
        /// Gets or sets the productid.PRODUCTID
        /// </summary>
        /// <value>
        /// The productid.
        /// </value>
        /// <remarks>Created At Time: [ 2017-2-21 22:43 ], By User:lishuai, On Machine:Brian-NB</remarks>
        public int PRODUCTID { get; set; }


        /// <summary>
        /// Gets or sets the barcode.条码
        /// </summary>
        /// <value>
        /// The barcode.
        /// </value>
        /// <remarks>Created At Time: [ 2017-2-21 22:43 ], By User:lishuai, On Machine:Brian-NB</remarks>
        public string BARCODE { get; set; }

        /// <summary>
        /// Gets or sets the transdate.交易时间
        /// </summary>
        /// <value>
        /// The transdate.
        /// </value>
        /// <remarks>Created At Time: [ 2017-2-21 22:44 ], By User:lishuai, On Machine:Brian-NB</remarks>
        public string TRANSDATE { get; set; }


    }
}
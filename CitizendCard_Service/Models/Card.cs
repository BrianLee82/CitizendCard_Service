using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CitizendCard_Service.Models
{
    public class Card
    {
        /// <summary>
        /// Gets or sets the product identifier.
        /// 门票票种ID
        /// </summary>
        /// <value>
        /// The product identifier.
        /// </value>
        /// <remarks>Created At Time: [ 2017-2-21 17:39 ], By User:lishuai, On Machine:Brian-NB</remarks>
        public int ProductId { get; set; }
        /// <summary>
        /// Gets or sets the ic no.
        /// 身份证号码
        /// </summary>
        /// <value>
        /// The ic no.
        /// </value>
        /// <remarks>Created At Time: [ 2017-2-21 17:39 ], By User:lishuai, On Machine:Brian-NB</remarks>
        public string IcNo { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// 姓名
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        /// <remarks>Created At Time: [ 2017-2-21 17:39 ], By User:lishuai, On Machine:Brian-NB</remarks>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the barcode.
        /// 市民卡产生条码10码
        /// </summary>
        /// <value>
        /// The barcode.
        /// </value>
        /// <remarks>Created At Time: [ 2017-2-21 17:40 ], By User:lishuai, On Machine:Brian-NB</remarks>
        public string Barcode { get; set; }

        /// <summary>
        /// Gets or sets the outer no.
        /// 市民卡订单号
        /// </summary>
        /// <value>
        /// The outer no.
        /// </value>
        /// <remarks>Created At Time: [ 2017-2-21 17:40 ], By User:lishuai, On Machine:Brian-NB</remarks>
        public string OuterNo { get; set; }

        /// <summary>
        /// Gets or sets the merchant code.
        /// 市民卡账号
        /// </summary>
        /// <value>
        /// The merchant code.
        /// </value>
        /// <remarks>Created At Time: [ 2017-2-21 17:36 ], By User:lishuai, On Machine:Brian-NB</remarks>
        public string MerchantCode { get; set; }

        public string Signature { get; set; }



    }
}
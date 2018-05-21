using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CitizendCard_Service.Models
{
    public class Gs_f_Access
    {
       
        public string NDEALID { get; set; }
        public string SBARCODE { get; set; }
        public string NTICKETID { get; set; }
        public int NSTATUS { get; set; }
        public string SSELLDATE { get; set; }
        public string STICKETTYPE { get; set; }
        public string STERMINAL { get; set; }
        public int NINCOUNT { get; set; }
        public int NOUTCOUNT { get; set; }
        //可进入的景区数
        public int NENTRANCECOUNT { get; set; }
        //每个景区可进入次数
        public int NALLPARKCOUNT { get; set; }
        public string DVSTARTDATE { get; set; }
        public string DVENDDATE { get; set; }
        public string DVSTARTTIME{get;set;}
        public string DVENDTIME { get; set; }
        public int NISREFUND { get; set; }
        public int NISREPRINT { get; set; }
        public int NISDELAY { get; set; }
        public int NTYPE { get; set; }
        public int NSALDAYS { get; set; }
        public int NTIMES { get; set; }
        public decimal NPRICE { get; set; }
        public int NIVALIDDAYSCOUNT { get; set; }
        public decimal NPRINTPRICE { get; set; }


    }
}
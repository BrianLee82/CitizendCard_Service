using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CitizendCard_Service.Models
{
    public class Result
    {

        public bool IsTrue { get; set; } //true:成功 false:失败
        public string ResultCode { get; set; } //操作代码
        public string ResultMsg { get; set; } //操作详细描述
        public dynamic ResultJson { get; set; } //结果json字符串
        /// <summary>
        /// 获取当前操纵做代码所对应的操纵做描述
        /// 20170220
        /// Brian
        /// </summary>
        public void GetError()
        {
            this.IsTrue = false;
            if (this.ResultCode == "00")
                this.IsTrue = true;
            this.ResultMsg = Common.GetError(this.ResultCode);
        }
    }
}
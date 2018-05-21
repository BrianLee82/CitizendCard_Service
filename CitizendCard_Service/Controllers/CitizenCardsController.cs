using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using CitizendCard_Service.Models;
using System.Configuration;
using CitizendCard_Service.BLL;

namespace CitizendCard_Service.Controllers
{
    public class CitizenCardsController : ApiController
    {
        #region configvalue
        private string Mcode = ConfigurationManager.AppSettings["merchantCode"];
        private string Pwd = ConfigurationManager.AppSettings["password"];
        #endregion

        #region api test
        //public string Get(int id)
        //{
        //    return "value";
        //}
        //[HttpGet]
        //public string GetAll(int id, string name)
        //{
        //    return "ChargingData" + id;
        //}

        //public string TEST(dynamic obj)
        //{//[FromBody]
        //    string resultJson = string.Empty;
        //    //增加参数日志记录
        //    DateTime stime = DateTime.Now;
        //    StringBuilder logInfo = new StringBuilder();
        //    logInfo.Append("调用接口名:Get_CitizenCards \r\n");
        //    logInfo.Append("\r\n");
        //    logInfo.Append("参数:\r\n");
        //    logInfo.AppendFormat("merchantCode:{0}\r\n", obj.merchantCode);
        //    logInfo.AppendFormat("signature:{0}\r\n", obj.signature);
        //    logInfo.Append("\r\n");
        //    try
        //    {

        //        Result result = new Result();
        //        result.IsTrue = false;
        //        List<Product> lisPro = new List<Product>();
        //        //检测是否包含特殊字符
        //        if (Common.CheckParameters(obj.merchantCode))
        //        {
        //            return Common.JsonSerialize(Common.GetResultMsg("102"));    //参数含有非法字符
        //        }

        //        if (Common.CheckSignature(obj.signature, Mcode + Pwd))
        //        {
        //            lisPro = ProductService.GetProducts();
        //            if (lisPro == null)
        //            {
        //                resultJson = Common.JsonSerialize(Common.GetResultMsg("108")); //查无可售市民卡数据  
        //            }
        //            else
        //            {
        //                result.IsTrue = true;
        //                result.ResultCode = "00";
        //                result.ResultMsg = "成功";
        //                result.ResultJson = Common.JsonSerialize(lisPro);
        //                resultJson = Common.JsonSerialize(result); //签名失败
        //            }
        //        }
        //        else
        //        {
        //            resultJson = Common.JsonSerialize(Common.GetResultMsg("106")); //签名失败    
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //添加错误信息记录
        //        logInfo.AppendFormat("错误信息:{0}\r\n", ex.Message);
        //        logInfo.AppendFormat("引发异常的方法:{0}\r\n", ex.TargetSite);
        //        logInfo.AppendFormat("错误应用程序或对象名称:{0}\r\n", ex.Source);
        //        logInfo.AppendFormat("在堆栈中表现形式:{0}\r\n", ex.StackTrace);
        //        logInfo.Append("\r\n");
        //        resultJson = Common.JsonSerialize(Common.GetResultMsg("101"));
        //    }
        //    finally
        //    {
        //        //返回信息记录
        //        logInfo.AppendFormat("返回:{0}\r\n", resultJson);
        //        DateTime etime = DateTime.Now;
        //        //添加时间记录
        //        logInfo.Append(Common.GetLogTimeSpan(stime, etime));
        //        //写入日志
        //        Common.WriteLog("", DateTime.Now.ToString("yyyy-MM-dd") + obj.merchantCode, logInfo.ToString());
        //    }
        //    return resultJson;
        //}

        #endregion

        #region ListCards 获取市民卡对应票种信息
        //[HttpGet]
        //public Result ListCards(string merchantCode, string signature)
        //{//[FromBody]
        //    string resultJson = string.Empty;
        //    //增加参数日志记录
        //    DateTime stime = DateTime.Now;
        //    StringBuilder logInfo = new StringBuilder();
        //    logInfo.Append("调用接口名:Get_CitizenCards \r\n");
        //    logInfo.Append("\r\n");
        //    logInfo.Append("参数:\r\n");
        //    logInfo.AppendFormat("merchantCode:{0}\r\n", merchantCode);
        //    logInfo.AppendFormat("signature:{0}\r\n", signature);
        //    logInfo.Append("\r\n");
        //    try
        //    {

        //        Result result = new Result();
        //        result.IsTrue = false;
        //        List<Product> lisPro = new List<Product>();
        //        //检测是否包含特殊字符
        //        if (Common.CheckParameters(merchantCode))
        //        {
        //            return Common.GetResultMsg("102");    //参数含有非法字符
        //        }

        //        if (Common.CheckSignature(signature, Mcode + Pwd))
        //        {
        //            lisPro = ProductService.GetProducts();
        //            if (lisPro == null)
        //            {
        //                return Common.GetResultMsg("108"); //查无可售市民卡数据  
        //            }
        //            else
        //            {
        //                result.IsTrue = true;
        //                result.ResultCode = "00";
        //                result.ResultMsg = "成功";
        //                result.ResultJson = lisPro;
        //                return result;
                        
        //            }
        //        }
        //        else
        //        {
        //            return Common.GetResultMsg("106"); //签名失败    
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //添加错误信息记录
        //        logInfo.AppendFormat("错误信息:{0}\r\n", ex.Message);
        //        logInfo.AppendFormat("引发异常的方法:{0}\r\n", ex.TargetSite);
        //        logInfo.AppendFormat("错误应用程序或对象名称:{0}\r\n", ex.Source);
        //        logInfo.AppendFormat("在堆栈中表现形式:{0}\r\n", ex.StackTrace);
        //        logInfo.Append("\r\n");
        //        return Common.GetResultMsg("101");
        //    }
        //    finally
        //    {
        //        //返回信息记录
        //        logInfo.AppendFormat("返回:{0}\r\n", resultJson);
        //        DateTime etime = DateTime.Now;
        //        //添加时间记录
        //        logInfo.Append(Common.GetLogTimeSpan(stime, etime));
        //        //写入日志
        //        Common.WriteLog("", DateTime.Now.ToString("yyyy-MM-dd") + merchantCode, logInfo.ToString());
        //    }
            
        //}

        [HttpPost]  
        public Result ListCards(ListPartner listpartner)
        {
            string resultJson = string.Empty;
            //增加参数日志记录
            DateTime stime = DateTime.Now;
            StringBuilder logInfo = new StringBuilder();
            logInfo.Append("调用接口名:ListCards \r\n");
            logInfo.Append("\r\n");
            logInfo.Append("参数:\r\n");
            logInfo.AppendFormat("merchantCode:{0}\r\n", listpartner.merchantCode);
            logInfo.AppendFormat("signature:{0}\r\n", listpartner.signature);
            logInfo.Append("\r\n");
            try
            {

                Result result = new Result();
                result.IsTrue = false;
                List<Product> lisPro = new List<Product>();
                //检测是否包含特殊字符
                if (Common.CheckParameters(listpartner.merchantCode))
                {
                    return Common.GetResultMsg("102");    //参数含有非法字符
                }

                if (Common.CheckSignature(listpartner.signature, Mcode + Pwd))
                {
                    lisPro=ProductService.GetProducts();
                    if (lisPro == null)
                    {
                        return Common.GetResultMsg("108"); //查无可售市民卡数据  
                    }
                    else
                    {
                        result.IsTrue = true;
                        result.ResultCode = "00";
                        result.ResultMsg = "成功";
                        result.ResultJson = lisPro;
                        return result;
                    }
                }
                else
                {
                    return Common.GetResultMsg("106"); //签名失败    
                }

            }
            catch (Exception ex)
            {
                //添加错误信息记录
                logInfo.AppendFormat("错误信息:{0}\r\n", ex.Message);
                logInfo.AppendFormat("引发异常的方法:{0}\r\n", ex.TargetSite);
                logInfo.AppendFormat("错误应用程序或对象名称:{0}\r\n", ex.Source);
                logInfo.AppendFormat("在堆栈中表现形式:{0}\r\n", ex.StackTrace);
                logInfo.Append("\r\n");
                return Common.GetResultMsg("101");
            }
            finally
            {
                //返回信息记录
                logInfo.AppendFormat("返回:{0}\r\n", resultJson);
                DateTime etime = DateTime.Now;
                //添加时间记录
                logInfo.Append(Common.GetLogTimeSpan(stime, etime));
                //写入日志
                Common.WriteLog("", DateTime.Now.ToString("yyyy-MM-dd") + listpartner.merchantCode, logInfo.ToString());
            }
           

            
        }

       

        #endregion

        #region SaveCard 市民卡信息推送接口
        [HttpPost]
        public Result SaveCard(Card oData)
        {
            //1、参数过滤；
            //2、签名验证；
            //3、条码重复验证；
            //4、解析Json为对象数据；V
            //5、没有这个订单数据，Insert DB(市民卡订单表CITIZENORDER、GS_F_ACCESS)
            //6、有这个订单数据，Update GS_F_ACCESS Barcode;
            //7、返回结果；

            #region log
            string resultJson = string.Empty;
            //增加参数日志记录
            DateTime stime = DateTime.Now;
            StringBuilder logInfo = new StringBuilder();
            logInfo.Append("调用接口名:SaveCard \r\n");
            logInfo.Append("\r\n");
            logInfo.Append("参数:\r\n");
            logInfo.AppendFormat("merchantCode:{0}\r\n", oData.MerchantCode);
            logInfo.AppendFormat("ProductId:{0}\r\n", oData.ProductId);
            logInfo.AppendFormat("Barcode:{0}\r\n", oData.Barcode);
            logInfo.AppendFormat("IcNo:{0}\r\n", oData.IcNo);
            logInfo.AppendFormat("Name:{0}\r\n", oData.Name);
            logInfo.AppendFormat("OuterNo:{0}\r\n", oData.OuterNo);
            logInfo.AppendFormat("Signature:{0}\r\n", oData.Signature);
            logInfo.Append("\r\n");

            #endregion

            try
            {

                Result result = new Result();
                result.IsTrue = false;
               
                Gs_f_Access gs=new Gs_f_Access();
                CitizenOrder corder = new CitizenOrder
                {
                    BARCODE=oData.Barcode,
                    ICNO=oData.IcNo,
                    NAME=oData.Name,
                    OUTORDERNO=oData.OuterNo,
                    PRODUCTID=oData.ProductId,
                    TRANSDATE=DateTime.Now.ToString("G")
                };
                

                #region 检测参数是否为空
                if (oData==null)
                {
                   return Common.GetResultMsg("104");    //参数空值
                }
                #endregion

                #region 检测是否包含特殊字符

                if (Common.CheckParameters(oData.Barcode))
                {
                    return Common.GetResultMsg("102");    //参数含有非法字符
                }
                if (Common.CheckParameters(oData.IcNo))
                {
                    return Common.GetResultMsg("102");    //参数含有非法字符
                }
                if (Common.CheckParameters(oData.MerchantCode))
                {
                    return Common.GetResultMsg("102");    //参数含有非法字符
                }
                if (Common.CheckParameters(oData.Name))
                {
                    return Common.GetResultMsg("102");    //参数含有非法字符
                }
                if (Common.CheckParameters(oData.OuterNo))
                {
                    return Common.GetResultMsg("102");    //参数含有非法字符
                }
                if (Common.CheckParameters(oData.ProductId.ToString()))
                {
                    return Common.GetResultMsg("102");    //参数含有非法字符
                }

                if (Common.CheckParameters(oData.Signature))
                {
                    return Common.GetResultMsg("102");    //参数含有非法字符
                }

                #endregion

                #region 验证票种是否正确
                if (!OrderService.CheckIsOkProduct(oData.ProductId))
                {
                    return Common.GetResultMsg("109");    //参数空值
                }

                #endregion


                //string innerSignature = Mcode + oData.ProductId.ToString() + oData.IcNo + oData.Name + oData.Barcode + oData.OuterNo + Pwd;
                string innerSignature = Mcode + oData.ProductId.ToString() + oData.IcNo + oData.Barcode + oData.OuterNo + Pwd;

                if (Common.CheckSignature(oData.Signature, innerSignature))
                {

                    #region 验证条码重复
                    if (OrderService.HaveSameBarcode(oData.Barcode))
                    {
                        return Common.GetResultMsg("103"); //此条码已存在
                        
                    }
                    #endregion

                   

                    #region DB判断

                    if (OrderService.HaveTheOrder(oData.OuterNo, oData.Barcode))
                    {
                        gs = OrderService.GetAccess(corder);
                        if (gs == null)
                        {
                            //107市民卡提交json信息解析错误，请检查json格式和必选参数是否为空
                            return Common.GetResultMsg("107"); //市民卡提交数据错误
                            
                        }

                        if (OrderService.ExecUpdateTrans(corder, gs))
                        {
                            result.IsTrue = true;
                            result.ResultCode = "00";
                            result.ResultMsg = "成功";
                            result.ResultJson = null;
                            return result;
                        }
                        else
                        {
                            result.ResultCode = "110";
                            result.ResultMsg = "失败";
                            result.ResultJson = null;
                            return result;

                        }
                    }
                    else
                    {
                        gs = OrderService.GetAccess(corder);
                        if (gs == null)
                        {
                            //107市民卡提交json信息解析错误，请检查json格式和必选参数是否为空
                            return Common.GetResultMsg("107"); //市民卡提交数据错误

                        }
                        if (OrderService.ExecInsertTrans(corder, gs))
                        {
                            result.IsTrue = true;
                            result.ResultCode = "00";
                            result.ResultMsg = "成功";
                            result.ResultJson = null;
                            return result;
                        }
                        else
                        {
                            result.ResultCode = "110";
                            result.ResultMsg = "失败";
                            result.ResultJson = null;
                            return result;
                        }

                    }


                    #endregion


                 
                }
                else
                {
                    return Common.GetResultMsg("106"); //签名失败    
                }

            }
            catch (Exception ex)
            {
                //添加错误信息记录
                logInfo.AppendFormat("错误信息:{0}\r\n", ex.Message);
                logInfo.AppendFormat("引发异常的方法:{0}\r\n", ex.TargetSite);
                logInfo.AppendFormat("错误应用程序或对象名称:{0}\r\n", ex.Source);
                logInfo.AppendFormat("在堆栈中表现形式:{0}\r\n", ex.StackTrace);
                logInfo.Append("\r\n");
                return Common.GetResultMsg("101");
            }
            finally
            {
                //返回信息记录
                logInfo.AppendFormat("返回:{0}\r\n", resultJson);
                DateTime etime = DateTime.Now;
                //添加时间记录
                logInfo.Append(Common.GetLogTimeSpan(stime, etime));
                //写入日志
                Common.WriteLog("", DateTime.Now.ToString("yyyy-MM-dd") + oData.MerchantCode, logInfo.ToString());
            }
          


        }

        #endregion

        #region  获取市民卡第一次入园信息

        #endregion


    }
}

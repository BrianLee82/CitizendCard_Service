using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Newtonsoft.Json;

using System.Web.Script.Serialization;
using System.Globalization;
namespace CitizendCard_Service.Models
{
    public static class Common
    {
        /// <summary>
        ///  检查DataSet是否为空
        /// </summary>
        /// <param name="Ds"></param>
        /// <returns>true：不为空  false：为空</returns>
        public static bool CheckDataSet(DataSet Ds)
        {
            if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///  检测参数是否存在特殊字符
        /// </summary>
        /// <param name="parames">参数</param>
        /// <returns>true：有特殊字符  false：无特殊字符</returns>
        public static bool CheckParameters(string parames)
        {
            string[] strs = { "'", "=", "\"" };
            try
            {

                bool flag = false;
                foreach (var item in strs)
                {
                    int count = parames.IndexOf(item);
                    if (count > 0)
                    {
                        flag = true;
                        break;
                    }
                }
                return flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 检查是否是时间类型
        /// </summary>
        /// <param name="date">时间值字符串类型</param>
        /// <returns>true是时间类型，false错误</returns>
        public static bool CheckDate(string date)
        {
            try
            {
                Convert.ToDateTime(date);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #region 加密相关
        private static byte[] DESKeys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        public static string DESEncode(this string encryptString, string encryptKey)
        {
            //encryptKey = encryptKey.Substring(0,8);
            //encryptKey = encryptKey.PadRight(8, ' ');
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            byte[] rgbIV = DESKeys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }
        public static string DESDecode(this string decryptString, string decryptKey)
        {
            try
            {
                //decryptKey = decryptKey.SubString(8, "");
                //decryptKey = decryptKey.PadRight(8, ' ');
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = DESKeys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch { return string.Empty; }
        }
        public static string Base64Encode(this string encryptString)
        {
            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(encryptString);
            return Convert.ToBase64String(encbuff);
        }
        public static string Base64Decode(this string decryptString)
        {
            byte[] decbuff = Convert.FromBase64String(decryptString);
            return System.Text.Encoding.UTF8.GetString(decbuff);
        }
        public static string MD5(this string str)
        {
            byte[] result = Encoding.UTF8.GetBytes(str.Trim());
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", "");
        }
        #endregion

        #region json序列化
        public static string JsonSerialize(object obj)
        {
            return JsonSerialize(obj, 100);
        }
        public static string JsonSerialize(object obj, int recursionLimit)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.RecursionLimit = recursionLimit;
            return serializer.Serialize(obj);
        }
        public static T JsonDeserialize<T>(string json)
        {
            return JsonDeserizlize<T>(json, 100);
        }
        public static T JsonDeserizlize<T>(string json, int recursionLimit)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.RecursionLimit = recursionLimit;
            return serializer.Deserialize<T>(json);
        }
        #endregion

        /// <summary>
        ///  根据代码 返回操作结果实体
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="msg">详细描述</param>
        /// <returns>操作结果实体</returns>
        public static Result GetResultMsg(string code)
        {
            try
            {
                Result result = new Result();
                result.IsTrue = false;
                result.ResultCode = code;
                result.ResultMsg = GetError(code);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        ///  获取代码对应详细描述
        /// </summary>
        /// <param name="code">错误代码</param>
        /// <returns>详细描述</returns>
        public static string GetError(string code)
        {
            try
            {
                string error = string.Empty;
                switch (code)
                {
                    case "00": error = "成功"; break;
                    case "101": error = "内部操作失败"; break;
                    case "102": error = "参数存在非法数据或特殊字符"; break;
                    case "103": error = "此条码已存在"; break;
                    case "104": error = "参数空值"; break;
                    case "105": error = "签名不能为空"; break;
                    case "106": error = "签名失败"; break;
                    case "107": error = "市民卡提交json信息解析错误，请检查json格式和必选参数是否为空"; break;
                    case "108": error = "查无可售市民卡数据"; break;
                    case "109": error = "无此票种"; break;
                    case "110": error = "接口内部执行sql失败"; break;
                    case "111": error = "系统已存在此交易号"; break;
                    case "112": error = "获取账户失败"; break;
                    case "113": error = "账户金额不足"; break;
                    case "114": error = "门票:{0}在{1}库存不足"; break;
                    case "115": error = "{0}配额不足{1}"; break;
                    case "116": error = "景区不存在"; break;
                    case "117": error = "明细中存在入园日期不可入园门票"; break;
                    case "118": error = "景区无{0}配额"; break;
                    case "119": error = "不存在此订单信息"; break;
                    case "120": error = "存在明细状态为空"; break;
                    case "121": error = "此单已释放"; break;
                    case "122": error = "此单已完成支付操作"; break;
                    case "123": error = "释放订单操作失败"; break;
                    case "124": error = "是否由平台发送短信标识错误"; break;
                    case "125": error = "完成订单操作失败"; break;
                    case "126": error = "数据超过100"; break;
                    case "127": error = "修改操作类型为空"; break;
                    // case "148": error = "数据超过100"; break;
                    //  case "149": error = "修改操作类型为空"; break;
                    case "128": error = "公司信息异常"; break;
                    case "129": error = "类型错误"; break;//zcc 加
                    case "130": error = "未查到有效协议"; break;
                    case "131": error = "产品包ID为空"; break;
                    case "132": error = "未分配产品包"; break;
                    case "133": error = "游玩日期不能小于当天日期"; break;
                    case "134": error = "产品中存在无效线路"; break;
                    case "140": error = "该日期内库存不足"; break;//add by dyg 140-150:为酒店错误消息
                    case "141": error = "生成库存明细出错"; break;//add by dyg 140-150:为酒店错误消息
                    case "142": error = "HotelID不存在"; break;//add by dyg 140-150:为酒店错误消息
                    case "143": error = "RoomID不存在"; break;//add by dyg 140-150:为酒店错误消息
                    case "144": error = "入住房间数量和入住天数不匹配"; break;//add by dyg 140-150:为酒店错误消息
                    case "145": error = "【{0}】数量小于1"; break;// 20151028 czf 
                    case "146": error = "当天不可售"; break;//add by:cjt 当天不可售 20151125
                    case "147": error = "不存需重发短彩信的订单"; break;
                    //以下为剧场票相关错误消息  20150602   wjh    160-170
                    case "160": error = "剧场票未设置层数、区域、场次、座位等信息"; break;
                    case "161": error = "剧场票未设置剧场或已删除"; break;
                    case "162": error = "剧场票未设置层数或已删除"; break;
                    case "163": error = "剧场票未设置区域或已删除"; break;
                    case "164": error = "剧场票未设置场次或已删除"; break;
                    case "165": error = "剧场票未设置座位或已删除"; break;
                    case "166": error = "剧场票剩余座位数量不足"; break;
                }
                return error;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 检测签名
        /// 20150121
        /// czf
        /// </summary>
        /// <param name="signature">ota入参签名</param>
        /// <param name="signatureStr">接口签名字符串</param>
        /// <returns></returns>
        public static bool CheckSignature(string signature, string signatureStr)
        {
            //if (signature == Base64Encode(MD5(signatureStr)))
            //{
            //    return true;
            //}
            if (signature.ToUpper() == MD5(signatureStr))
            {
                return true;
            }
            return false;
        }
      
        private static string[] qStr = new string[] { "'", "delete", "create", "update", "select", "exec", "=" };
        /// <summary>
        /// 检测对象中值是否包含特殊字符
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsInjectionObj<T>(T obj)
        {
            var _Properties = obj.GetType().GetProperties();
            foreach (var item in _Properties)
            {
                foreach (string str in qStr)
                {
                    var ivalues = item.GetValue(obj, null);
                    if (ivalues != null && ivalues.ToString().IndexOf(str) >= 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
     
        /// <summary>
        /// 生成串码   2015-07-07
        /// </summary>
        /// <returns></returns>
        public static long CreateBarcode()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            //修改生成电子串码长度为12位 20150709 zm
            return long.Parse(BitConverter.ToInt64(buffer, 0).ToString().Substring(0, 12));
        }

        //默认日志文件夹地址 20151008 czf
        private static string DefaultLogPath = HttpContext.Current.Server.MapPath("\\DefLog");
        //判断是否存在文件 不存在则进行创建 20151008 czf
        public static void IsDirectory(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = DefaultLogPath;
            }
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
        }
        //判断文件是否存在，不存在则进行创建 20151010 czf
        public static string isLogFile(string filePath, string fileName)
        {
            filePath = GetFilePath(filePath, fileName);
            if (!File.Exists(filePath))
            {
                #region 注释
                //FileStream fs = File.Create(filePath);
                //Byte[] byteContent = Encoding.UTF8.GetBytes(fileContent);
                //fs.Write(byteContent, 0, byteContent.Length);
                #endregion
                FileStream fs = new FileStream(filePath, FileMode.CreateNew);
                fs.Close();
                fs.Dispose();
            }
            return filePath;
        }
        //获取对应文件地址 20151010 czf
        public static string GetFilePath(string filePath, string fileName)
        {
            if (fileName.PadRight(3) != ".txt")
                fileName += ".txt";
            if (filePath.PadRight(2) == "\\")
            {
                filePath += fileName;
            }
            else
            {
                filePath += "\\" + fileName;
            }
            return filePath;
        }
        public static void WriteLog(string filePath, string fileName, string fileContent)
        {
            #region
            //if (string.IsNullOrEmpty(filePath.Trim()))
            //{
            //    filePath = HttpContext.Current.Server.MapPath("\\DefLog");
            //    //return;
            //}
            //if (!Directory.Exists(filePath))
            //{
            //    Directory.CreateDirectory(filePath);
            //}
            #endregion
            //判断是否存在当前日志文件夹 20151008 czf
            if (string.IsNullOrEmpty(filePath.Trim()))
            {
                filePath = DefaultLogPath;
            }
            //判断是否存在当前路径文件加，不存在则进行创建 20151010 czf
            IsDirectory(filePath);
            filePath += "\\" + DateTime.Now.ToString("yyyy-MM-dd");
            //判断是否存在当前路径文件加，不存在则进行创建 20151010 czf
            IsDirectory(filePath);
            if (string.IsNullOrEmpty(fileName.Trim()))
            {
                fileName = DateTime.Now.ToString("yyyy-MM-dd");// +".txt";
                //return;
            }
            //else
            //{
            //    fileName += ".txt";
            //}
            //检索当前路径下 相应文件 20151010 czf
            string[] strFilesArry = Directory.GetFiles(filePath, fileName + "》*.txt");
            if (strFilesArry.Length == 0)
            {
                // 如果过不存在对应文件则进行创建 20151010 czf
                filePath = isLogFile(filePath, fileName + "》0");
            }
            else
            {
                int fileMax = 0;
                bool isTrue = false;
                //遍历所检索的对应文件名  20151010 czf
                foreach (string str in strFilesArry)
                {
                    string fileStr = str.Trim();
                    int indexnum = fileStr.LastIndexOf("》");
                    //判断文件为第几个文件 20151010 czf
                    if (fileStr.LastIndexOf("》") >= 0)
                    {
                        fileStr = fileStr.Substring(fileStr.LastIndexOf("》") + 1);
                        fileStr = fileStr.Split('.')[0].Trim();
                        int fileNum = Convert.ToInt32(fileStr);
                        if (fileNum >= fileMax)
                            fileMax = fileNum;
                        isTrue = true;
                    }
                }
                //获取最后创建的文件地址 20151010 czf
                string filePathStr = GetFilePath(filePath, fileName + "》" + fileMax.ToString());
                if (!isTrue)
                    filePathStr = GetFilePath(filePath, fileName + "》0");
                //获取对应文件信息 20151010 czf
                FileInfo fileInfo = new FileInfo(filePathStr);
                //判断文件是否大于设定值，如果大于则新建文件 20151010 czf
                int fileSize = 1;
                try
                {
                    fileSize = Convert.ToInt32(System.Configuration.ConfigurationManager.ConnectionStrings["LogFileSize"].ConnectionString);
                    if (fileSize < 1)
                        fileSize = 1;
                }
                catch (Exception ex) { fileSize = 1; }
                if (fileInfo.Length > 1024 * 1024 * fileSize)
                {
                    fileName += "》" + (fileMax + 1).ToString();
                    filePath = isLogFile(filePath, fileName);
                }
                else
                {
                    isLogFile(filePath, fileName + "》0");
                    filePath = filePathStr;

                }
            }

            StringBuilder rContent = new StringBuilder();
            rContent.Append("\r\n");
            rContent.Append("\r\n");
            rContent.Append("\r\n");
            rContent.Append("====================【开始】====================");
            rContent.Append("\r\n");
            rContent.Append("记录时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\r\n");
            rContent.Append(fileContent + "\r\n");
            rContent.Append("====================【结束】====================");
            FileStream nfs = new FileStream(filePath, FileMode.Append);
            StreamWriter sw = new StreamWriter(nfs, Encoding.UTF8);
            sw.Write(rContent);
            sw.Close();
            sw.Dispose();
            nfs.Close();
            nfs.Dispose();
            //删除 历史日志 20151010 czf
            DelLog();
        }
        //删除历史日志 20151010 czf
        public static void DelLog()
        {
            int delDays = 0;
            try
            {
                delDays = Convert.ToInt32(System.Configuration.ConfigurationManager.ConnectionStrings["DelLogDays"].ConnectionString);
                if (delDays < 2)
                    delDays = 10;
            }
            catch (Exception ex) { delDays = 10; }
            string ntime = DateTime.Now.AddDays(-delDays).ToString("yyyy-MM-dd");
            string filePath = DefaultLogPath + "\\" + ntime;
            if (Directory.Exists(filePath))
            {
                Directory.Delete(filePath, true);
            }
        }

        public static string PostInput()
        {
            try
            {
                #region
                System.IO.Stream s = HttpContext.Current.Request.InputStream;
                int count = 0;
                byte[] buffer = new byte[1024];
                StringBuilder builder = new StringBuilder();
                while ((count = s.Read(buffer, 0, 1024)) > 0)
                {
                    builder.Append(Encoding.GetEncoding("GBK").GetString(buffer, 0, count));
                }
                s.Flush();
                s.Close();
                s.Dispose();
                return builder.ToString();
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog("", "PostInput", "ex:" + ex.Message + "-" + HttpContext.Current.Request.Url.Query.ToString());
                return "";
            }
        }

        /// <summary>
        /// 解析json，根据传入的key取value
        /// </summary>
        /// <param name="str">json字符串</param>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public static string AnalysisByJson(string str, string key)
        {
            var obj = JsonDeserizlize<Dictionary<string, string>>(str, 100);

            if (obj != null && obj.Count > 0 && obj.Keys.Contains(key))
            {
                return obj[key];
            }
            else
            {
                return "-1";
            }
        }

        /// <summary>
        /// 根据sql语句返回dataset
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static DataSet GetDataSetBySql(string strSql)
        {
            //try
            //{
            //    var db = new OTAInterFace.OracleDB.OracleHelper();
            //    return db.ExecSQLDataSet(strSql);
            //}
            //catch (Exception ex)
            //{
            //    //return null;
            //    throw ex; //抛出异常，外层catch处理 20151202 cjt
            //}
            return null;
        }

        /// <summary>
        /// 根据字符串转换成为日期类型 如：20150424;
        /// 成功则返回日期格式，失败则返回(New DateTime())
        /// </summary>
        /// <param name="str">日期字符串:20150424</param>
        /// <param name="format">日期格式：默认(yyyyMMdd)</param>
        /// <returns></returns>
        public static DateTime FormatDateTimeByString(string str, string format)
        {

            CultureInfo provider = CultureInfo.InvariantCulture;
            if (string.IsNullOrEmpty(format))
            {
                format = "yyyyMMdd";
            }
            try
            {
                return DateTime.ParseExact(str, format, provider);
            }
            catch (FormatException ex)
            {
                return (new DateTime());
            }
        }

        /// <summary>
        /// 执行带事务的sql语句，成功返回true，失败返回false
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ExecSQLTransaction(string sql)
        {
            //OracleHelper db = new OracleHelper();
            //return db.ExecSQLTransaction(sql.ToString()) > 0 ? true : false;
            return false;
        }
        /// < summary> 
        /// 动态调用web服务 
        /// < /summary> 
        /// < param name="url">WSDL服务地址< /param> 
        /// < param name="methodname">方法名< /param> 
        /// < param name="args">参数< /param> 
        /// < returns>< /returns> 
        public static object InvokeWebService(string url, string methodname, object[] args)
        {
            return InvokeWebService(url, null, methodname, args);
        }
        private static string GetWsClassName(string wsUrl)
        {
            string[] parts = wsUrl.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');
            return pps[0];
        }
        /// < summary> 
        /// 动态调用web服务 
        /// < /summary> 
        /// < param name="url">WSDL服务地址< /param> 
        /// < param name="classname">类名< /param> 
        /// < param name="methodname">方法名< /param> 
        /// < param name="args">参数< /param> 
        /// < returns>< /returns> 
        public static object InvokeWebService(string url, string classname, string methodname, object[] args)
        {
            return null;
            //string @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
            //if ((classname == null) || (classname == ""))
            //{
            //    classname = GetWsClassName(url);
            //}
            //try
            //{ //获取WSDL 
            //    WebClient wc = new WebClient();
            //    Stream stream = wc.OpenRead(url + "?WSDL");
            //    ServiceDescription sd = ServiceDescription.Read(stream);
            //    ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
            //    sdi.AddServiceDescription(sd, "", "");
            //    CodeNamespace cn = new CodeNamespace(@namespace);
            //    //生成客户端代理类代码 
            //    CodeCompileUnit ccu = new CodeCompileUnit();
            //    ccu.Namespaces.Add(cn);
            //    sdi.Import(cn, ccu);
            //    CSharpCodeProvider icc = new CSharpCodeProvider();
            //    //设定编译参数 
            //    CompilerParameters cplist = new CompilerParameters();
            //    cplist.GenerateExecutable = false;
            //    cplist.GenerateInMemory = true;
            //    cplist.ReferencedAssemblies.Add("System.dll");
            //    cplist.ReferencedAssemblies.Add("System.XML.dll");
            //    cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
            //    cplist.ReferencedAssemblies.Add("System.Data.dll");
            //    //编译代理类 
            //    CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
            //    if (true == cr.Errors.HasErrors)
            //    {
            //        System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //        foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
            //        {
            //            sb.Append(ce.ToString());
            //            sb.Append(System.Environment.NewLine);
            //        }
            //        throw new Exception(sb.ToString());
            //    }
            //    //生成代理实例，并调用方法 
            //    System.Reflection.Assembly assembly = cr.CompiledAssembly;
            //    Type t = assembly.GetType(@namespace + "." + classname, true, true);
            //    object obj = Activator.CreateInstance(t);
            //    System.Reflection.MethodInfo mi = t.GetMethod(methodname);
            //    return mi.Invoke(obj, args);
            //    // PropertyInfo propertyInfo = type.GetProperty(propertyname); 
            //    //return propertyInfo.GetValue(obj, null); 
            //}
            //catch (Exception ex)
            //{
            //    Common.WriteLog("", DateTime.Now.ToString("yyyy-MM-dd"), ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace + "\r\n" + ex.TargetSite);
            //    throw ex; //throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
            //}
        }
        //获取两个时间差记录 20151008 czf
        public static string GetLogTimeSpan(DateTime stime, DateTime etime)
        {
            StringBuilder logInfo = new StringBuilder();
            TimeSpan htime = etime - stime;
            logInfo.AppendFormat("开始时间:{0}\r\n", stime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            logInfo.AppendFormat("结束时间:{0}\r\n", etime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            logInfo.AppendFormat("耗    时:{0}时{1}分{2}秒{3}毫秒", htime.Hours, htime.Minutes, htime.Seconds, htime.Milliseconds);
            return logInfo.ToString();
        }

    }
}
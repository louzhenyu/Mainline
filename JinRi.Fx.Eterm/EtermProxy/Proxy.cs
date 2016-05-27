using System;
using System.IO;
using System.Net;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Collections;
using EtermProxy.Utility;
using System.Reflection;
using Newtonsoft.Json;
using JetermEntity.Response;

namespace EtermProxy
{    
    [Guid("18F18C7A-AE06-45BD-9D5B-E277B4BD7C1E")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Proxy:IProxy
    {
        /// <summary>
        /// Eterm代理方法
        /// </summary>
        /// <param name="url">请求url</param>
        /// <returns>返回结果</returns>
        public string InvokeEterm(IntPtr hwnd, IntPtr handle, string strPost,string strParam)
        {
            string sret = string.Empty;

            try
            {                
                EtermRequest ereq = JsonConvert.DeserializeObject<EtermRequest>(strPost);

                if (ereq != null)
                {       
                    Type bllType = Type.GetType(string.Format("EtermProxy.BLL.{0}", ereq.ClassName));
                    Type reqType = Type.GetType(string.Format("JetermEntity.Request.{0},JetermEntity", ereq.ClassName));

                    object reqObj = JsonConvert.DeserializeObject(strParam, reqType);
                    if (bllType != null && reqObj != null)
                    {
                        object obj = Activator.CreateInstance(bllType, new object[] { hwnd, handle, ereq.Config, ereq.OfficeNo });

                        if (obj != null)
                        {
                            MethodInfo info = bllType.GetMethod("BusinessDispose");
                            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance;
                            object objret = null;
                            try
                            {
                                objret = info.Invoke(obj, flag, Type.DefaultBinder, new object[] { reqObj }, null);
                            }
                            catch (TargetInvocationException targetEx)
                            {
                                LogWrite.WriteLog(targetEx);
                            }
                            if (objret!=null) sret = Newtonsoft.Json.JsonConvert.SerializeObject(objret);
                        }
                    }
                }
                return sret;
            }
            catch (Exception ex)
            {
                LogWrite.WriteLog(ex);
            }
            return sret;
        }


        /// <summary>
        /// 设置Eterm数据
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="data"></param>
        public void SetEtermData(string config,string guid, string cmd, string data)
        {
            EtermHelper.EtermData[config + guid + cmd] = data;

            LogWrite.WriteLog(string.Format("config={0} guid={1} cmd={2}\r\ndata={3}", config, guid, cmd, data));
        }

        /// <summary>
        /// 请求参数对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        private object getObject(string typeName, List<string> param)
        {
            Type type = Type.GetType(typeName);

            object result = Activator.CreateInstance(type);

            int index = 0;            
            foreach (PropertyInfo info in type.GetProperties())
            {
                if (index < param.Count)
                {
                    object obj = null;
                    if (info.PropertyType.IsEnum)
                    {
                        obj = Enum.Parse(info.PropertyType, param[index]);
                    }
                    else
                    {
                        obj = Convert.ChangeType(param[index], info.PropertyType);
                    }
                    info.SetValue(result, obj, null);
                }
                index++;
            }
            return result;

            

        }

    }

}

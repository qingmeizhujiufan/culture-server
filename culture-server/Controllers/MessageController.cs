using culture_server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace culture_server.Controllers
{
    public class MessageController : ApiController
    {
        #region 获取用户消息列表
        /// <summary>  
        /// 获取用户消息列表 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryList(string userId)
        {
            
            Object data;
            try
            {
                DataTable dt = new BLL.handleMessage().queryList(userId);

                if (dt.Rows.Count >= 0)
                {
                    List<message> list = new List<message>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        list.Add(generateMessage(dt.Rows[i]));
                    }

                    data = new
                    {
                        success = true,
                        backData = list
                    };
                }
                else
                {
                    data = new
                    {
                        success = false,
                        backMsg = "数据异常"
                    };
                }
            }
            catch (Exception e)
            {
                data = new
                {
                    success = false,
                    backMsg = "服务异常"
                };
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(data);
            return new HttpResponseMessage
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        }
        #endregion

        #region 删除消息
        /// <summary>  
        /// 删除消息
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage delete(dynamic d)
        {
            object data = new object();
            try
            {
                BLL.handleMessage message = new BLL.handleMessage();
                bool flag = false;

                flag = message.delete(d);

                if (flag)
                {
                    data = new
                    {
                        success = true
                    };
                }
                else
                {
                    data = new
                    {
                        success = false,
                        backMsg = "删除消息失败"

                    };
                }
            }
            catch (Exception ex)
            {
                data = new
                {
                    success = false,
                    backMsg = "服务异常"

                };
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(data);
            return new HttpResponseMessage
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        }
        #endregion

        #region 私有方法集
        //返回message对象
        private message generateMessage(dynamic d)
        {
            message n = new message();
            n.id = d["id"].ToString();
            n.receiver = d["receiver"].ToString();
            n.messageTitle = d["messageTitle"].ToString();
            n.messageContent = d["messageContent"].ToString();
            n.type = Convert.ToInt32(d["type"].ToString());
            n.create_time = d["create_time"].ToString();

            return n;
        }
        #endregion
    }
}

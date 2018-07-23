using culture_server.Core;
using culture_server.Models;
using culture_server.Util;
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
    public class UserController : ApiController
    {
        #region 获取用户列表
        /// <summary>  
        /// 获取用户列表 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryList(dynamic d)
        {
            DataTable dt = new BLL.handleUser().queryList();
            Object data;
            if (dt.Rows.Count >= 0)
            {
                List<user> list = new List<user>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(generateUser(dt.Rows[i]));
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

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(data);
            return new HttpResponseMessage
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        }
        #endregion

        #region 获取用户信息详情
        /// <summary>  
        /// 获取用户信息详情 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryDetail(string id)
        {
            DataTable dt = new BLL.handleUser().queryDetail(id);
            Object data;
            if (dt.Rows.Count == 1)
            {
                data = new
                {
                    success = true,
                    backData = generateUser(dt.Rows[0])
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

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(data);
            return new HttpResponseMessage
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        }
        #endregion

        #region 更新或者新增用户信息
        /// <summary>  
        /// 更新或者新增用户信息 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [SupportFilter]
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage save(dynamic d)
        {
            Object data;

            try
            {
                BLL.handleUser user = new BLL.handleUser();
                bool flag = false;
                flag = user.saveAP(d);

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
                        backMsg = "保存信息失败"

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

        #region 删除用户
        /// <summary>  
        /// 删除用户
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [SupportFilter]
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage delete(dynamic d)
        {
            string ids = d.ids;
            object data = new object();
            try
            {
                BLL.handleUser user = new BLL.handleUser();
                bool flag = false;

                flag = user.delete(ids);

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
                        backMsg = "删除用户失败"

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

        #region 最近注册用户统计
        /// <summary>  
        /// 最近注册用户统计
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [SupportFilter]
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage getNewlyRegisterUserData(string type)
        {
            DataTable dt = new BLL.handleUser().getNewlyRegisterUserData(type);
            Object data;
            if (dt.Rows.Count >= 0)
            {
                List<userCount> list = new List<userCount>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    userCount count = new userCount();
                    count.id = dt.Rows[i]["id"].ToString();
                    count.countDate = dt.Rows[i]["countDate"].ToString();
                    count.num = Convert.ToInt32(dt.Rows[i]["num"].ToString());

                    list.Add(count);
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

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(data);
            return new HttpResponseMessage
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        }
        #endregion     

        #region 私有方法集
        //返回user对象
        private user generateUser(dynamic d)
        {
            user n = new user();
            n.id = d["id"].ToString();
            n.avatar = util.generateImage(d["avatar"].ToString());
            n.sex = d["sex"].ToString();
            n.userName = d["userName"].ToString();
            n.nickName = d["nickName"].ToString();
            n.telephone = d["telephone"].ToString();
            n.state = Convert.ToInt32(d["state"].ToString());
            n.isDelete = Convert.ToInt32(d["isDelete"].ToString());
            n.create_time = d["create_time"].ToString();

            return n;
        }
        #endregion
    }
}

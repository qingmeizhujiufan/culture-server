using CommonTool;
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
using System.Web.Security;

namespace culture_server.Controllers
{
    public class UserController : ApiController
    {
        #region 微信登录授权
        /// <summary>  
        /// 管理员登录授权  
        /// </summary>  
        /// <param name="user">用户实体</param> 
        /// <returns></returns>  
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage login(dynamic user)
        {
            if (user == null)
            {
                return new HttpResponseMessage
                {
                    Content = new StringContent("", System.Text.Encoding.UTF8, "application/json")
                };
            }
            string code = Convert.ToString(user.code);
            WxUser wxUser = CommonTool.WXOperate.GetWxUser(code);
            var data = new object { };
            if (string.IsNullOrEmpty(wxUser.openid))
            {
                data = new
                {
                    success = false,
                    backMsg = "登录失败，请重试！"
                };
            }
            else
            {
                DataTable dt_user = new BLL.handleUser().findUserByOpenId(wxUser.openid);
                user _user = new user();
                _user.openid = wxUser.openid;
                _user.nickname = wxUser.nickname;
                _user.sex = wxUser.sex;
                _user.province = wxUser.province;
                _user.city = wxUser.city;
                _user.country = wxUser.country;
                _user.headimgurl = wxUser.headimgurl;
                _user.unionid = wxUser.unionid;

                if (dt_user.Rows.Count == 1)
                {
                    _user.id = dt_user.Rows[0]["id"].ToString();
                    new BLL.handleUser().update(_user);
                }
                else
                {
                    _user.id = System.Guid.NewGuid().ToString();                  
                    new BLL.handleUser().insert(_user);
                }

                FormsAuthenticationTicket token = new FormsAuthenticationTicket(0, wxUser.openid, DateTime.Now,
                            DateTime.Now.AddHours(3), true, string.Format("{0}", wxUser.openid),
                            FormsAuthentication.FormsCookiePath);
                //返回登录结果、用户信息、用户验证票据信息
                var Token = FormsAuthentication.Encrypt(token);
                //将身份信息保存在数据库中，验证当前请求是否是有效请求
                string str_token = @"insert into dbo.c_token (userId, token, expireDate) values ('{0}', '{1}', '{2}')";
                str_token = string.Format(str_token, _user.id, Token, DateTime.Now.AddHours(3));
                DBHelper.SqlHelper.ExecuteSql(str_token);

                data = new
                {
                    success = true,
                    token = Token,
                    backData = new
                    {
                        userInfo = _user
                    }
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

        #region 用户退出登录，清空Token
        /// <summary>  
        /// 用户退出登录，清空Token  
        /// </summary>  
        /// <param name="id">用户ID</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage LoginOut(dynamic id)
        {
            if (id == null)
            {
                return new HttpResponseMessage
                {
                    Content = new StringContent("", System.Text.Encoding.UTF8, "application/json")
                };
            }
            string userId = Convert.ToString(id.userId);
            int flag = 0;
            try
            {
                //清空数据库该用户票据数据  
                string str_clear = @"delete dbo.c_token where userId='{0}'";
                str_clear = string.Format(str_clear, userId);
                flag = DBHelper.SqlHelper.ExecuteSql(str_clear);
            }
            catch (Exception ex) { }
            //返回信息
            var data = new object { };
            if (flag > 0)
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
                    backMsg = "退出失败，请重试！"
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

        #region 发送邮件用于重置密码
        /// <summary>  
        /// 发送邮件用于重置密码
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage SendEmail()
        {
            object data = new object();
            try
            {
                BLL.handleUser user = new BLL.handleUser();
                bool flag = false;

                flag = CommonTool.MailHelper.SendEmailDefault("616028858@qq.com", "123", "456", "789");

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
                        backMsg = "邮件发送失败"

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
            n.openid = d["openid"].ToString();
            n.nickname = d["nickname"].ToString();
            n.sex = d["sex"].ToString();
            n.province = d["province"].ToString();
            n.city = d["city"].ToString();
            n.country = d["country"].ToString();
            n.headimgurl = d["headimgurl"].ToString();
            n.unionid = d["unionid"].ToString();
            n.state = Convert.ToInt32(d["state"].ToString());
            n.isDelete = Convert.ToInt32(d["isDelete"].ToString());
            n.create_time = d["create_time"].ToString();

            return n;
        }
        #endregion
    }
}

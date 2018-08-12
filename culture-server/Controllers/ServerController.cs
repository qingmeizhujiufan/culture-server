using culture_server.Core;
using culture_server.Models;
using culture_server.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.SessionState;

namespace culture_server.Controllers
{
    public class SessionRouteHandler : HttpControllerHandler, IRequiresSessionState
    {
        public SessionRouteHandler(RouteData routeData)
            : base(routeData)
        {

        }
    }

    public class SessionControllerRouteHandler : HttpControllerRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new SessionRouteHandler(requestContext.RouteData);
        }
    }

    public class ServerController : ApiController
    {
        #region 管理员登录授权
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
            string userName = Convert.ToString(user.userName);
            string userPwd = Convert.ToString(user.userPwd);
            string strSql = @"select    id, type, typeName
                                        from dbo.c_admin
                                        where userName = '{0}' and userPwd = '{1}'";
            strSql = string.Format(strSql, userName, userPwd);
            DataTable dt_user = DBHelper.SqlHelper.GetDataTable(strSql);
            var data = new object { };
            if (dt_user.Rows.Count == 1)
            {
                FormsAuthenticationTicket token = new FormsAuthenticationTicket(0, userName, DateTime.Now,
                            DateTime.Now.AddHours(5), true, string.Format("{0}&{1}", userName, userPwd),
                            FormsAuthentication.FormsCookiePath);
                //返回登录结果、用户信息、用户验证票据信息
                var Token = FormsAuthentication.Encrypt(token);
                //将身份信息保存在数据库中，验证当前请求是否是有效请求
                string str_token = @"insert into dbo.c_token (userId, token, expireDate) values ('{0}', '{1}', '{2}')";
                str_token = string.Format(str_token, dt_user.Rows[0]["id"], Token, DateTime.Now.AddHours(3));
                DBHelper.SqlHelper.ExecuteSql(str_token);

                data = new
                {
                    success = true,
                    token = Token,
                    userId = dt_user.Rows[0]["id"],
                    type = dt_user.Rows[0]["type"],
                    typeName = dt_user.Rows[0]["typeName"],
                    expireDate = DateTime.Now.AddHours(3).ToString()
                };
            }
            else
            {
                data = new
                {
                    success = false,
                    backMsg = "身份验证失败，请重试！"
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

        #region 管理员退出登录，清空Token
        /// <summary>  
        /// 管理员退出登录，清空Token  
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
                    backMsg = "安全退出失败，请重试！"
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

        #region 获取首页文化展示轮播图详情
        /// <summary>  
        /// 获取首页文化展示轮播图详情 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryHomeCulutreDetail()
        {
            DataTable dt = new BLL.Common().queryHomeCulutreDetail();
            Object data;
            if (dt.Rows.Count == 1)
            {
                data = new
                {
                    success = true,
                    backData = generateHomeCulture(dt.Rows[0])
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

        #region 首页文化展示轮播图保存
        /// <summary>  
        /// 首页文化展示轮播图保存 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [SupportFilter]
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage saveHomeSlider(dynamic d)
        {
            Object data;

            try
            {
                BLL.Common common = new BLL.Common();
                bool flag = false;
                flag = common.saveHomeSlider(d);

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

        #region 获取背景音乐
        /// <summary>  
        /// 获取背景音乐 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryMusic()
        {
            DataTable dt = new BLL.Common().queryMusic();
            Object data;
            if (dt.Rows.Count == 1)
            {
                data = new
                {
                    success = true,
                    music = generateMusic(dt.Rows[0])
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

        #region 背景音乐保存
        /// <summary>  
        /// 背景音乐保存 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [SupportFilter]
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage saveMusic(dynamic d)
        {
            Object data;

            try
            {
                BLL.Common common = new BLL.Common();
                bool flag = false;
                flag = common.saveMusic(d);

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

        #region 总量统计
        /// <summary>  
        /// 总量统计
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [SupportFilter]
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage getWebTotal()
        {
            DataTable dt = new BLL.Common().getWebTotal();
            Object data;
            if (dt.Rows.Count > 0)
            {
                data = new
                {
                    success = true,
                    backData = new {
                        cultureTotal = dt.Rows[0][0],
                        artTotal = dt.Rows[1][0],
                        tasteTotal = dt.Rows[2][0],
                        videoTotal = dt.Rows[3][0]
                    }                  
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

        #region 文化，艺术品，新闻，图片，视频浏览分布统计
        /// <summary>  
        /// 文化，艺术品，新闻，图片，视频浏览分布统计
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [SupportFilter]
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage countCAN()
        {
            DataTable dt = new BLL.Common().countCAN();
            Object data;
            if (dt.Rows.Count > 0)
            {
                data = new
                {
                    success = true,
                    backData = new
                    {
                        cultureTotal = dt.Rows[0][0],
                        artTotal = dt.Rows[1][0],
                        newsTotal = dt.Rows[2][0],
                        tasteTotal = dt.Rows[3][0],
                        videoTotal = dt.Rows[4][0]
                    }
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
        //返回homeCulture对象
        private homeCulture generateHomeCulture(dynamic d)
        {
            homeCulture t = new homeCulture();
            t.slider_1 = util.generateImage(d["slider_1"].ToString());
            t.slider_2 = util.generateImage(d["slider_2"].ToString());
            t.slider_3 = util.generateImage(d["slider_3"].ToString());
            t.slider_4 = util.generateImage(d["slider_4"].ToString());
            t.slider_5 = util.generateImage(d["slider_5"].ToString());
            t.slider_6 = util.generateImage(d["slider_6"].ToString());
            t.slider_7 = util.generateImage(d["slider_7"].ToString());
            t.slider_8 = util.generateImage(d["slider_8"].ToString());
            t.slider_9 = util.generateImage(d["slider_9"].ToString());

            return t;
        }

        //返回music对象
        private music generateMusic(dynamic d)
        {
            music t = new music();
            t.bgMusic = util.generateImage(d["bgMusic"].ToString());

            return t;
        }
        #endregion
    }
}
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
    public class CultureController : ApiController
    {
        #region 获取文化列表
        /// <summary>  
        /// 获取文化列表 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryList(int pageNumber, int pageSize, string conditionText, string cityId)
        {
            DataTable dt = new BLL.handleCulture().queryList(pageNumber, pageSize, conditionText, cityId);
            Object data;
            if (dt.Rows.Count >= 0)
            {
                List<culture> list = new List<culture>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(generateCulture(dt.Rows[i]));
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

        #region 获取管理文化列表
        /// <summary>  
        /// 获取管理文化列表 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryListByAdmin(string userId)
        {
            DataTable dt = new BLL.handleCulture().queryListByAdmin(userId);
            Object data;
            if (dt.Rows.Count >= 0)
            {
                List<culture> list = new List<culture>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(generateCulture(dt.Rows[i]));
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

        #region 获取文化信息详情
        /// <summary>  
        /// 获取文化信息详情 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryDetail(string id)
        {
            new BLL.Common().insertRead(id);
            DataTable dt = new BLL.handleCulture().queryDetail(id);
            Object data;
            if (dt.Rows.Count == 1)
            {
                data = new
                {
                    success = true,
                    backData = generateCulture(dt.Rows[0])
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

        #region 更新或者新增文化信息
        /// <summary>  
        /// 更新或者新增文化信息 
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
                BLL.handleCulture culture = new BLL.handleCulture();
                bool flag = false;
                flag = culture.saveAP(d);

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

        #region 删除文化
        /// <summary>  
        /// 删除文化
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage delete(dynamic d)
        {
            object data = new object();
            try
            {
                BLL.handleCulture culture = new BLL.handleCulture();
                bool flag = false;

                flag = culture.delete(d);

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
                        backMsg = "删除失败"

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

        #region 审核文化
        /// <summary>  
        /// 审核文化 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [SupportFilter]
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage review(dynamic d)
        {
            Object data;

            try
            {
                BLL.handleCulture culture = new BLL.handleCulture();
                int flag = culture.review(d);

                if (flag == 1)
                {
                    data = new
                    {
                        success = true
                    };
                }
                else if (flag == 2)
                {
                    data = new
                    {
                        success = false,
                        backMsg = "审核失败"

                    };
                }
                else if (flag == -1)
                {
                    data = new
                    {
                        success = false,
                        backMsg = "不允许审核"

                    };
                }
                else if (flag == 0)
                {
                    data = new
                    {
                        success = false,
                        backMsg = "新闻不存在"

                    };
                }
                else
                {
                    data = new
                    {
                        success = false,
                        backMsg = "系统异常，请重试！"

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

        #region 收藏文化
        /// <summary>  
        /// 收藏文化 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage collect(dynamic d)
        {
            Object data;
            string userId = string.Empty;
            if (d != null)
            {
                userId = d.userId;
            }

            if (string.IsNullOrEmpty(userId))
            {
                data = new
                {
                    success = false,
                    backMsg = "请先登录再操作！"
                };
            }
            else
            {
                try
                {
                    BLL.handleCulture culture = new BLL.handleCulture();
                    bool flag = culture.collect(d);

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
                            backMsg = "操作失败，请重试！"

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
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(data);
            return new HttpResponseMessage
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        }
        #endregion

        #region 是否设置文化为推荐
        /// <summary>  
        /// 是否设置文化为推荐 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [SupportFilter]
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage settingRecommend(dynamic d)
        {
            Object data;

            try
            {
                BLL.handleCulture culture = new BLL.handleCulture();
                bool flag = false;
                flag = culture.settingRecommend(d);

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

        #region 获取Top5 推荐
        /// <summary>  
        /// 获取Top5 推荐 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryRecommendTop5()
        {
            DataTable dt = new BLL.handleCulture().queryRecommendTop5();
            Object data;
            if (dt.Rows.Count >= 0)
            {
                List<culture> list = new List<culture>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(generateCulture(dt.Rows[i]));
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

        #region 获取用户收藏文化
        /// <summary>  
        /// 获取用户收藏文化
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryUserCollectCulture(string userId)
        {
            DataTable dt = new BLL.handleCulture().queryUserCollectCulture(userId);
            Object data;
            if (dt.Rows.Count >= 0)
            {
                List<culture> list = new List<culture>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(generateCulture(dt.Rows[i]));
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

        #region 获取评论列表
        /// <summary>  
        /// 获取评论列表 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryCommentList(string cultureId)
        {
            DataTable dt = new BLL.handleCulture().queryCommentList(cultureId);
            Object data;
            if (dt.Rows.Count >= 0)
            {
                List<cultureComment> list = new List<cultureComment>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(generateCultureComment(dt.Rows[i]));
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

        #region 保存评论信息
        /// <summary>  
        /// 保存评论信息 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage add(dynamic d)
        {
            Object data;

            try
            {
                BLL.handleCulture culture = new BLL.handleCulture();
                bool flag = false;
                flag = culture.add(d);

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

        #region 批量删除评论
        /// <summary>  
        /// 批量删除评论
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [SupportFilter]
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage deleteComment(dynamic d)
        {
            string ids = d.ids;
            object data = new object();
            try
            {
                BLL.handleCulture culture = new BLL.handleCulture();
                bool flag = false;

                flag = culture.deleteComment(ids);

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
                        backMsg = "删除失败"

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

        #region 删除用户收藏的文化
        /// <summary>  
        /// 删除用户收藏的文化
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage delete2(dynamic d)
        {
            object data = new object();
            try
            {
                BLL.handleCulture culture = new BLL.handleCulture();
                bool flag = false;

                flag = culture.delete2(d);

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
                        backMsg = "删除失败"

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

        #region 获取管理评论列表
        /// <summary>  
        /// 获取管理评论列表 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryAdminCommentList()
        {
            DataTable dt = new BLL.handleCulture().queryAdminCommentList();
            Object data;
            if (dt.Rows.Count >= 0)
            {
                List<cultureComment> list = new List<cultureComment>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(generateCultureComment(dt.Rows[i]));
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
        //返回culture对象
        private culture generateCulture(dynamic d)
        {
            culture n = new culture();
            n.id = d["id"].ToString();
            n.cityId = d["cityId"].ToString();
            n.cityName = d["cityName"].ToString();
            n.cultureType = d["cultureType"].ToString();
            n.cultureTitle = d["cultureTitle"].ToString();
            n.cultureCover = util.generateImage(d["cultureCover"].ToString());
            n.cultureContent = d["cultureContent"].ToString();
            n.cultureAuthor = d["cultureAuthor"].ToString();
            n.cultureBrief = d["cultureBrief"].ToString();
            n.state = Convert.ToInt32(d["state"].ToString());
            n.readNum = Convert.ToInt32(d["readNum"].ToString());
            n.collectNum = Convert.ToInt32(d["collectNum"].ToString());
            n.isRecommend = Convert.ToInt32(d["isRecommend"].ToString());
            n.updator = d["updator"].ToString();
            n.updatorName = d["updatorName"].ToString();
            n.update_time = d["update_time"].ToString();
            n.creator = d["creator"].ToString();
            n.creatorName = d["creatorName"].ToString();
            n.typeName = d["typeName"].ToString();
            n.create_time = d["create_time"].ToString();

            return n;
        }

        //返回comment对象
        private cultureComment generateCultureComment(dynamic d)
        {
            cultureComment t = new cultureComment();
            t.id = d["id"].ToString();
            t.pId = d["pId"].ToString();
            t.cultureId = d["cultureId"].ToString();
            t.userId = d["userId"].ToString();
            t.avatar = util.generateImage(d["avatar"].ToString());
            t.userName = d["userName"].ToString();
            t.comment = d["comment"].ToString();
            t.create_time = d["create_time"].ToString();

            return t;
        }
        #endregion
    }
}

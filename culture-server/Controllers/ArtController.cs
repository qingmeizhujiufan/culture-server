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
    public class ArtController : ApiController
    {
        #region 获取艺术品列表
        /// <summary>  
        /// 获取艺术品列表 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryList(int pageNumber, int pageSize, string conditionText, string cityId)
        {
            DataTable dt = new BLL.handleArt().queryList(pageNumber, pageSize, conditionText, cityId);
            Object data;
            if (dt.Rows.Count >= 0)
            {
                List<art> list = new List<art>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(generateArt(dt.Rows[i]));
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

        #region 获取管理艺术品列表
        /// <summary>  
        /// 获取管理艺术品列表 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryListByAdmin(dynamic d)
        {
            DataTable dt = new BLL.handleArt().queryListByAdmin();
            Object data;
            if (dt.Rows.Count >= 0)
            {
                List<art> list = new List<art>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(generateArt(dt.Rows[i]));
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

        #region 获取艺术信息详情
        /// <summary>  
        /// 获取艺术信息详情 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryDetail(string id)
        {
            new BLL.Common().insertRead(id);
            DataTable dt = new BLL.handleArt().queryDetail(id);
            Object data;
            if (dt.Rows.Count == 1)
            {
                data = new
                {
                    success = true,
                    backData = generateArt(dt.Rows[0])
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

        #region 更新或者新增艺术信息
        /// <summary>  
        /// 更新或者新增艺术信息 
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
                BLL.handleArt culture = new BLL.handleArt();
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

        #region 删除艺术
        /// <summary>  
        /// 删除艺术
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage delete(dynamic d)
        {
            string id = d.id;
            object data = new object();
            try
            {
                BLL.handleArt culture = new BLL.handleArt();
                bool flag = false;

                flag = culture.delete(id);

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
                        backMsg = "删除新闻信息失败"

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

        #region 审核艺术品
        /// <summary>  
        /// 审核艺术品 
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
                BLL.handleArt art = new BLL.handleArt();
                int flag = art.review(d);

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
                        backMsg = "艺术品不存在"

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

        #region 收藏艺术品
        /// <summary>  
        /// 收藏艺术品 
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
                    BLL.handleArt art = new BLL.handleArt();
                    bool flag = art.collect(d);

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

        #region 是否设置艺术品为推荐
        /// <summary>  
        /// 是否设置艺术品为推荐 
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
                BLL.handleArt art = new BLL.handleArt();
                bool flag = false;
                flag = art.settingRecommend(d);

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

        #region 获取Top3 推荐
        /// <summary>  
        /// 获取Top3 推荐 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryRecommendTop3()
        {
            DataTable dt = new BLL.handleArt().queryRecommendTop3();
            Object data;
            if (dt.Rows.Count >= 0)
            {
                List<art> list = new List<art>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(generateArt(dt.Rows[i]));
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

        #region 获取用户收藏艺术品
        /// <summary>  
        /// 获取用户收藏艺术品
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryUserCollectArt(string userId)
        {
            DataTable dt = new BLL.handleArt().queryUserCollectArt(userId);
            Object data;
            if (dt.Rows.Count >= 0)
            {
                List<art> list = new List<art>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(generateArt(dt.Rows[i]));
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
        public HttpResponseMessage queryCommentList(string artId)
        {
            DataTable dt = new BLL.handleArt().queryCommentList(artId);
            Object data;
            if (dt.Rows.Count >= 0)
            {
                List<artComment> list = new List<artComment>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(generateArtComment(dt.Rows[i]));
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
                BLL.handleArt art = new BLL.handleArt();
                bool flag = false;
                flag = art.add(d);

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

        #region 删除用户收藏的艺术品
        /// <summary>  
        /// 删除用户收藏的艺术品
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage delete2(dynamic d)
        {
            object data = new object();
            try
            {
                BLL.handleArt art = new BLL.handleArt();
                bool flag = false;

                flag = art.delete2(d);

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

        #region 私有方法集
        //返回art对象
        private art generateArt(dynamic d)
        {
            art n = new art();
            n.id = d["id"].ToString();
            n.cityId = d["cityId"].ToString();
            n.cityName = d["cityName"].ToString();
            n.artType = d["artType"].ToString();
            n.artTitle = d["artTitle"].ToString();
            n.artCover = util.generateListImage(d["artCover"].ToString());
            n.artMoney = Convert.ToSingle(d["artMoney"].ToString());
            n.buyUrl = d["buyUrl"].ToString();
            n.artContent = util.generateListImage(d["artContent"].ToString());
            n.artAuthor = d["artAuthor"].ToString();
            n.artBrief = d["artBrief"].ToString();
            n.state = Convert.ToInt32(d["state"].ToString());
            n.readNum = Convert.ToInt32(d["readNum"].ToString());
            n.isCollect = Convert.ToInt32(d["isCollect"].ToString());
            n.isRecommend = Convert.ToInt32(d["isRecommend"].ToString());
            n.updator = d["updator"].ToString();
            n.updatorName = d["updatorName"].ToString();
            n.update_time = d["update_time"].ToString();
            n.creator = d["creator"].ToString();
            n.creatorName = d["creatorName"].ToString();
            n.create_time = d["create_time"].ToString();

            return n;
        }

        //返回artComment对象
        private artComment generateArtComment(dynamic d)
        {
            artComment t = new artComment();
            t.id = d["id"].ToString();
            t.pId = d["pId"].ToString();
            t.artId = d["artId"].ToString();
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

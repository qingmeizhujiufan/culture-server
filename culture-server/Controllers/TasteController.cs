﻿using culture_server.Core;
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
    public class TasteController : ApiController
    {
        #region 获取兴趣圈图片列表
        /// <summary>  
        /// 获取兴趣圈图片列表 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryList(string userId, int pageNumber, int pageSize)
        {
            DataTable dt = new BLL.handleTaste().queryList(userId, pageNumber, pageSize);
            Object data;
            if (dt.Rows.Count >= 0)
            {
                List<taste> list = new List<taste>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(generateTaste(dt.Rows[i]));
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

        #region 获取管理兴趣圈图片列表
        /// <summary>  
        /// 获取管理兴趣圈图片列表 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryListByAdmin(dynamic d)
        {
            DataTable dt = new BLL.handleTaste().queryListByAdmin();
            Object data;
            if (dt.Rows.Count >= 0)
            {
                List<taste> list = new List<taste>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(generateTaste(dt.Rows[i]));
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

        #region 获取图片详情
        /// <summary>  
        /// 获取图片详情 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryDetail(string id)
        {
            DataTable dt = new BLL.handleNews().queryDetail(id);
            Object data;
            if (dt.Rows.Count == 1)
            {
                data = new
                {
                    success = true,
                    backData = generateTaste(dt.Rows[0])
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

        #region 更新或者新增兴趣图片
        /// <summary>  
        /// 更新或者新增兴趣图片 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage save(dynamic d)
        {
            Object data;

            try
            {
                BLL.handleTaste taste = new BLL.handleTaste();
                bool flag = false;
                flag = taste.saveAP(d);

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

        #region 删除图片
        /// <summary>  
        /// 删除图片
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [SupportFilter]
        [AcceptVerbs("OPTIONS", "POST")]
        public HttpResponseMessage delete(dynamic d)
        {
            string id = d.id;
            object data = new object();
            try
            {
                BLL.handleTaste taste = new BLL.handleTaste();
                bool flag = false;

                flag = taste.delete(id);

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
                        backMsg = "删除图片失败"

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

        #region 审核图片
        /// <summary>  
        /// 审核图片 
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
                BLL.handleTaste taste = new BLL.handleTaste();
                int flag = taste.review(d);

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

        #region 收藏图片
        /// <summary>  
        /// 收藏图片 
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
                    BLL.handleTaste taste = new BLL.handleTaste();
                    bool flag = taste.collect(d);

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

        #region 获取收藏或者评论TOP 10
        /// <summary>  
        /// 获取收藏或者评论TOP 10
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [SupportFilter]
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryRankingListTop10(string type)
        {
            DataTable dt = new BLL.handleTaste().queryRankingListTop10(type);
            Object data;
            if (dt.Rows.Count >= 0)
            {
                List<taste> list = new List<taste>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(generateTaste(dt.Rows[i]));
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
        //返回taste对象
        private taste generateTaste(dynamic d)
        {
            taste n = new taste();
            n.id = d["id"].ToString();
            n.tasteCover = util.generateImage(d["tasteCover"].ToString());
            n.tasteBrief = d["tasteBrief"].ToString();
            n.isLike = Convert.ToInt32(d["isLike"].ToString());
            n.likeNum = Convert.ToInt32(d["likeNum"].ToString());
            n.commentNum = Convert.ToInt32(d["commentNum"].ToString());
            n.state = Convert.ToInt32(d["state"].ToString());
            n.avatar = util.generateImage(d["avatar"].ToString());
            n.updator = d["updator"].ToString();
            n.update_time = d["update_time"].ToString();
            n.creator = d["creator"].ToString();
            n.creatorName = d["creatorName"].ToString();
            n.create_time = d["create_time"].ToString();

            return n;
        }
        #endregion
    }
}

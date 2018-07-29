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
    public class NewsController : ApiController
    {
        #region 获取新闻列表
        /// <summary>  
        /// 获取新闻列表 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryList(int pageNumber, int pageSize, string conditionText, string cityId)
        {
            DataTable dt = new BLL.handleNews().getNewsList(pageNumber, pageSize, conditionText, cityId);
            Object data;
            if (dt.Rows.Count >= 0)
            {
                List<news> list = new List<news>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(generateNews(dt.Rows[i]));
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

        #region 获取管理新闻列表
        /// <summary>  
        /// 获取管理新闻列表 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryListByAdmin(dynamic d)
        {
            DataTable dt = new BLL.handleNews().queryListByAdmin();
            Object data;
            if (dt.Rows.Count >= 0)
            {
                List<news> list = new List<news>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(generateNews(dt.Rows[i]));
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

        #region 获取新闻信息详情
        /// <summary>  
        /// 获取新闻信息详情 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryDetail(string id)
        {
            new BLL.Common().insertRead(id);
            DataTable dt = new BLL.handleNews().queryDetail(id);
            Object data;
            if (dt.Rows.Count == 1)
            {
                data = new
                {
                    success = true,
                    backData = generateNews(dt.Rows[0])
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

        #region 更新或者新增新闻信息
        /// <summary>  
        /// 更新或者新增新闻信息 
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
                BLL.handleNews news = new BLL.handleNews();
                bool flag = false;
                flag = news.saveAP(d);

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

        #region 删除新闻
        /// <summary>  
        /// 删除新闻
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
                BLL.handleNews news = new BLL.handleNews();
                bool flag = false;

                flag = news.delete(id);

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

        #region 审核新闻
        /// <summary>  
        /// 审核新闻 
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
                BLL.handleNews news = new BLL.handleNews();
                int flag = news.review(d);

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

        #region 私有方法集
        //返回news对象
        private news generateNews(dynamic d)
        {
            news n = new news();
            n.id = d["id"].ToString();
            n.cityId = d["cityId"].ToString();
            n.cityName = d["cityName"].ToString();
            n.newsType = d["newsType"].ToString();
            n.newsTitle = d["newsTitle"].ToString();
            n.newsCover = util.generateImage(d["newsCover"].ToString());
            n.newsContent = d["newsContent"].ToString();
            n.newsAuthor = d["newsAuthor"].ToString();
            n.newsBrief = d["newsBrief"].ToString();
            n.state = Convert.ToInt32(d["state"].ToString());
            n.readNum = Convert.ToInt32(d["readNum"].ToString());
            n.updator = d["updator"].ToString();
            n.updatorName = d["updatorName"].ToString();
            n.update_time = d["update_time"].ToString();
            n.creator = d["creator"].ToString();
            n.creatorName = d["creatorName"].ToString();
            n.typeName = d["typeName"].ToString();
            n.create_time = d["create_time"].ToString();

            return n;
        }  
        #endregion
    }
}

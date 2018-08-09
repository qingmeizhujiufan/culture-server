using culture_server.Core;
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
    public class CityController : ApiController
    {
        #region 获取城市列表
        /// <summary>  
        /// 获取城市列表 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage queryList()
        {
            DataTable dt = new BLL.handleCity().queryList();
            Object data;
            if (dt.Rows.Count >= 0)
            {
                List<city> list = new List<city>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(generateCity(dt.Rows[i]));
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

        #region 更新或者新增城市信息
        /// <summary>  
        /// 更新或者新增城市信息 
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
                BLL.handleCity city = new BLL.handleCity();
                bool flag = false;
                flag = city.saveAP(d);

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

        #region 删除城市
        /// <summary>  
        /// 删除城市
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
                BLL.handleCity city = new BLL.handleCity();
                bool flag = false;

                flag = city.delete(id);

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
                        backMsg = "删除城市信息失败"

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
        private city generateCity(dynamic d)
        {
            city n = new city();
            n.id = d["id"].ToString();
            n.cityName = d["cityName"].ToString();
            n.cultureTotal = Convert.ToInt32(d["cultureTotal"].ToString());
            n.artTotal = Convert.ToInt32(d["artTotal"].ToString());
            n.newsTotal = Convert.ToInt32(d["newsTotal"].ToString());
            n.create_time = d["create_time"].ToString();

            return n;
        }
        #endregion
    }
}

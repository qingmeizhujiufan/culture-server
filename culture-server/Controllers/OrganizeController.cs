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
    public class OrganizeController : ApiController
    {
        #region 获取所有动态信息
        /// <summary>  
        /// 获取所有动态信息 
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>
        [AcceptVerbs("OPTIONS", "GET")]
        public HttpResponseMessage getAllOrganizeInfo()
        {
            DataTable dt = new BLL.handleOrganize().getAllOrganizeInfo();
            Object data;
            if (dt.Rows.Count >= 0)
            {
                List<organize> list = new List<organize>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    organize o = new organize();
                    o.id = dt.Rows[i]["id"].ToString();
                    o.pId = dt.Rows[i]["pId"].ToString();
                    o.userName = dt.Rows[i]["userName"].ToString();
                    o.type = Convert.ToInt32(dt.Rows[i]["type"].ToString());
                    o.typeName = dt.Rows[i]["typeName"].ToString();
                    o.update_time = dt.Rows[i]["update_time"].ToString();
                    o.create_time = dt.Rows[i]["create_time"].ToString();

                    list.Add(o);
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
    }
}

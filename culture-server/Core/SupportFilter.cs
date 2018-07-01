using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Security;

namespace culture_server.Core
{
    public class SupportFilter : AuthorizeAttribute
    {
        //重写基类的验证方式，加入我们自定义的Ticket验证
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Method == HttpMethod.Options)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Accepted);
                return;
            }
            //url获取token
            var content = actionContext.Request.Properties["MS_HttpContext"] as HttpContextBase;
            var token = content.Request.Headers["Token"];
            if (!string.IsNullOrEmpty(token))
            {
                //解密用户ticket,并校验用户名密码是否匹配
                if (ValidateTicket(token))
                {
                    base.IsAuthorized(actionContext);
                }
                else
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
            //如果取不到身份验证信息，并且不允许匿名访问，则返回未验证401
            else
            {
                //var attributes = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
                //bool isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
                //if (isAnonymous) base.OnAuthorization(actionContext);
                //else HandleUnauthorizedRequest(actionContext);
                HandleUnauthorizedRequest(actionContext);
            }
        }

        //校验用户名密码（对Session匹配，或数据库数据匹配）
        private bool ValidateTicket(string encryptToken)
        {
            bool flag = false;
            var strTicket = string.Empty;
            try
            {
                //解密Ticket
                strTicket = FormsAuthentication.Decrypt(encryptToken).UserData;
            }
            catch
            {
                return flag;
            }


            //从Ticket里面获取用户名和密码
            var index = strTicket.IndexOf("&");
            string userName = strTicket.Substring(0, index);
            string password = strTicket.Substring(index + 1);
            //取得token，不通过说明用户退出，或者session已经过期
            string str_checkToken = @"select * from dbo.c_token where token='{0}'";
            str_checkToken = string.Format(str_checkToken, encryptToken);
            try
            {
                DataTable dt = DBHelper.SqlHelper.GetDataTable(str_checkToken);
                //对比数据库中的令牌
                if (dt.Rows.Count == 1)
                {
                    //未超时  
                    flag = (DateTime.Now <= Convert.ToDateTime(dt.Rows[0]["expireDate"])) ? true : false;
                }
            }
            catch (Exception ex) { }
            return flag;
        }
    }
}
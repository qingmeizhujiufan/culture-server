using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI;
using External;
using Com.Alipay;

namespace BLL
{
    public class Common
    {
        /// <summary>
        /// 判断管理员是否登陆
        /// </summary>
        /// <returns></returns>
        public bool IsAdminLogin()
        {
            bool bRtn = true;

            if (HttpContext.Current.Session["admin_id"] == null)
            {
                bRtn = false;
            }

            return bRtn;
        }

        /// <summary>
        /// 判断用户是否登陆
        /// </summary>
        /// <returns></returns>
        public bool IsUserLogin()
        {
            bool bRtn = true;

            if (HttpContext.Current.Session["user_openid"] == null)
            {
                bRtn = false;
            }

            return bRtn;
        }

        /// <summary>
        /// 获取当前登陆用户id
        /// </summary>
        /// <returns></returns>
        public string GetCurrentUserID()
        {
            string strID = string.Empty;

            if (HttpContext.Current.Session["user_openid"] != null)
            {
                strID = HttpContext.Current.Session["user_openid"].ToString();
            }

            return strID;
        }

        public string CreateOrderNo()
        {
            string strRtn = string.Empty;
            strRtn = CommonTool.Common.CreateOrderNo("tree");
            return strRtn;
        }

    }
}

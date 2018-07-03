using culture_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace culture_server.Controllers
{
    public class AdminManageController : Controller
    {
        #region 接口

        //单附件上传
        public JsonResult UpLoadImage(HttpPostedFileBase file)
        {
            var res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            try
            {
                string id = Guid.NewGuid().ToString();
                image img = new image();
                img.fileName = file.FileName;
                img.fileType = file.ContentType;
                img.fileSize = file.ContentLength;
                String[] arr = file.FileName.Split(new char[1] { '.' });
                img.filePath = "/UpLoadFile/" + id + "." + arr[arr.Length - 1];

                string path = Server.MapPath(img.filePath);
                file.SaveAs(path);

                bool flag = new BLL.handleImage().save(img);
                if (flag)
                {
                    res.Data = new
                    {
                        success = true,
                        data = new
                        {
                            id = id,
                            link = img.filePath
                        }
                    };
                }
                else
                {
                    res.Data = new
                    {
                        success = false
                    };
                }

                
                return res;
            }
            catch (Exception ex)
            {
                res.Data = new
                {
                    success = false
                };
                return res;
            }
        }

        #endregion

    }
}

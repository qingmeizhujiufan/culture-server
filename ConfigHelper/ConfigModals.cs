using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Xml;

namespace ConfigHelper
{
    public class ConfigModals
    {
        /// <summary>
        /// 获取数据库连接配置模型
        /// </summary>
        /// <param name="strConfigFolderPath"></param>
        /// <returns></returns>
        public DBConfigModal GetDBConfigModal(string strConfigFolderPath)
        {
            DBConfigModal modal = new DBConfigModal();
            modal.DBConnectionNodes = new List<DBConnectionStringNode>();

            string strConfigFilePath = strConfigFolderPath + DBConfigModal.strFileName;
            DBConnectionStringNode nodeItem;
            if (!File.Exists(strConfigFilePath))
            {
                CommonTool.WriteLog.Write("数据库配置文件不存在，请检查跟目录下的ConfigFile文件夹下是否存在配置文件,出错路径为："+strConfigFilePath);
                return modal;
            }

            string strOldContent = CommonTool.FileOper.OpenFileRead(strConfigFilePath);
            //CommonTool.WriteLog.Write("数据连接信息strOldContent：" + strOldContent);
            //strOldContent = strOldContent.Substring(5, strOldContent.Length - 5);
            string strXmlContent = strOldContent;
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(strXmlContent);
            try
            {
                XmlNodeList nodes = dom.SelectNodes("/root/connectionStrings/add");
                foreach (XmlNode node in nodes)
                {
                    nodeItem = new DBConnectionStringNode();
                    nodeItem.IsCurrentUse = node.Attributes.GetNamedItem("isCurrentUse").Value == "true" ? true : false;
                    nodeItem.Name = node.Attributes.GetNamedItem("name").Value;
                    nodeItem.Value = node.Attributes.GetNamedItem("value").Value;
                    nodeItem.DbType = (DBType)Convert.ToInt16(node.Attributes.GetNamedItem("dbType").Value);
                    if (nodeItem.IsCurrentUse == true)
                    {
                        modal.CurrentConnectionString = nodeItem.Value;
                        modal.CurrentDbType = nodeItem.DbType;
                    }
                    modal.DBConnectionNodes.Add(nodeItem);
                }
            }
            catch (Exception ex)
            {
                CommonTool.WriteLog.Write(ex.Message);
                throw ex;
            }
            finally
            {
            }

            return modal;
        }
    }
}

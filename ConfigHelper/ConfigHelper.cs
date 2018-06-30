using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ConfigHelper
{
    public class ConfigHelper
    {
        #region 数据部分

        public static string ConfigFilePath
        {
            get;
            set;
        }
        private static ConfigModals configModals;

        #endregion 

        /// <summary>
        /// 默认情况下配置文件为启动项目跟目录下的ConfigFile文件夹
        /// </summary>
        static ConfigHelper()
        {
            ConfigFilePath = CommonTool.PathHelper.GetCurrentBasePath() + @"ConfigFile\";
            configModals = new ConfigModals();
        }
         
        public static DBConfigModal DBConfigModal
        {
            get
            {
                return configModals.GetDBConfigModal(ConfigFilePath);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using Newtonsoft.Json.Linq;
using CommonTool;

namespace DBHelper
{
    /// <summary>
    /// 数据模型，可容易实现数据插入、更新、删除
    /// </summary>
    public class DataModal
    {
        private List<FieldItem> listFieldItem;

        public DataModal()
        {
            this.listFieldItem = new List<FieldItem>();
        }

        public string TableName { get; set; }

        public string PrimaryKey { get; set; }

        /// <summary>
        /// insert, update,delete
        /// </summary>
        public ExecuteType Type  { get; set; }

        public string OnlyFlag { get; set; }

        public List<FieldItem> ListFieldItem 
        { 
            get 
            {
                return this.listFieldItem;
            }
            set
            {
                this.listFieldItem = value;
            }
        }

        public string GetSql()
        {
            string strSql = string.Empty;

            string strTempFields = string.Empty;
            string strTempValues = string.Empty;
            bool bTag = false;

            

            switch(this.Type)
            {
                case ExecuteType.Insert:
                    {
                        if (this.listFieldItem.Count <= 0)
                        {
                            strSql = "";
                            return strSql;
                        }

                        foreach (FieldItem item in this.listFieldItem)
                        {
                            if (bTag == true)
                            {
                                strTempFields += ",";
                                strTempValues += ",";
                            }
                            strTempFields += item.FieldName;
                            switch (item.FieldType)
                            {
                                case JsonValueType.Boolean:
                                case JsonValueType.Number:
                                    strTempValues += item.FieldValue;
                                    break;
                                default:
                                    strTempValues += "'" + item.FieldValue + "'";
                                    break;
                            }
                            bTag = true;
                        }
                        strSql += string.Format("insert into {0}({1}) values({2})", TableName, strTempFields, strTempValues);
                    }
                    break;
                case ExecuteType.Update:
                    {
                        if (this.listFieldItem.Count <= 0)
                        {
                            strSql = "";
                            return strSql;
                        }

                        foreach (FieldItem item in this.listFieldItem)
                        {
                            if (bTag == true)
                            {
                                strTempFields += ",";
                            }
                            switch (item.FieldType)
                            {
                                case JsonValueType.Boolean:
                                case JsonValueType.Number:
                                    strTempFields += item.FieldName + "="  + item.FieldValue;
                                    break;
                                default:
                                    strTempFields += item.FieldName + "=" + string.Format("'{0}'", item.FieldValue);
                                    break;
                            }
                            
                            bTag = true;
                        }
                        strSql += string.Format("update {0} set {1} where {2} = '{3}'", TableName, strTempFields, this.PrimaryKey, this.OnlyFlag);
                    }
                    
                    break;
                case ExecuteType.Delete:
                    strSql += string.Format("delete from {0} where {1} = '{2}'", this.TableName, this.PrimaryKey, this.OnlyFlag);
                    break;
            }

            return strSql;
        }

        public int Execute()
        {
            string strSql = this.GetSql();
            return SqlHelper.ExecuteSql(strSql);
        }

        public void Clear()
        {
            this.listFieldItem.Clear();
            this.TableName = string.Empty;
            this.PrimaryKey = string.Empty;
            this.OnlyFlag = string.Empty;
        }
    }

    /// <summary>
    /// 字段单元
    /// </summary>
    public class FieldItem
    {
        public FieldItem()
        {
        }
        public FieldItem(string strFieldName, string strFieldValue, JsonValueType valueType)
        {
            this.FieldName = strFieldName;
            this.FieldValue = strFieldValue;
            this.FieldType = valueType;
        }
        public FieldItem(string strFieldName, string strFieldValue)
        {
            this.FieldName = strFieldName;
            this.FieldValue = strFieldValue;
            this.FieldType = JsonValueType.String;
        }
        public string FieldName = string.Empty;
        public string FieldValue = string.Empty;
        public JsonValueType FieldType = JsonValueType.String;
    }

    public enum ExecuteType
    {
        Insert =1,
        Update,
        Delete
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.Script.Serialization;

using Newtonsoft.Json.Linq;
using System.Runtime.Serialization.Json;

namespace CommonTool
{
    /// <summary>
    /// 出来json字符串相关的操作：
    /// 1. DataTable转为json字符串
    /// 2. json字符串转为对象
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// 最简易模式， 取值方式： json.table.rows[i].cells[j].value
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJsonString(DataTable dt)
        {
            JsonDealBll bll = new JsonDealBll();
            return bll.DataTableToJsonString(dt);
        }

        /// <summary>
        /// 适应EasyUI格式的json  格式：{"total":50,"rows":[{fieldName1:value1,field2:value2...},{},{}...]}
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJsonString_EasyUI(DataTable dt)
        {
            JsonDealBll bll = new JsonDealBll();
            return bll.DataTableToJsonString_EasyUI(dt);
        }

        public static string ConvertToJson(Dictionary<string, string> dic)
        {
            JsonDealBll bll = new JsonDealBll();
            return bll.ConvertToJson(dic);
        }

        public static string ObjectToJSON(object obj)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                return jss.Serialize(obj);
            }
            catch (Exception ex)
            {

                throw new Exception("JSONHelper.ObjectToJSON(): " + ex.Message);
            }
        }

        public static string DataTableToJSON(DataTable dt)
        {
            return ObjectToJSON(DataTableToList(dt));
        }
        public static List<Dictionary<string, object>> DataTableToList(DataTable dt)
        {
            List<Dictionary<string, object>> list
                 = new List<Dictionary<string, object>>();

            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    dic.Add(dc.ColumnName, dr[dc.ColumnName]);
                }
                list.Add(dic);
            }
            return list;
        }

        /// <summary>
        /// 将字符串转化字典
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static Dictionary<string, JToken> GetParms(string strJson)
        {
            Dictionary<string, JToken> dictionary = new Dictionary<string, JToken>();
            if (!StringIsJsonObject(strJson))
            {
                CommonTool.WriteLog.Write("传入的字符串格式不正确");
                return dictionary;
            }

            JObject jObj = JObject.Parse(strJson);
            IEnumerator<KeyValuePair<string, JToken>> enumRator = jObj.GetEnumerator();
            while (enumRator.MoveNext())
            {
                dictionary.Add(enumRator.Current.Key, enumRator.Current.Value);
            }

            return dictionary;
        }

        /// <summary>
        /// 将字符串转化为字典
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetParms2(string strJson)
        {
            Dictionary<string, JToken> dictionary = GetParms(strJson);
            Dictionary<string, string> dicRtn = new Dictionary<string, string>();
            foreach (string key in dictionary.Keys)
            {
                dicRtn.Add(key, dictionary[key].ToString());
            }

            return dicRtn;
        }

        public static List<T> JsonToList<T>(string jsonText)
        {
            List<T> list = new List<T>();

            DataContractJsonSerializer _Json = new DataContractJsonSerializer(list.GetType());
            byte[] _Using = System.Text.Encoding.UTF8.GetBytes(jsonText);
            System.IO.MemoryStream _MemoryStream = new System.IO.MemoryStream(_Using);
            _MemoryStream.Position = 0;

            return (List<T>)_Json.ReadObject(_MemoryStream);
        }

        /// <summary>
        /// 判断字符串是否能转换为json对象
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static bool StringIsJsonObject(string strJson)
        {
            bool bRtn = true;
            try
            {
                JObject.Parse(strJson);
            }
            catch (Exception ex)
            {
                bRtn = false;
            }

            return bRtn;
        }
    }

    public class JsonDealBll
    {
        /// <summary>
        /// 最简易模式， 取值方式： json.table.rows[i].cells[j].value
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string DataTableToJsonString(DataTable dt)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            string strRowTemp = string.Empty;
            string strCellTemp = string.Empty;
            string strCellJsonTemp = string.Empty;
            sb.Append("{table:{rows:[");

            int intColumnCount = dt.Columns.Count;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < intColumnCount; j++)
                {
                    strCellTemp = "{value:\"{0}\"}";
                    strCellTemp = strCellTemp.Replace("{0}", dt.Rows[i][j].ToString().Replace("\"", "'"));
                    if (j != 0)
                    {
                        strCellJsonTemp += ",";
                    }
                    strCellJsonTemp += strCellTemp;
                }
                strRowTemp = "{cells:[{0}]}";
                strRowTemp = strRowTemp.Replace("{0}", strCellJsonTemp);
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(strRowTemp);
                strCellJsonTemp = "";
            }
            sb.Append("]}}");
            return sb.ToString();
        }

        /// <summary>
        /// 适应EasyUI格式的json  格式：{"total":50,"rows":[{fieldName1:value1,field2:value2...},{},{}...]}
        /// 前端取值方式 ： json.rows[i].fieldName
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string DataTableToJsonString_EasyUI(DataTable dt)
        {
            string strRtn = string.Empty;

            System.Text.StringBuilder sbRows = new StringBuilder();
            string strCells = string.Empty;

            int intColumnCount = dt.Columns.Count;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    sbRows.Append(",");
                }
                sbRows.Append("{");
                for (int j = 0; j < intColumnCount; j++)
                {
                    if (j != 0)
                    {
                        sbRows.Append(",");
                    }
                    sbRows.Append("\"" + dt.Columns[j].ColumnName.ToUpper() + "\":\"" + dt.Rows[i][j].ToString().Replace("\"", "'") + "\"");
                }
                sbRows.Append("}");
            }
            strRtn = "{\"total\":" + dt.Rows.Count + ",\"rows\":[" + sbRows.ToString() + "]}";
            return strRtn;
        }



        public string ConvertToJson(Dictionary<string, string> dic)
        {
            string strRtn = string.Empty;
            

            if (dic.Count <= 0)
            {
                strRtn = "{}";
            }

            strRtn = this.GetJsonString(dic);

            return strRtn;
        }

        public string GetSingleJsonProperty2Value(string key, string value, JsonValueType jType)
        {
            string strRtn = string.Empty;
            string strValue = string.Empty;

            strRtn += "\"" + key + "\"";
            strRtn += ":";
            if (jType == JsonValueType.String)
            {
                strValue = "\"" + value + "\"";
            }
            else if (jType == JsonValueType.Boolean || jType == JsonValueType.Number || jType == JsonValueType.Array || jType == JsonValueType.Json)
            {
                strValue = value;
            }
            strRtn += strValue;

            return strRtn;
        }
        public string GetSingleJsonProperty2Value(JsonSingleProtery2Value value)
        {
            string strRtn = string.Empty;
            string strValue = string.Empty;

            strRtn += "\"" + value.Key + "\"";
            strRtn += ":";
            if (value.JType == JsonValueType.String)
            {
                strValue = "\"" + value.Value + "\"";
            }
            else if (value.JType == JsonValueType.Boolean || value.JType == JsonValueType.Number || value.JType == JsonValueType.Array || value.JType == JsonValueType.Json)
            {
                strValue = value.Value;
            }
            strRtn += strValue;

            return strRtn;
        }

        public string GetJsonString(Dictionary<string, JsonSingleProtery2Value> dic)
        {
            string strRtn = string.Empty;
            StringBuilder sb = new StringBuilder();
            bool bTag = false;
            sb.Append("{");
            foreach (string key in dic.Keys)
            {
                if (bTag == true)
                {
                    sb.Append(",");
                }
                sb.Append(this.GetSingleJsonProperty2Value(dic[key]));
                bTag = true;
            }
            sb.Append("}");
            strRtn = sb.ToString();
            return strRtn;
        }

        public string GetJsonString(Dictionary<string, string> dic)
        {
            string strRtn = string.Empty;
            StringBuilder sb = new StringBuilder();
            bool bTag = false;

            Dictionary<string, JsonSingleProtery2Value> dicDeal = this.OrdiaryDicToJsonDic(dic);

            sb.Append("{");
            foreach (string key in dicDeal.Keys)
            {
                if (bTag == true)
                {
                    sb.Append(",");
                }
                sb.Append(this.GetSingleJsonProperty2Value(dicDeal[key]));
                bTag = true;
            }
            sb.Append("}");
            strRtn = sb.ToString();
            return strRtn;
        }

        public Dictionary<string, JsonSingleProtery2Value> OrdiaryDicToJsonDic(Dictionary<string, string> dic)
        {
            Dictionary<string, JsonSingleProtery2Value> dicRtn = new Dictionary<string, JsonSingleProtery2Value>();
            JsonSingleProtery2Value value = new JsonSingleProtery2Value();
            foreach (string key in dic.Keys)
            {
                value = new JsonSingleProtery2Value();
                value.Key = key;
                value.Value = dic[key];
                value.JType = JsonValueType.String;
                dicRtn.Add(key, value);
            }

            return dicRtn;
        }
        


    }

    public enum JsonValueType
    {
        String = 1,
        Number = 2,
        Boolean = 3,
        Array = 4,
        Json = 5
    }

    public struct JsonSingleProtery2Value
    {
        public string Key;
        public string Value;
        public JsonValueType JType;
    }
}

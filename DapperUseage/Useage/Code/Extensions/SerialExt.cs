using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Useage.Code.Utils
{
    public static class SerialExt
    {
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T t)
        {
            //正则替换，不输出IEntity里面的内容
            System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(",\"DBTableName\":\"([\\s\\S]*)\",\"PKName\":\"([\\s\\S]*)\",\"Fields\":\"([\\s\\S]*)\",\"FieldList\":\\[(\"([a-zA-Z0-9\u4e00-\u9fa5-_])+\"(,)?)+\\]},", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regEx2 = new System.Text.RegularExpressions.Regex(",\"DBTableName\":\"([\\s\\S]*)\",\"PKName\":\"([\\s\\S]*)\",\"Fields\":\"([\\s\\S]*)\",\"FieldList\":\\[(\"([a-zA-Z0-9\u4e00-\u9fa5-_])+\"(,)?)+\\]}]", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var iso = new IsoDateTimeConverter();
            iso.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            string result= regEx.Replace(JsonConvert.SerializeObject(t, iso), "},");
            return regEx2.Replace(result, "}]");
        }

        /// <summary>
        /// DataTable转化为json
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToJson(this DataTable dt)
        {
            return JsonConvert.SerializeObject(dt, new DataTableConverter());
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string strJson)
        {
            return JsonConvert.DeserializeObject<T>(strJson);
        }
    }
}

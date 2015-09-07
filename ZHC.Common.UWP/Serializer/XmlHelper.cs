using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZHC.Common.UWP.Serializer
{
    public class XmlHelper
    {
        public static string XmlSerializer<T>(T t)
        {
            var ser = new DataContractSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                ser.WriteObject(ms, t);
                //内存流相当指针，设置为0，从头开始读
                ms.Position = 0;
                using (StreamReader sr = new StreamReader(ms))
                {
                    string jsonString = sr.ReadToEnd();
                    return jsonString;
                }
            }
        }

        #region Json→T
        public static T XmlToObj<T>(string jsonString)
        {
            //将"yyyy-MM-dd HH:mm:ss"格式的字符串转为"\/Date(1294499956278+0800)\/"格式
            string p = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);
            Regex reg = new Regex(p);
            jsonString = reg.Replace(jsonString, matchEvaluator);
            DataContractSerializer ser = new DataContractSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                T obj = (T)ser.ReadObject(ms);
                return obj;
            }
        }
        public static string ConvertDateStringToJsonDate(Match m)
        {
            string result = string.Empty;
            DateTime dt = DateTime.Parse(m.Groups[0].Value);
            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");
            result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
            return result;
        }
        #endregion
    }
}


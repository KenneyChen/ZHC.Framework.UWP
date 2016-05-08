using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZHC.Common.UWP
{
    public static class Extensions
    {
        public static Predicate<T> ToPredicate<T>(this Func<T, bool> source)
        {
            Predicate<T> result = new Predicate<T>(source);
            return result;
        }

        public static string ToCacheKey(this Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            string hashedResult = uri.GetHashCode().ToString();

            //http://stackoverflow.com/questions/3009284/using-regex-to-replace-invalid-characters
            string pattern = "[\\~#%&*{}/:<>?|\"-]";
            string replacement = " ";

            Regex regEx = new Regex(pattern);
            string sanitized = Regex.Replace(regEx.Replace(hashedResult, replacement), @"\s+", "_");

            return sanitized;
        }
        public static string J_Format(this string s, params object[] p)
        {
            return string.Format(s, p);
        }

        /// <summary>
        /// 往前面字符串补0
        /// </summary>
        /// <param name="s"></param>
        /// <param name="len"></param>
        public static string J_AddZero(this object s, int len = 2)
        {
            if (s.ToString().Length < len)
            {
                var zero = "";
                for (int i = 0; i < len - s.ToString().Length; i++)
                {
                    zero += "0";
                }
                s = zero + s;
            }

            return s.ToString();
        }

        /// <summary>
        ///     把对象类型转化为指定类型，转化失败时返回该类型默认值
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 要转化的源对象 </param>
        /// <returns> 转化后的指定类型的对象，转化失败返回类型的默认值 </returns>
        public static T CastTo<T>(this object value)
        {
            object result;
            Type type = typeof(T);
            try
            {
                if (type == typeof(Guid))
                {
                    result = Guid.Parse(value.ToString());
                }
                else
                {
                    result = Convert.ChangeType(value, type);
                }
            }
            catch
            {
                result = default(T);
            }

            return (T)result;
        }

        /// <summary>
        ///     把对象类型转化为指定类型，转化失败时返回指定的默认值
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 要转化的源对象 </param>
        /// <param name="defaultValue"> 转化失败返回的指定默认值 </param>
        /// <returns> 转化后的指定类型对象，转化失败时返回指定的默认值 </returns>
        public static T CastTo<T>(this object value, T defaultValue)
        {
            object result;
            Type type = typeof(T);
            try
            {
                result = Convert.ChangeType(value, type);
            }
            catch
            {
                result = defaultValue;
            }
            return (T)result;
        }


    }
}

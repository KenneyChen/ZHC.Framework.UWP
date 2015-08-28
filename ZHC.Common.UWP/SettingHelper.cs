using LYL.Common.RT.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using ZHC.Common.UWP.Model;

namespace ZHC.Common.UWP
{
    /// <summary>
    /// SettingHelper
    /// </summary>
    public class SettingHelper
    {
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="func"></param>
        /// <param name="seconds">等于0永远不过期</param>
        /// <returns></returns>
        public static T GetCache<T>(string cacheKey, Func<T> func, int seconds = 0) where T : BaseCache
        {
            T data = GetSettings<T>(cacheKey);
            if (data == null || (seconds != 0 && data != null && (DateTime.Now - data.ExpirationDate).TotalSeconds > seconds))
            {
                data = func();
                if (seconds != 0)
                {
                    data.ExpirationDate = DateTime.Now;
                }

                SaveSettings<T>(cacheKey, data);
            }
            return data;
        }

        /// <summary>
        /// 获取缓存，如果没有，存入方法返回中的内容到缓存再返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey">键</param>
        /// <param name="seconds">缓存秒数</param>
        /// <param name="func">方法，调用方式：
        /// ()=>{return (T)data;}
        /// </param>
        /// <returns></returns>
        public static T GetFunc<T>(string cacheKey, int seconds, Func<T> func)
        {
            return DataCache.GetCache<T>(() =>
            {
                return GetSettings<T>(cacheKey);
            },
            () =>
            {
                var data = func();
                SaveSettings<T>(cacheKey, data);
                return data;
            });
        }

        public static T Get<T>(string key, T obj)
        {
            var result = GetSettings<T>(key);
            if (result != null)
            {
                return result;
            }
            else
            {
                SaveSettings<T>(key, obj);
                return obj;
            }

        }

        #region App Setting
        /// <summary>
        /// 保存string
        /// </summary>
        public static void SaveStringSettings(string key, string contents)
        {
            ApplicationData.Current.LocalSettings.Values[key] = contents;
        }

        public static void SaveSettings<T>(string key, T t)
        {
            var json = JsonHelper.JsonSerializer<T>(t);
            ApplicationData.Current.LocalSettings.Values[key] = json;
        }

        /// <summary>
        /// 保存obj
        /// </summary>
        public static void SaveSettings(string key, object obj)
        {
            ApplicationData.Current.LocalSettings.Values[key] = obj;
        }

        /// <summary>
        /// 获取string设置
        /// </summary>
        public static string GetSettings(string key)
        {
            var settings = ApplicationData.Current.LocalSettings;
            if (settings.Values.ContainsKey(key))
            {
                return (string)settings.Values[key];
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取T设置
        /// </summary>
        public static T GetSettings<T>(string key)
        {
            var settings = ApplicationData.Current.LocalSettings;
            if (settings.Values.ContainsKey(key))
            {
                var json = settings.Values[key].ToString();

                return JsonHelper.JsonToObj<T>(json);
            }
            else
            {
                return default(T);
            }
        }

        #endregion
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using ZHC.Common.UWP.Model;
using ZHC.Common.UWP.Serializer;

namespace ZHC.Common.UWP.Storage
{
    /// <summary>
    /// SettingHelper
    /// </summary>
    public class SettingHelper
    {
        public static ApplicationDataContainer LocalSettings
        {
            get { return ApplicationData.Current.LocalSettings; }
        }

        public static ApplicationDataContainer RoamingSettings
        {
            get { return ApplicationData.Current.RoamingSettings; }
        }


        public static ApplicationDataContainer GetCurrentSettings(SettingType type)
        {
            switch (type)
            {
                case SettingType.Local:
                    return LocalSettings;
                case SettingType.Remote:
                    return RoamingSettings;
                case SettingType.Temp:
                    throw new Exception("未实现");
                default:
                    return LocalSettings;
            }
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="func"></param>
        /// <param name="seconds">等于0永远不过期</param>
        /// <returns></returns>
        public static T GetCache<T>(string cacheKey, Func<T> func, SettingType type = SettingType.Local, int seconds = 0)
        {
            CacheModel<T> data = null;
            try
            {
                data = GetSettings<CacheModel<T>>(cacheKey);
            }
            catch (Exception ex)
            {

            }
            if (data == null || (seconds != 0 && data != null && (DateTime.Now - data.ExpirationDate).TotalSeconds > seconds))
            {
                data = new CacheModel<T> { Model = func() };
                data.ExpirationDate = DateTime.Now;
                SaveSettings<CacheModel<T>>(cacheKey, data,type);
            }

            return (data ?? new CacheModel<T>()).Model;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="func"></param>
        /// <param name="seconds">等于0永远不过期</param>
        /// <returns></returns>
        public static async Task<T> GetCacheAsync<T>(string cacheKey, Func<Task<T>> func, int seconds = 0, SettingType type = SettingType.Local)
        {
            CacheModel<T> data = null;
            try
            {
                data = GetSettings<CacheModel<T>>(cacheKey, type);
            }
            catch (Exception ex)
            {

            }

            if (data == null || (seconds != 0 && data != null && (DateTime.Now - data.ExpirationDate).TotalSeconds > seconds))
            {
                data = new CacheModel<T> { Model = await func() };
                data.ExpirationDate = DateTime.Now;
                SaveSettings<CacheModel<T>>(cacheKey, data, type);
            }

            return (data ?? new CacheModel<T>()).Model;
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
        public static T GetFunc<T>(string cacheKey, int seconds, Func<T> func, SettingType type = SettingType.Local)
        {
            return DataCache.GetCache<T>(() =>
            {
                return GetSettings<T>(cacheKey,type);
            },
            () =>
            {
                var data = func();
                SaveSettings<T>(cacheKey, data,type);
                return data;
            });
        }

        //public static T Get<T>(string key, T obj)
        //{
        //    var result = GetSettings<T>(key);
        //    if (result != null)
        //    {
        //        return result;
        //    }
        //    else
        //    {
        //        SaveSettings<T>(key, obj);
        //        return obj;
        //    }
        //}

        #region App Setting
        /// <summary>
        /// 保存string
        /// </summary>
        public static void SaveStringSettings(string key, string contents, SettingType type = SettingType.Local)
        {
            var local = GetCurrentSettings(type);
            local.Values[key] = contents;
        }

        public static void SaveSettings<T>(string key, T t, SettingType type = SettingType.Local)
        {
            var local = GetCurrentSettings(type);
            var json = JsonHelper.JsonSerializer<T>(t);
            if (local.Values.ContainsKey(key))
            {
                local.Values.Remove(key);
            }
            local.Values[key] = json;

        }

        /// <summary>
        /// 保存obj
        /// </summary>
        public static void SaveSettings(string key, object obj, SettingType type = SettingType.Local)
        {
            var local = GetCurrentSettings(type);
            local.Values[key] = obj;
        }

        /// <summary>
        /// 获取string设置
        /// </summary>
        public static string GetSettings(string key, SettingType type = SettingType.Local)
        {
            var settings = GetCurrentSettings(type);
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
        public static T GetSettings<T>(string key, SettingType type = SettingType.Local)
        {
            var settings = GetCurrentSettings(type);
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

    public enum SettingType
    {
        /// <summary>
        /// 本地
        /// </summary>
        Local = 1,

        /// <summary>
        /// 远程
        /// </summary>
        Remote = 2,

        /// <summary>
        /// 临时
        /// </summary>
        Temp = 3,
    }
}

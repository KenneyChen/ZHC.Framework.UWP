using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHC.Common.UWP
{
    public class DataCache
    {

        public static T GetCache<T>(Func<T> getCache, Func<T> setCache)
        {
            T data = getCache();
            if (data == null)
            {
                data = setCache();
            }
            return data;
        }
    }
}

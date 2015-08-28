using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHC.Common.UWP
{
    public class DataCache
    {
        public static T GetCache<T>(Func<T> getFunc, Func<T> setFunc)
        {
            T data = getFunc();
            if (data == null)
            {
                data = setFunc();
            }
            return data;
        }
    }
}

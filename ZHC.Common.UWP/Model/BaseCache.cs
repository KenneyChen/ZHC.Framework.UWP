using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHC.Common.UWP.Model
{
    public abstract class BaseCache
    {
        public DateTime ExpirationDate { get; set; }

    }

   
    public class CacheModel<T> : BaseCache
    {
        public T Model { get; set; }
    }
}

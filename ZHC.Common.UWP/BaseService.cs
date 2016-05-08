using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;
using ZHC.Common.UWP.Serializer;
using ZHC.Common.UWP.Storage;

namespace ZHC.Common.UWP
{
    public abstract class BaseService
    {
        public virtual string FileKey { get { return "FileKey"; } }

        protected async Task<T> GetAsync<T>(string uri) where T : class
        {
            // set a timeout of 20s
            //var source = new CancellationTokenSource(20000);

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync(new Uri(uri, UriKind.Absolute));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonHelper.JsonToObj<T>(content);

                        return result;
                    }
                    else
                    {
                        throw new Exception(string.Format("Oops! Something bad just happend (Error code: {0}). :(" + response.StatusCode.ToString()));
                    }
                }
                catch (TaskCanceledException)
                {
                    throw new TaskCanceledException("Your Internet is slow as...");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }



        public async Task<T> GetAll<T>()
        {
            return await StorageHelper.ReadTextFileAsync<T>(FileKey);
        }

        public async Task<bool> Add<TResult>(TResult model, Func<TResult, bool> predicate)
        {
            var isAdd = false;
            List<TResult> list = await GetAll<List<TResult>>();
            var ret = new List<TResult>() { };
            if (list != null && list.Count > 0)
            {
                list.ForEach(x => ret.Add(x));
                if (!list.Any(predicate))
                {
                    ret.Add(model);
                    isAdd = true;
                }
            }
            else
            {
                ret.Add(model);
                isAdd = true;
            }

            if (isAdd)
            {
                await StorageHelper.WriteToTextFileAsync(FileKey, ret);
            }

            return true;
        }

        public async Task<bool> Delete<TResult>(Func<TResult, bool> predicate)
        {
            List<TResult> list = await GetAll<List<TResult>>();
            if (list != null && list.Count > 0)
            {
                if (list.Any(predicate))
                {
                    list.RemoveAll(new Predicate<TResult>(predicate));
                    await StorageHelper.WriteToTextFileAsync(FileKey, list);
                    return true;
                }
            }

            return false;
        }

    }

}

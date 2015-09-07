using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;
using ZHC.Common.UWP.Serializer;

namespace ZHC.Common.UWP
{
    public abstract class BaseService
    {
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

        //protected async Task<ResponseResult<T>> Post<T>(string path, List<PostData> data) where T : class, new()
        //{
        //    T t = new T();
        //    ResponseStatus status;

        //    var http = new HttpClient();

        //    var values = new List<KeyValuePair<string, string>>();

        //    foreach (var item in data)
        //    {
        //        values.Add(new KeyValuePair<string, string>(item.Key, item.Value));
        //    }
        //    var res = await http.PostAsync(new Uri(Constants.BASE_URL + path, UriKind.Absolute), new HttpFormUrlEncodedContent(values));
        //    if (res.StatusCode == HttpStatusCode.Ok)
        //    {
        //        try
        //        {
        //            var json = await res.Content.ReadAsStringAsync();
        //            t = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        //            status = ResponseStatus.SUCCESS;
        //        }
        //        catch (Exception)
        //        {
        //            status = ResponseStatus.EXCEPTION;
        //        }
        //    }
        //    else
        //    {
        //        status = ResponseStatus.NOT_NETWORK;
        //    }

        //    return new ResponseResult<T> { Data = t, Code = status };
        //}
    }

    //public class ResponseResult<T>
    //{
    //    public ResponseStatus Code { get; set; }
    //    public T Data { get; set; }
    //}

    //public class PostData
    //{
    //    public string Key { get; set; }
    //    public string Value { get; set; }

    //}

    //public enum ResponseStatus
    //{
    //    SUCCESS,
    //    NOT_NETWORK,
    //    SERVER_ERROR,
    //    EXCEPTION,
    //    PARAMETER_MISSING,
    //}
}

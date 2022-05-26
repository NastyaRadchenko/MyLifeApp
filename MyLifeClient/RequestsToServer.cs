using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyLifeClient
{
    public class RequestsToServer<T>
    {
        public static async Task<HttpResponseMessage> SendPost(T input, string uri)
        {
            var httpClient = new HttpClient();
            var jsonObject = JsonConvert.SerializeObject(input);
            HttpContent httpContent = new StringContent(jsonObject);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var responce = await httpClient.PostAsync(new Uri(uri), httpContent);
            return responce;
        }

        public static async Task<HttpResponseMessage> SendPut(T input, string uri)
        {
            var httpClient = new HttpClient();
            var jsonObject = JsonConvert.SerializeObject(input);
            HttpContent httpContent = new StringContent(jsonObject);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var responce = await httpClient.PutAsync(new Uri(uri), httpContent);
            return responce;
        }

        public static async Task<T> SendGet(string uri)
        {
            var httpClient = new HttpClient();
            var responce = await httpClient.GetAsync(uri);
            if (!responce.IsSuccessStatusCode) 
                return default(T);
            var result = JsonConvert.DeserializeObject<T>(responce.Content.ReadAsStringAsync().Result);
            return result;
        }

        public static async Task<HttpResponseMessage> SendDelete(string uri)
        {
            var httpClient = new HttpClient();
            var responce = await httpClient.DeleteAsync(uri);
            return responce;
        }
    }
}

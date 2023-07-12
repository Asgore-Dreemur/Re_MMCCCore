using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace MMCCCore.Tools
{
    public static class HttpTools
    {
        private static HttpClient Client = new HttpClient();

        public static async Task<HttpResponseMessage> HttpGetTaskAsync(string url, Tuple<string, string> AuthTuple = null)
        {
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, url);
            if(AuthTuple != null)
            {
                message.Headers.Authorization = new AuthenticationHeaderValue(AuthTuple.Item1, AuthTuple.Item2);
            }
            return await Client.SendAsync(message);
        }

        public static async Task<HttpResponseMessage> HttpPostTaskAsync(string url, string content_type, string body)
        {
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, url);
            message.Content = new StringContent(body);
            message.Content.Headers.ContentType = new MediaTypeHeaderValue(content_type);
            return await Client.SendAsync(message);
        }

        public static async Task<HttpResponseMessage> HttpPostTaskAsync(string url,
            Dictionary<string, string> UrlEncodedParam,
            string content_type = "application/x-www-form-urlencoded")
        {
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, url);
            message.Content = new FormUrlEncodedContent(UrlEncodedParam);
            message.Content.Headers.ContentType = new MediaTypeHeaderValue(content_type);
            return await Client.SendAsync(message);
        }
    }
}

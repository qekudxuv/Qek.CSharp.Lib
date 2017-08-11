using System;
using System.IO;
using System.Net;
using System.Text;

namespace Qek.IO
{
    public class HttpHandler
    {

        protected CookieContainer _cookieContainer = new CookieContainer();

        public HttpHandler()
        {
        }

        public byte[] FetchResourceByHttpGet(string resourceUrl)
        {
            byte[] remoteResource = null;
            Uri uri = new Uri(resourceUrl);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Get;
            request.CookieContainer = _cookieContainer;
            request.KeepAlive = true;
            request.ContentType = "application/x-www-form-urlencoded";
            //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.116 Safari/537.36";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                //在這裡對接收到的頁面內容進行處理
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        remoteResource = FileConverter.StreamToByteArray(responseStream);
                    }
                }
            }

            return remoteResource;
        }

        public byte[] FetchResourceByHttpPost(string resourceUrl, string param)
        {
            byte[] remoteResource = null;
            byte[] bs = Encoding.ASCII.GetBytes(param);
            Uri uri = new Uri(resourceUrl);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Post;
            request.ContentLength = bs.Length;
            request.CookieContainer = _cookieContainer;
            request.KeepAlive = true;
            request.ContentType = "application/x-www-form-urlencoded";
            //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.116 Safari/537.36";

            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                //在這裡對接收到的頁面內容進行處理
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        remoteResource = FileConverter.StreamToByteArray(responseStream);
                    }
                }
            }

            return remoteResource;
        }
    }
}

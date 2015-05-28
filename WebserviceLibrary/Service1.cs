using System;
using System.ServiceModel.Web;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace WebserviceLibrary
{
    public class Service1 : IService1
    {
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "data/")]
        public Stream GetData()
        {
            var json = new WebClient().DownloadString("http://olympos.intellifi.nl/api/events");
            JObject o = JObject.Parse(json);
            JArray results = (JArray)o["results"];
            string nextUrl = o["next_url"].ToString();
            while (nextUrl != "")
            {
                var tempJSON = new WebClient().DownloadString(nextUrl);
                JObject tempObject = JObject.Parse(tempJSON);
                nextUrl = tempObject["next_url"].ToString();

            }
            
            
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(results.ToString()));
            return ms;
        }
    }

    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
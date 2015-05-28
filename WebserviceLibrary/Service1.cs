using System;
using System.ServiceModel.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace WebserviceLibrary
{
    public class Service1 : IService1
    {
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "json/")]
        public string GetJson()
        {
            return "kaas";
        }
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "data/")]
        public Stream GetData()
        {
            // lijst met events ophalen
            var eventjson = new WebClient().DownloadString("http://olympos.intellifi.nl/api/events");
            JObject events = JObject.Parse(eventjson);
            JArray results = (JArray)events["results"];
            string nextUrl = events["next_url"].ToString();

            int i = 0;
            while (nextUrl != "" && i < 5)
            {
                var tempJSON = new WebClient().DownloadString(nextUrl);
                JObject tempObject = JObject.Parse(tempJSON);
                nextUrl = tempObject["next_url"].ToString();
                foreach(JToken j in (JArray)tempObject["results"])
                {
                    results.Add(j);
                }
                i++;

            }
            
            //todo: Loop maken die event objecten aanmaakt en variabelen set


            var locationjson = new WebClient().DownloadString("http://olympos.intellifi.nl/api/locations");
            JObject locations = JObject.Parse(locationjson);
            foreach (JToken j in results)
            {

                var labels = from l in locations["results"]
                             where j["topic"]["arguments"]["1"].ToString() == l["id"].ToString()
                             select l["label"].ToString();


                foreach (string l in labels)
                {
                    
                }
            }


            
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(results.ToString()));
            return ms;
        }
    }

    public class Event
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTime Time { get; set; }
        public string location { get; set; }
        public int sporterID { get; set; }
    }
}
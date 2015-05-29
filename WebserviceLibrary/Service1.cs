using System;
using System.ServiceModel.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;


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
        public List<Event> GetData()
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

            List<Event> eventList = new List<Event>();




            //todo: Loop maken die event objecten aanmaakt en variabelen set


            var locationJson = new WebClient().DownloadString("http://olympos.intellifi.nl/api/locations");
            JObject locations = JObject.Parse(locationJson);

            
            
            foreach (JToken j in results)
            {

                var locationLabel = from l in locations["results"]
                             where j["topic"]["arguments"]["1"].ToString() == l["id"].ToString()
                             select l["label"].ToString();

                var itemJson = new WebClient().DownloadString("http://olympos.intellifi.nl/api/items/" + j["topic"]["arguments"]["0"].ToString());
                JObject items = JObject.Parse(itemJson);


                
                string id = j["id"].ToString();
                string type = j["topic"]["action"].ToString();
                DateTime time = DateTime.Parse(j["time_event"].ToString(), null, System.Globalization.DateTimeStyles.RoundtripKind);
                string location = locationLabel.FirstOrDefault();
                string sporterId = items["code_hex"].ToString();
                Event e = new Event(id, type, time, location, sporterId);
                eventList.Add(e);
                foreach (Event item in eventList){
                    Console.WriteLine(item.Id);
                }
                
            }

            
            
            return eventList;
        }
    }

    public class Event
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public DateTime Time { get; set; }
        public string Location { get; set; }
        public string SporterID { get; set; }

        public Event(string id, string type, DateTime time, string location, string sporterId)
        {
            Id = id;
            Type = type;
            Time = time;
            Location = location;
            SporterID = sporterId;
        }
    }
}
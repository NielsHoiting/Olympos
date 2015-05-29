using System;
using System.ServiceModel.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Timers;
using System.Runtime.Serialization;


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
            while (nextUrl != "" && i < 3)
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

            var locationJson = new WebClient().DownloadString("http://olympos.intellifi.nl/api/locations");
            JObject locations = JObject.Parse(locationJson);

            Console.WriteLine(DateTime.Now.ToString());

            Dictionary<string, string> itemDictionary = new Dictionary<string, string>();

            foreach (JToken j in results)
            {

                var locationLabel = from l in locations["results"]
                             where j["topic"]["arguments"]["1"].ToString() == l["id"].ToString()
                             select l["label"].ToString();

                string sporterId = "IDNOTFOUND";
                if (itemDictionary.ContainsKey(j["topic"]["arguments"]["0"].ToString()))
                {
                    sporterId = itemDictionary[j["topic"]["arguments"]["0"].ToString()];
                }
                else
                {
                    var itemJson = new WebClient().DownloadString("http://olympos.intellifi.nl/api/items/" + j["topic"]["arguments"]["0"].ToString());
                    JObject items = JObject.Parse(itemJson);
                    sporterId = items["code_hex"].ToString();
                    itemDictionary.Add(j["topic"]["arguments"]["0"].ToString(), sporterId);
                }
                
                string eventId = j["id"].ToString();
                string type = j["topic"]["action"].ToString();
                string time = j["time_created"].ToString();
                string location = locationLabel.FirstOrDefault();
                Event e = new Event(eventId, type, time, location, sporterId);
                eventList.Add(e);


                
                
            }

            Console.WriteLine(DateTime.Now.ToString());
            return eventList;
        }
    }
    [DataContract]
    public class Event
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string Time { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public string SporterID { get; set; }

        public Event(string id, string type, string time, string location, string sporterId)
        {
            Id = id;
            Type = type;
            Time = time;
            Location = location;
            SporterID = sporterId;
        }
    }
}
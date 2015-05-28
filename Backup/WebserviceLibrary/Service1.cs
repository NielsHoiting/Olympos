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
            o.get
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(o.ToString()));
            return ms;
        }
    }

    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
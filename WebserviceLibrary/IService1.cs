using System.ServiceModel;
using System.IO;
using System.Collections.Generic;

namespace WebserviceLibrary
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        List<Event> GetData(string pages);

        [OperationContract]
        List<Event> GetDataById(string pages, string id);

        [OperationContract]
        List<Event> GetDataBySpot(string pages, string spot);

    }
}
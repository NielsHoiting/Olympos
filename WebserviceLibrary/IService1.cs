using System.ServiceModel;
using System.IO;
using System.Collections.Generic;

namespace WebserviceLibrary
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        List<Event> GetData();

        [OperationContract]
        string GetJson();
    }
}
using System.ServiceModel;
using System.IO;

namespace WebserviceLibrary
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        Stream GetData();

        [OperationContract]
        string GetJson();
    }
}
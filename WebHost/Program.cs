using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using WebserviceLibrary;

namespace Persistence
{
    public class program
    {
        static void Main(string[] args) 
        {
            using (ServiceHost host = new ServiceHost(typeof(Service1)))
            {
                host.Open();
                Console.WriteLine("Service ready");

                Console.ReadKey();
            }
        }

    }
}

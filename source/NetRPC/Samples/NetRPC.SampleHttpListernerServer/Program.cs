using NetRPC.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC.SampleHttpListernerServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new HttpListenerHost(Consts.URI))
            {
                host.AddEndpoint<IExample>(() => new ExampleService(), "Example");
                Console.WriteLine("Host open");
                Console.ReadKey();
            }
        }
    }
    public class Consts
    {
        public static string URI = "http://localhost:9999/";
    }
    interface IExample
    {
        string Echo(string input);
        void CallVoid(string input);
        string GetResponse();
    }

    class ExampleService : IExample
    {
        public string Echo(string input)
        {
            Console.WriteLine("Returning {0}", input);
            return input;
        }


        public void CallVoid(string input)
        {
            Console.WriteLine("Got {0}", input);

        }

        public string GetResponse()
        {
            var output = "Response";
            Console.WriteLine("Returning {0}", output);
            return output;

        }
    }
}
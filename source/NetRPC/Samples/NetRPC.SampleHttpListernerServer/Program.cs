﻿using NetRPC.Server;
using NetRPC.Server;
using NetRPC.Serialization;
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
            var container = new ServiceContainer();
            var pipe = new Pipeline(typeof(IExample),new JsonSerializer(), new DelegateServiceFactory(c => new ExampleService(), _ => { }),new DefaultInvoker());
            var endpoint = new Endpoint("Example",typeof(IExample), pipe);
            container.AddEndpoint(endpoint);
            using (var host = new HttpListenerHost(Consts.URI, container))
            {
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
        void CallVoid();
        void CallMethod(string input);
        string GetResponse();
    }

    class ExampleService : IExample
    {
        public string Echo(string input)
        {
            Console.WriteLine("Returning {0}", input);
            return input;
        }


        public void CallVoid()
        {
            Console.WriteLine("Got call");

        }
        public void CallMethod(string input)
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
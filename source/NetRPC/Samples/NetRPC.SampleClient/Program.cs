using NetRPC.SampleHttpListernerServer;
using NetRPC.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetRPC.SampleClient
{
    class Program
    {
        private static ISerializer serializer = new JsonSerializer();
        static void Main(string[] args)
        {
           
            var callvoid = new Request
            {
                CallId = Guid.NewGuid(),
                SessionId = Guid.NewGuid(),
                Method = "CallVoid",
                Version = Constants.Version
            };

            Call(serializer.SerializeRequest(callvoid));




            Console.ReadKey();
        }

        public static void Call(string payload)
        {
            var client = WebRequest.Create(Consts.URI + "Example");
            client.Method = "POST";
            client.ContentType = "rpc/json";
            var stream = client.GetRequestStream();
            stream.Write(payload.ToByteArray(), 0, payload.ToByteArray().Length);
            stream.Close();
            var resp = client.GetResponse();

            var responseStream = resp.GetResponseStream();
            var respBytes = new byte[responseStream.Length];
            responseStream.Read(respBytes, 0, (int)responseStream.Length);
            Console.WriteLine(Encoding.UTF8.GetString(respBytes));
        }
    }
}

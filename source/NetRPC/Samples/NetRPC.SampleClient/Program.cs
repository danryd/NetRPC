using NetRPC.SampleHttpListernerServer;
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
        static void Main(string[] args)
        {

            string payload = "{\"Version\": \"0.5\", \"Method\": \"Echo\", \"params\": [\"oi\"], \"id\":\""+Guid.NewGuid().ToString()+"\" }";
            byte[] bytes = Encoding.UTF8.GetBytes(payload);
            var client = WebRequest.Create(Consts.URI + "Example");
            client.Method = "POST";
            client.ContentType = "rpc/json";
            var stream = client.GetRequestStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
            var resp = client.GetResponse();

            var responseStream = resp.GetResponseStream();
            var respBytes = new byte[responseStream.Length];
            responseStream.Read(respBytes, 0, (int)responseStream.Length);

            Console.WriteLine(Encoding.UTF8.GetString(respBytes));

            Console.ReadKey();
        }
    }
}

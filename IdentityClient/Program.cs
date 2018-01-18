using IdentityModel.Client;
using System;

namespace IdentityClient
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Do();
            Console.Read();
        }

        public static async void Do()
        {
            // 从元数据中发现端口
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");

            var tokenClient = new TokenClient(disco.TokenEndpoint, "pwdclient", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("abc","123");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.Read();
        }
    }
}

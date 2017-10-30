using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
            Console.ReadKey();
        }

        private static async Task MainAsync()
        {
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            //if (disco.IsError)     //需要启动Server项目
            
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("农码一生", "mima", "api1");
            //if (tokenResponse.IsError)//需要启动Api项目 

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:5001/api/values/get?str=私密相册吧");
            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(content);

            var client2 = new HttpClient();
            //这里可以正常访问，因为get2没有加验证
            var get2String = await client2.GetStringAsync("http://localhost:5001/api/values/get2?str=私密相册吧");
            //这里不可以访问，因为get加了验证，且没带AccessToken
            var getString = await client2.GetStringAsync("http://localhost:5001/api/values/get?str=私密相册吧");
        }
    }
}

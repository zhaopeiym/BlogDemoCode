using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace _3_ClientA
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1Async()
        {
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            Assert.True(!disco.IsError);

            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");
            Assert.True(!tokenResponse.IsError);

            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var response = await client.GetAsync("http://localhost:5001/api/Values?str=可能是私密相册");
            Assert.True(response.IsSuccessStatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.True("可能是私密相册" == content);

            //如果直接访问 不添加AccessToken，是不能访问的。因为这个api已经被
            //var client2 = new HttpClient();
            //var temp = await client2.GetAsync("http://localhost:5001/api/Values?str=可能是私密相册");
        }
    }
}

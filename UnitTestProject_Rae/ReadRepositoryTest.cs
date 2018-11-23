using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RaeClass;
using RaeClass.Models;
using RaeClass.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject_Rae
{
    [TestClass]
    public class ReadRepositoryTest
    {

        HttpClient client;

        [TestInitialize]
        public void Init()
        {
            var builder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseEnvironment("Development");
            var server = new TestServer(builder);
            var client = server.CreateClient();
            // client always expects json results
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [TestMethod]
        public async Task AddAsyncTest()
        {
            var response = await client.GetAsync($"api/Read/GetReadList");
        }
    }
}

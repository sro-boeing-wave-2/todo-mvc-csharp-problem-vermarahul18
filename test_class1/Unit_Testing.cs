using google_keep;
using google_keep.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace XUnitTestProject1
{
    public class IntegrationTest
    {
        private HttpClient _client;

        public IntegrationTest()
        {
            var host = new TestServer(
                new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<Startup>());

            _client = host.CreateClient();


        }

        Note note1 = new Note
        {
            Title = "Title-1-Deletable",
            text = "Message-1-Deletable",
            checklist = new List<CheckList>()
                       {
                           new CheckList(){ Check = "checklist-1", isChecked = true},
                           new CheckList(){ Check = "checklist-2", isChecked = false}
                       },
            labels = new List<Labels>()
                       {
                           new Labels(){label = "Label-1-Deletable"},
                           new Labels(){ label = "Label-2-Deletable"}
                       },
            Pinned = false
        };


        //var optionsBuilder = new DbContextOptionsBuilder<TodoContext>();
        //optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

        [Fact]
        public async void TestRequest()
        {
            var response = await _client.GetAsync("/api/Notes");
            response.EnsureSuccessStatusCode();

            //var responseString = await response.Content.ReadAsStringAsync();
            //Console.WriteLine(responseBody);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

     
        [Fact]
        public async void Testpost()
        {
            var json = JsonConvert.SerializeObject(note1);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var Response = await _client.PostAsync("/api/Notes", stringContent);
           // var ResponseGet = await _client.GetAsync("/api/Notes");
            Response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, Response.StatusCode);
        }

        
    }
}
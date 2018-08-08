using google_keep;
using google_keep.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using FluentAssertions;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace XUnitTestProject1
{
    public class IntegrationTest
    {
        public HttpClient _client;
        private NoteContext _context;

        public IntegrationTest()
        {
            var host = new TestServer(
                new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<Startup>()
                );
            _context = host.Host.Services.GetService(typeof(NoteContext)) as NoteContext;
            _client = host.CreateClient();
            List<Note> TestNoteProper1 = new List<Note> { new Note()
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
                Pinned = true
            } };
            _context.Note.AddRange(TestNoteProper1);
            _context.Note.AddRange(TestNoteDelete);
            _context.SaveChanges();

        }

        Note TestNoteProper = new Note()
        {
            Id = 1,
            Title = "Title-1-Updatable",
            text = "Message-1-Updatable",
            checklist = new List<CheckList>()
                        {
                            new CheckList(){ Check = "checklist-1", isChecked = true},
                            new CheckList(){ Check = "checklist-2", isChecked = false}
                        },
            labels = new List<Labels>()
                        {
                            new Labels(){label = "Label-1-Deletable"},
                            new Labels(){ label = "Label-2-Updatable"}
                        },
            Pinned = true
        };

        Note TestNotePut = new Note
        {
            Id = 1,
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

        Note TestNotePost = new Note
        {
            Title = "Title-2-Deletable",
            text = "Message-2-Deletable",
            checklist = new List<CheckList>()
                        {
                            new CheckList(){ Check = "checklist-2-1", isChecked = true},
                            new CheckList(){ Check = "checklist-2-2", isChecked = false}
                        },
            labels = new List<Labels>()
                        {
                            new Labels(){label = "Label-2-1-Deletable"},
                            new Labels(){ label = "Label-2-2-Deletable"}
                        },
            Pinned = true
        };

        Note TestNoteDelete = new Note()
        {
            Title = "this is deleted title",
            text = "some text",
            Pinned = false,
            labels = new List<Labels>
               {
                   new Labels{ label="My First Tag" },
                   new Labels{ label = "My second Tag" },
                   new Labels{ label = "My third Tag" },
               },
            checklist = new List<CheckList>
               {
               new CheckList{Check="first item" , isChecked = true},
               new CheckList{Check="second item", isChecked = true},
               new CheckList{Check="third item", isChecked = true},
               }
        };

        [Fact]
        public async void TestGet()
        {
            var response = await _client.GetAsync("/api/Notes");
            var responsestring = await response.Content.ReadAsStringAsync();
           // var responsenote = JsonConvert.DeserializeObject<List<Note>>(responsestring);
            responsestring.Should().Contain("Message-1-Deletable")
              .And.Contain("some text")
              .And.Contain("true");
            //Console.WriteLine(responsenote.ToString());
            //Assert.Equal(HttpStatusCode.OK, response. StatusCode);

        }

      
        [Fact]
        public async void PostData()
        {
            HttpRequestMessage postMessage = new HttpRequestMessage(HttpMethod.Post, "api/Notes")
            {
                Content = new StringContent(JsonConvert.SerializeObject(TestNotePost), UnicodeEncoding.UTF8, "application/json")
            };
            var response = await _client.SendAsync(postMessage);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<Note>(responseString);
            Assert.True(TestNotePost.IsEquals(obj));

        }

        [Fact]
        public async void TestPut()
        {
            HttpRequestMessage putMessage = new HttpRequestMessage(HttpMethod.Put, "api/Notes/1")
            {
                Content = new StringContent(JsonConvert.SerializeObject(TestNotePut), UnicodeEncoding.UTF8, "application/json")
            };
            var response = await _client.SendAsync(putMessage);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<Note>(responseString);
            Assert.True(TestNotePut.IsEquals(obj));
        }

        [Fact]
        public async void TestDelete()
        {
            var response = await _client.DeleteAsync("api/Notes?Title=this is deleted title");
            var responsecode = response.StatusCode;
            Assert.Equal(HttpStatusCode.NoContent, responsecode);
            _context.Note.Should().NotContain("this is deleted title");
        }

    }
}
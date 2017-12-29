using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.Models;
using Xunit;

namespace WebApiCore.IntegrationTests.Helpers
{
    public class TestFixture : IDisposable
    {
        public TestServer Server { get; set; }
        private HttpClient _client;

        public HttpClient Client
        {
            get { return _client; }
            set { _client = value; }
        }

        public UserOut User { get; set; }

        public TestFixture()
        {
            // We must configure the realpath of the targeted project
            string appRootPath = Path.GetFullPath(Path.Combine(
                            AppContext.BaseDirectory,
                            "..", "..", "..", "..", "WebApi"));

            // set environment variables the application needs to read on start up
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
            //Environment.SetEnvironmentVariable("REGISTRY_CONFIG_FILE", Path.Combine(appRootPath, "appsettings.json"));
            //Environment.SetEnvironmentVariable("REGISTRY_DB_PASSWORD_SECRET_FILE", Path.Combine(appRootPath, "InsecureSecretFiles", "RegistryDbPassword.txt"));
            //Environment.SetEnvironmentVariable("REGISTRY_USE_DOCKER_SECRETS", "false");

            Server = new TestServer(Program.GetWebHostBuilder(appRootPath, new string[] { }));
            var client = Server.CreateClient();

            Client = client;

            SignIn_To_Get_Bearer_Token().Wait();

            //Set Authorization header with Bearer token with valid token
            Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {User.Token}");
        }


        //[Fact(DisplayName = "SignIn_To_Get_Bearer_Token")]
        public async Task SignIn_To_Get_Bearer_Token()
        {
            // Arrange
            var obj = new
            {
                Username = "admin",
                Password = "admin"
            };
            string stringData = JsonConvert.SerializeObject(obj);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");
            // Act
            var response = await Client.PostAsync($"/api/login", contentData);
            response.EnsureSuccessStatusCode();
            // Assert

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            User = JsonConvert.DeserializeObject<UserOut>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(User);
            Assert.NotNull(User?.Token);
        }

        public static IEnumerable<object[]> GetJsonObjects(string file, Type type)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), file);
            if (!File.Exists(filePath))
                throw new Exception($"Test-cases JSON data file '{file}' does not exists");
            var fileData = File.ReadAllText(filePath);
            var dataList = JsonConvert.DeserializeObject(fileData, type) as IList;
            List<object[]> array = new List<object[]>();
            foreach (var item in dataList)
            {
                array.Add(new object[] { item });
            }
            return array.ToArray();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}

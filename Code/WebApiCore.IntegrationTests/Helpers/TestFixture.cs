using BusinessCore.Security;
using BusinessCore.Services.Inventory;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
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
        private IPrincipal _appPrincipal;
        private List<Claim> _claims;
        public UserOutModel User { get; set; }

        public HttpClient Client
        {
            get { return _client; }
            set { _client = value; }
        }

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
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(User.Token) as JwtSecurityToken;
            _claims = tokenS.Claims.ToList();

            //Set App Principal with correct user details
            var service = GetService<BusinessCore.Security.IAppPrincipal>();
            _appPrincipal = service.SetPrincipal(
                Convert.ToInt32(GetClaimGetClaimTokenValue(AppClaims.UserId)),
                GetClaimGetClaimTokenValue(AppClaims.Firstname),
                GetClaimGetClaimTokenValue(AppClaims.Lastname),
                GetClaimGetClaimTokenValue(AppClaims.Username),
                Convert.ToInt32(GetClaimGetClaimTokenValue(AppClaims.CompanyId)));
        }

        private string GetClaimGetClaimTokenValue(string key)
        {
            return _claims.First(claim => claim.Type == key).Value;
        }

        public T GetService<T>() where T : class
        {
            var _services = Server.Host.Services;
            var type = typeof(T);
            var service = _services.GetService(type);
            return service as T;
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

            User = JsonConvert.DeserializeObject<UserOutModel>(await response.Content.ReadAsStringAsync());

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

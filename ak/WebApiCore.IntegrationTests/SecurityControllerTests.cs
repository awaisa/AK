using WebApiCore.IntegrationTests.Helpers;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using WebApiCore.Models;

namespace WebApiCore.IntegrationTests
{
    [Collection("Database collection")]
    public class SecurityControllerTests : IClassFixture<TestFixture>
    {
        private TestFixture _testFixture;

        public SecurityControllerTests(TestFixture testFixture)
        {
            _testFixture = testFixture;
        }

        [Fact(DisplayName = "IsAuthenticated")]
        public async Task IsAuthenticated()
        {
            // Act
            var response = await _testFixture.Client.GetAsync($"/api/isAuthenticated");
            response.EnsureSuccessStatusCode();
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var isAuthenticated = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //Assert.True(jwToken.expiration > DateTime.UtcNow);
            //Assert.True(jwToken.token.Split('.').Length == 3);
        }
    }
}

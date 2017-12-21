using WebApiCore.IntegrationTests.Helpers;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace WebApiCore.IntegrationTests
{
    [Collection("Database collection")]
    public class InventoryControllerTests : IClassFixture<TestFixture>
    {
        private TestFixture _testFixture;
        readonly string _apiUrl = "/api/Inventory";

        public InventoryControllerTests(TestFixture testFixture)
        {
            _testFixture = testFixture;
        }
        
        /// <summary>
        /// This is a bad test, it is just to verify requests can be made via the TestServer
        /// </summary>
        [Fact]
        public async Task WhenGet_ThenReturnsOk()
        {
            var response = await _testFixture.Client.GetAsync($"{_apiUrl}/");
            var contents = await response.Content.ReadAsStringAsync();
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Expected OK but received {response.StatusCode}");
        }
    }
}

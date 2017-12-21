using WebApiCore.IntegrationTests.Helpers;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using WebApiCore.Models.Purchase;
using FluentAssertions;

namespace WebApiCore.IntegrationTests
{
    [Collection("Database collection")]
    public class PurchaseControllerTests : IClassFixture<TestFixture>
    {
        private TestFixture _testFixture;
        readonly string _baseApiUrl = "/api/Purchase";
        public PurchaseControllerTests(TestFixture testFixture)
        {
            _testFixture = testFixture;
        }
        
        /// <summary>
        /// This is a bad test, it is just to verify requests can be made via the TestServer
        /// </summary>
        [Fact]
        public async Task WhenGet_ThenReturnsOk()
        {
            var response = await _testFixture.Client.GetAsync($"{_baseApiUrl}/");
            var contents = await response.Content.ReadAsStringAsync();
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Expected OK but received {response.StatusCode}");
        }


        /// <summary>
        /// This is a bad test, it is just to verify requests can be made via the TestServer
        /// </summary>
        [Fact]
        public async Task WhenPost_ThenReturnsOk()
        {
            var guid = Guid.NewGuid().ToString().Replace("-", "");
            var obj = new InvoiceModel()
            {
                Date = DateTime.Now,
                No = guid.Substring(0, 10),
                Description = guid,
                VendorInvoiceNo = guid.Substring(10),
                //VendorId
            };
            string stringData = JsonConvert.SerializeObject(obj);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");
            // Act
            var response = await _testFixture.Client.PostAsync($"{_baseApiUrl}/Invoice", contentData);
            var contents = await response.Content.ReadAsStringAsync();
            //Assert.True(response.StatusCode == HttpStatusCode.OK);

            response.StatusCode.Should().Be(HttpStatusCode.OK, $"Expected OK but received {response.StatusCode}");

        }
    }
}

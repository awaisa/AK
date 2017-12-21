using WebApiCore.IntegrationTests.Helpers;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using WebApiCore.Models.Vendor;
using System.Linq;
using FluentAssertions;

namespace WebApiCore.IntegrationTests
{
    [Collection("Database collection")]
    public class VendorControllerTests : IClassFixture<TestFixture>
    {
        private TestFixture _testFixture;
        readonly string _baseApiUrl = "/api/Vendor";
        public VendorControllerTests(TestFixture testFixture)
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
            var obj = JsonConvert.DeserializeObject<SearchModel>(contents);
            var vendor = obj.Data.FirstOrDefault();
            if (vendor != null)
            {
                response = await _testFixture.Client.GetAsync($"{_baseApiUrl}/Vendor/{vendor.Id}");
                contents = await response.Content.ReadAsStringAsync();
                var vendorById = JsonConvert.DeserializeObject<VendorModel>(contents);
                Assert.NotNull(vendorById);
            }
        }


        /// <summary>
        /// This is a bad test, it is just to verify requests can be made via the TestServer
        /// </summary>
        [Fact]
        public async Task WhenPost_ThenReturnsOk()
        {
            var guid = Guid.NewGuid().ToString().Replace("-", "");
            var obj = new VendorModel()
            {
            };
            string stringData = JsonConvert.SerializeObject(obj);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");
            // Act
            var response = await _testFixture.Client.PostAsync($"{_baseApiUrl}/Vendor", contentData);

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest, $"Expected BadRequest but received {response.StatusCode}");
            var hash = DateTime.Now.GetHashCode();
            var model = new VendorModel()
            {
                Name = $"Test Name {hash}",
                Address = $"Test Address {hash}",
                Email = $"Test Email {hash}",
                Fax = $"hash",
                No = $"{hash}",
                Phone = $"{hash}",
                Website = $"Website{hash}.test",
                TaxGroup = new Models.Common.TaxGroupModel() {
                    
                }
            };


            var contents = await response.Content.ReadAsStringAsync();
            var obk = JsonConvert.DeserializeObject(contents);
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Expected OK but received {response.StatusCode}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
        }
    }
}

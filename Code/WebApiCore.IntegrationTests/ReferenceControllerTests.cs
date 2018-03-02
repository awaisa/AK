using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.IntegrationTests.Helpers;
using Xunit;

namespace WebApiCore.IntegrationTests
{
    [Collection("Database collection")]
    public class ReferenceControllerTests : IClassFixture<TestFixture>
    {
        private TestFixture _testFixture;
        readonly string _baseApiUrl = "/api/Reference";

        public ReferenceControllerTests(TestFixture testFixture)
        {
            _testFixture = testFixture;
        }


        [Fact]
        public async Task Get_All_Items_Then_Returns_Ok()
        {
            var response = await _testFixture.Client.GetAsync($"{_baseApiUrl}/GetCatagory");
            var contents = await response.Content.ReadAsStringAsync();
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Expected OK but received {response.StatusCode}");
        }

        [Fact]
        public async Task Get_All_Brand_Then_Returns_Ok()
        {
            var response = await _testFixture.Client.GetAsync($"{_baseApiUrl}/GetBrand");
            var contents = await response.Content.ReadAsStringAsync();
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Expected OK but received {response.StatusCode}");
        }

        [Fact]
        public async Task Get_All_Model_Then_Returns_Ok()
        {
            var response = await _testFixture.Client.GetAsync($"{_baseApiUrl}/GetModel");
            var contents = await response.Content.ReadAsStringAsync();
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Expected OK but received {response.StatusCode}");
        }

        [Fact]
        public async Task Get_All_TaxGroup_Then_Returns_Ok()
        {
            var response = await _testFixture.Client.GetAsync($"{_baseApiUrl}/GetItemTaxGroup");
            var contents = await response.Content.ReadAsStringAsync();
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Expected OK but received {response.StatusCode}");
        }

        [Fact]
        public async Task Get_All_Measuremets_Then_Returns_Ok()
        {
            var response = await _testFixture.Client.GetAsync($"{_baseApiUrl}/GetMeasuremets");
            var contents = await response.Content.ReadAsStringAsync();
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Expected OK but received {response.StatusCode}");
        }

        [Fact]
        public async Task Get_All_Accounts_Then_Returns_Ok()
        {
            var response = await _testFixture.Client.GetAsync($"{_baseApiUrl}/GetAccounts");
            var contents = await response.Content.ReadAsStringAsync();
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Expected OK but received {response.StatusCode}");
        }

        [Fact]
        public async Task Get_All_Vendors_Then_Returns_Ok()
        {
            var response = await _testFixture.Client.GetAsync($"{_baseApiUrl}/GetVendors");
            var contents = await response.Content.ReadAsStringAsync();
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Expected OK but received {response.StatusCode}");
        }

        [Fact]
        public async Task Get_Items_Then_Returns_Ok()
        {
            var response = await _testFixture.Client.GetAsync($"{_baseApiUrl}/GetItems");
            var contents = await response.Content.ReadAsStringAsync();
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Expected OK but received {response.StatusCode}");
        }

        [Fact]
        public async Task Get_All_Taxes_Then_Returns_Ok()
        {
            var response = await _testFixture.Client.GetAsync($"{_baseApiUrl}/GetTaxes");
            var contents = await response.Content.ReadAsStringAsync();
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Expected OK but received {response.StatusCode}");
        }
    }
}

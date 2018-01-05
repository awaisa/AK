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
using System.Collections.Generic;
using WebApiCore.Models.Customer;

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
        /// Post vendor with valid data to check if all validation pass and data saved
        /// </summary>
        /// <param name="vendorModel"></param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(TestFixture.GetJsonObjects), @"TestData\VendorControllerTests-ValidVendor.json", typeof(List<VendorModel>), MemberType = typeof(TestFixture))]
        public async Task Post_vendor_With_Valid_Data_Then_Returns_Ok(VendorModel vendorModel)
        {
            string stringData = JsonConvert.SerializeObject(vendorModel);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");
            // Act
            var response = await _testFixture.Client.PostAsync($"{_baseApiUrl}", contentData);
            var contents = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK, $"Expected OK but received {response.StatusCode}.");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var obsj = JsonConvert.DeserializeObject<VendorModel>(contents);

                //update by id
                vendorModel.Party.Name = vendorModel.Party.Name + "changed";
                vendorModel.Party.Email = vendorModel.Party.Email.Replace("@", "1@");
                vendorModel.Party.Website = vendorModel.Party.Website.Replace(".com", "1.com");
                vendorModel.No = vendorModel.No + "1";
                vendorModel.Id = obsj.Id;
                string Data = JsonConvert.SerializeObject(vendorModel);
                var content = new StringContent(Data, Encoding.UTF8, "application/json");
                var updateResponse = await _testFixture.Client.PutAsync($"{_baseApiUrl}", content);
                var updateContants = await updateResponse.Content.ReadAsStringAsync();
                updateResponse.StatusCode.Should().Be(HttpStatusCode.OK, $"Expected ok but received {updateResponse.StatusCode}");

                //VendorGetby id
                var Response = await _testFixture.Client.GetAsync($"{_baseApiUrl}/{obsj.Id}");
                var contants = await Response.Content.ReadAsStringAsync();
                Response.StatusCode.Should().Be(HttpStatusCode.OK, $"Expected OK by id but received {Response.StatusCode}");

                //Delete
                var deleteResponse = await _testFixture.Client.DeleteAsync($"{_baseApiUrl}/{obsj.Id}");
                deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK, $"Deleted expected OK but received {deleteResponse.StatusCode}");

                //verify deleted 
                var DeleteResponse = await _testFixture.Client.GetAsync($"{_baseApiUrl}/{obsj.Id}");
                var DeleteContants = await DeleteResponse.Content.ReadAsStringAsync();
                DeleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound, $"Expected NotFound but received {DeleteResponse.StatusCode}");
            }
        }





        /// <summary>
        /// Post vendor with in-valid data to check if all validation rules apply and return bad request
        /// </summary>
        /// <param name="vendorModel"></param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(TestFixture.GetJsonObjects), @"TestData\CustomerControllerTests-InvalidCustomer.json", typeof(List<VendorModel>), MemberType = typeof(TestFixture))]
        public async Task Post_Customer_With_InValid_Data_Then_Returns_Bad_Request(VendorModel vendorModel)
        {
            string stringData = JsonConvert.SerializeObject(vendorModel);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");
            // Act
            var response = await _testFixture.Client.PostAsync($"{_baseApiUrl}", contentData);
            var contents = await response.Content.ReadAsStringAsync();
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest, $"Expected BadRequest but received {response.StatusCode}");
        }

        [Fact]
        public async Task Get_All_Customers_Then_Returns_Ok()
        {
            var response = await _testFixture.Client.GetAsync($"{_baseApiUrl}/");
            var contents = await response.Content.ReadAsStringAsync();
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Expected OK but received {response.StatusCode}");
        }
        
    }
}

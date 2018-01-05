using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.IntegrationTests.Helpers;
using WebApiCore.Models.Customer;
using Xunit;
using FluentAssertions;

namespace WebApiCore.IntegrationTests
{
    [Collection("Database collection")]
    public class CustomerControllerTests : IClassFixture<TestFixture>
    {
        private TestFixture _testFixture;
        readonly string _baseApiUrl = "/api/Customer";
        public CustomerControllerTests(TestFixture testFixture)
        {
            _testFixture = testFixture;
        }

        /// <summary>
        /// Post customer with valid data to check if all validation pass and data saved
        /// </summary>
        /// <param name="customerModel"></param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(TestFixture.GetJsonObjects), @"TestData\CustomerControllerTests-ValidCustomer.json", typeof(List<CustomerModel>), MemberType = typeof(TestFixture))]
        public async Task Post_Customer_With_Valid_Data_Then_Returns_Ok(CustomerModel customerModel)
        {
            string stringData = JsonConvert.SerializeObject(customerModel);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");
            // Act
            var response = await _testFixture.Client.PostAsync($"{_baseApiUrl}", contentData);
            var contents = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK, $"Expected OK but received {response.StatusCode}.");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var obsj = JsonConvert.DeserializeObject<CustomerModel>(contents);
                
                
                //CustomerGetby id
                var Response = await _testFixture.Client.GetAsync($"{_baseApiUrl}/{obsj.Id}");
                var contants = await Response.Content.ReadAsStringAsync();
                Response.StatusCode.Should().Be(HttpStatusCode.OK, $"Expected OK by id but received {Response.StatusCode}");

                //GetAllCustomers
                var Allresponse = await _testFixture.Client.GetAsync($"{_baseApiUrl}/");
                var Allcontents = await Allresponse.Content.ReadAsStringAsync();
                Allresponse.StatusCode.Should().Be(HttpStatusCode.OK, $"Expected OK but received {Allresponse.StatusCode}");

                //update by id
                customerModel.Party.Name = customerModel.Party.Name + "changed";
                customerModel.Party.Email = customerModel.Party.Email.Replace("@", "1@");
                customerModel.Party.Website = customerModel.Party.Website.Replace(".com", "1.com");
                customerModel.No = customerModel.No + "1";
                customerModel.Id = obsj.Id;
                string Data = JsonConvert.SerializeObject(customerModel);
                var content = new StringContent(Data, Encoding.UTF8, "application/json");
                var updateResponse = await _testFixture.Client.PutAsync($"{_baseApiUrl}", content);
                var updateContants = await updateResponse.Content.ReadAsStringAsync();
                updateResponse.StatusCode.Should().Be(HttpStatusCode.OK, $"Expected ok but received {updateResponse.StatusCode}");

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
        /// Post customer with in-valid data to check if all validation rules apply and return bad request
        /// </summary>
        /// <param name="customerModel"></param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(TestFixture.GetJsonObjects), @"TestData\CustomerControllerTests-InvalidCustomer.json", typeof(List<CustomerModel>), MemberType = typeof(TestFixture))]
        public async Task Post_Customer_With_InValid_Data_Then_Returns_Bad_Request(CustomerModel customerModel)
        {
            string stringData = JsonConvert.SerializeObject(customerModel);
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

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
using WebApiCore.Models.Sale;
using Xunit;
using FluentAssertions;
using BusinessCore.Services.Inventory;
using BusinessCore.Services.Sales;

namespace WebApiCore.IntegrationTests
{
    [Collection("Database collection")]
    public class SaleControllerTests : IClassFixture<TestFixture>
    {
        private TestFixture _testFixture;
        readonly string _baseApiUrl = "/api/Sale";
        public SaleControllerTests(TestFixture testFixture)
        {
            _testFixture = testFixture;
        }

        /// <summary>
        /// Post sale invoice with valid data to check if all validation pass and data saved
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(TestFixture.GetJsonObjects), @"TestData\SaleControllerTests-ValidSaleInvoices.json", typeof(List<InvoiceModel>), MemberType = typeof(TestFixture))]
        public async Task Post_Sale_Invoice_With_Valid_Data_Then_Returns_Ok(InvoiceModel model)
        {
            string stringData = JsonConvert.SerializeObject(model);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");
            // Act

            var inventoryService = _testFixture.GetService<IInventoryService>();
            var salesService = _testFixture.GetService<ISalesService>();
            var items = inventoryService.GetAllItems().ToList();
            var customer = salesService.GetCustomers().FirstOrDefault();

            var response = await _testFixture.Client.PostAsync($"{_baseApiUrl}/Invoice", contentData);
            var contents = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK, $"Expected OK but received {response.StatusCode}.");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                //CustomerGetby id
                var obsj = JsonConvert.DeserializeObject<InvoiceModel>(contents);
                var Response = await _testFixture.Client.GetAsync($"{_baseApiUrl}/Invoice/{obsj.Id}");
                var contants = await Response.Content.ReadAsStringAsync();
                Response.StatusCode.Should().Be(HttpStatusCode.OK, $"Expected OK but received {Response.StatusCode}");

                //Delete
                var deleteResponse = await _testFixture.Client.DeleteAsync($"{_baseApiUrl}/Invoice/{obsj.Id}");
                deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK, $"Delete expected OK but received {deleteResponse.StatusCode}");
            }
        }

        /// <summary>
        /// Post sale invoice with in-valid data to check if all validation rules apply and return bad request
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(TestFixture.GetJsonObjects), @"TestData\CustomerControllerTests-InvalidCustomer.json", typeof(List<InvoiceModel>), MemberType = typeof(TestFixture))]
        public async Task Post_Sale_Invoice_With_InValid_Data_Then_Returns_Bad_Request(InvoiceModel model)
        {
            string stringData = JsonConvert.SerializeObject(model);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");
            // Act
            var response = await _testFixture.Client.PostAsync($"{_baseApiUrl}/Invoice", contentData);
            var contents = await response.Content.ReadAsStringAsync();
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest, $"Expected BadRequest but received {response.StatusCode}");
        }

        [Fact]
        public async Task Get_All_Sale_Invoices_Then_Returns_Ok()
        {
            var response = await _testFixture.Client.GetAsync($"{_baseApiUrl}/Invoice");
            var contents = await response.Content.ReadAsStringAsync();
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Expected OK but received {response.StatusCode}");
        }
    }
}

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
    [CollectionDefinition("Database collection")]
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

            //insert invoice 
            var response = await _testFixture.Client.PostAsync($"{_baseApiUrl}/Invoice", contentData);
            var contents = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK, $"Expected OK but received {response.StatusCode}.");

            if (response.StatusCode == HttpStatusCode.OK)
            {

                //invoiceGetby id
                var obsj = JsonConvert.DeserializeObject<InvoiceModel>(contents);
                var Response = await _testFixture.Client.GetAsync($"{_baseApiUrl}/Invoice/{obsj.Id}");
                var contants = await Response.Content.ReadAsStringAsync();
                Response.StatusCode.Should().Be(HttpStatusCode.OK, $"Expected OK but received {Response.StatusCode}");

                //GetAll invoice
                var Allresponse = await _testFixture.Client.GetAsync($"{_baseApiUrl}/Invoice");
                var Allcontents = await Allresponse.Content.ReadAsStringAsync();
                Allresponse.StatusCode.Should().Be(HttpStatusCode.OK, $"Expected ok but recieved{Allresponse.StatusCode }");

                //update by id
                model.Description = model.Description + " Changed";
                model.No = model.No + 9;
                model.Id = obsj.Id;
                string Data = JsonConvert.SerializeObject(model);
                var content = new StringContent(Data, Encoding.UTF8, "application/json");
                var updateResponse = await _testFixture.Client.PostAsync($"{_baseApiUrl}/Invoice", content);
                var updateContants = await updateResponse.Content.ReadAsStringAsync();
                updateResponse.StatusCode.Should().Be(HttpStatusCode.OK, $"Expected ok but received {updateResponse.StatusCode}");

                //UpdatedinvoiceGetby id
                var UpdatedResponse = await _testFixture.Client.GetAsync($"{_baseApiUrl}/Invoice/{obsj.Id}");
                var Updatedcontants = await UpdatedResponse.Content.ReadAsStringAsync();
                UpdatedResponse.StatusCode.Should().Be(HttpStatusCode.OK, $"Expected OK but received {UpdatedResponse.StatusCode}");


                //Delete
                var deleteResponse = await _testFixture.Client.DeleteAsync($"{_baseApiUrl}/Invoice/{obsj.Id}");
                var deletecontants = await deleteResponse.Content.ReadAsStringAsync();
                deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK, $"Delete expected OK but received {deleteResponse.StatusCode}");

                //Verify Deleted
                var DeletedResponse = await _testFixture.Client.GetAsync($"{_baseApiUrl}/Invoice/{obsj.Id}");
                var Deletedcontants = await DeletedResponse.Content.ReadAsStringAsync();
                DeletedResponse.StatusCode.Should().Be(HttpStatusCode.NotFound, $"Expected OK but received {DeletedResponse.StatusCode}");

            }
        }

        /// <summary>
        /// Post sale invoice with in-valid data to check if all validation rules apply and return bad request
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(TestFixture.GetJsonObjects), @"TestData\SaleControllerTests-InvalidSaleInvoices.json", typeof(List<InvoiceModel>), MemberType = typeof(TestFixture))]
        public async Task Post_Sale_Invoice_With_InValid_Data_Then_Returns_Bad_Request(InvoiceModel model)
        {
            string stringData = JsonConvert.SerializeObject(model);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");
         
            var response = await _testFixture.Client.PostAsync($"{_baseApiUrl}/Invoice", contentData);
            var contents = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK, $"Expected OK but received {response.StatusCode}.");
        }

        [Fact]
        public async Task Get_All_Sale_Invoices_Then_Returns_Ok()
        {
            var response = await _testFixture.Client.GetAsync($"{_baseApiUrl}/Invoice");
            var contents = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK, $"Expected OK but received{ response.StatusCode}.");
        }
    }
}

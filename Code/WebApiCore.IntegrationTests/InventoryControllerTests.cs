using WebApiCore.IntegrationTests.Helpers;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using BusinessCore.Services.Inventory;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using FluentAssertions;
using WebApiCore.Models.Inventory;

namespace WebApiCore.IntegrationTests
{
    [Collection("Database collection")]
    public class InventoryControllerTests : IClassFixture<TestFixture>
    {
        private TestFixture _testFixture;
        readonly string _baseApiUrl = "/api/Inventory";

        public InventoryControllerTests(TestFixture testFixture)
        {
            _testFixture = testFixture;
        }



        /// <summary>
        /// Post customer with valid data to check if all validation pass and data saved
        /// </summary>
        /// <param name="ItemModel"></param>
        /// <returns></returns>
        [Theory]
        [MemberData(nameof(TestFixture.GetJsonObjects), @"TestData\InventoryControllerTests-ValidItems.json", typeof(List<ItemModel>), MemberType = typeof(TestFixture))]
        public async Task Post_Inventory_With_Valid_Data_Then_Returns_Ok(ItemModel itemModel)
        {
            var inventoryService = _testFixture.GetService<IInventoryService>();
            var items = inventoryService.GetItemCategories().LastOrDefault();
            //itemModel.Id = items.Id;
            itemModel.ItemCategoryId = items.ItemCategoryId;
            //itemModel.ModelId = items.ModelId;
            //itemModel.BrandId = items.BrandId;
            itemModel.InventoryAccountId = items.InventoryAccountId;
            itemModel.MeasurementId = items.MeasurementId;
            string stringData = JsonConvert.SerializeObject(itemModel);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");
            // Act
            var response = await _testFixture.Client.PostAsync($"{_baseApiUrl}", contentData);
            var contents = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK, $"Expected OK but received {response.StatusCode}.");


            if (response.StatusCode == HttpStatusCode.OK)
            {
                var obsj = JsonConvert.DeserializeObject<ItemModel>(contents);

                //update by id
                itemModel.PurchaseDescription = itemModel.PurchaseDescription + " Changed";
                itemModel.Cost = itemModel.Cost + 9;
                itemModel.Code = itemModel.Code + 9;
                itemModel.Description = itemModel.Description+" changed";
                itemModel.Id = obsj.Id;
                string Data = JsonConvert.SerializeObject(itemModel);
                var content = new StringContent(Data, Encoding.UTF8, "application/json");
                var updateResponse = await _testFixture.Client.PutAsync($"{_baseApiUrl}", content);
                var updateContants = await updateResponse.Content.ReadAsStringAsync();
                updateResponse.StatusCode.Should().Be(HttpStatusCode.OK, $"Expected ok but received {updateResponse.StatusCode}");

                //CustomerGetby id
                var Response = await _testFixture.Client.GetAsync($"{_baseApiUrl}/{obsj.Id}");
                var contants = await Response.Content.ReadAsStringAsync();
                Response.StatusCode.Should().Be(HttpStatusCode.OK, $"Expected OK by id but received {Response.StatusCode}");

                //GetAll Items
                var resonse = await _testFixture.Client.GetAsync($"{_baseApiUrl}/");
                var cotents = await resonse.Content.ReadAsStringAsync();
                Assert.True(resonse.StatusCode == HttpStatusCode.OK, $"Expected OK but received {resonse.StatusCode}");

                //Delete
                var deleteResponse = await _testFixture.Client.DeleteAsync($"{_baseApiUrl}/{obsj.Id}");
                var DeleteContant = await deleteResponse.Content.ReadAsStringAsync();
                deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK, $"Deleted expected OK but received {deleteResponse.StatusCode}");

                //verify deleted 
                var DeleteResponse = await _testFixture.Client.GetAsync($"{_baseApiUrl}/{obsj.Id}");
                var DeleteContants = await DeleteResponse.Content.ReadAsStringAsync();
                DeleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound, $"Expected NotFound but received {DeleteResponse.StatusCode}");

            }
        }
        
        [Theory]
        [MemberData(nameof(TestFixture.GetJsonObjects), @"TestData\InventoryControllerTests-InvalidItems.json", typeof(List<ItemModel>), MemberType = typeof(TestFixture))]
        public async Task Post_Inventory_With_InValid_Data_Then_Returns_Ok(ItemModel itemModel)
        {
            var inventoryService = _testFixture.GetService<IInventoryService>();
            var items = inventoryService.GetItemCategories().FirstOrDefault();
            itemModel.Id = items.Id;
            itemModel.ItemCategoryId = items.ItemCategoryId;
            //itemModel.ModelId = items.ModelId;
            //itemModel.BrandId = items.BrandId;
            itemModel.InventoryAccountId = items.InventoryAccountId;
            itemModel.MeasurementId = items.MeasurementId;
            string stringData = JsonConvert.SerializeObject(itemModel);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");
            // Act
            var response = await _testFixture.Client.PostAsync($"{_baseApiUrl}", contentData);
            var contents = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, $"Expected BadRequest but received {response.StatusCode}.");
        }

        [Fact]
        public async Task Get_All_Items_Then_Returns_Ok()
        {
            var response = await _testFixture.Client.GetAsync($"{_baseApiUrl}/");
            var contents = await response.Content.ReadAsStringAsync();
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Expected OK but received {response.StatusCode}");
        }
    }
}

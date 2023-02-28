using System.Net;
using System.Net.Http.Json;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SampleWebApi.Models;
using Xunit;

namespace SampleWebApi.IntegrationTests
{
    public class TodoItemsControllerTests
    {
        private readonly CustomWebAppFactory<Startup> _webApplicationFactory;

        private HttpClient SystemUnderTest => _webApplicationFactory.CreateClient();

        public TodoItemsControllerTests()
        {
            _webApplicationFactory = new CustomWebAppFactory<Startup>();
        }

        [SetUp]
        public async Task SetUp()
        {
            await _webApplicationFactory.Init();
        }

        [TearDown]
        public async Task TearDown()
        {
            await _webApplicationFactory.DisposeAsync();
        }

        [Fact]
        public async Task Create_ToDoItem_WhenDataIsValid()
        {
            // Arrange
            var todoItem = new TodoItem();
            todoItem.Id = 0;
            todoItem.Name = "Test";
            todoItem.IsComplete = true;
            await _webApplicationFactory.Init();

            //Act
            var response = await SystemUnderTest.PostAsJsonAsync("api/TodoItems", todoItem);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }


        [Fact]
        public async Task Delete_ToDoItem_()
        {
            // Arrange
            var todoItem = new TodoItem();
            todoItem.Id = 0;
            todoItem.Name = "Test";
            todoItem.IsComplete = true;
            await _webApplicationFactory.Init();
            await SystemUnderTest.PostAsJsonAsync("api/TodoItems", todoItem);
            var items = await SystemUnderTest.GetAsync("api/TodoItems");

            // Act
            var response = await SystemUnderTest.DeleteAsync("api/TodoItems/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}

using System.Net;
using System.Net.Http.Json;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SampleWebApi.Models;

namespace SampleWebApi.IntegrationTests
{
    [TestFixture]
    public class TodoItemsControllerTests
    {
        private readonly CustomWebAppFactory<Program> _webApplicationFactory;

        private HttpClient SystemUnderTest => _webApplicationFactory.CreateClient();

        public TodoItemsControllerTests()
        {
            _webApplicationFactory = new CustomWebAppFactory<Program>();
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

        [Test]
        [AutoData]
        public async Task Create_ToDoItem_WhenDataIsValid(TodoItem todoItem)
        {
            // Arrange
            todoItem.Id = 0;

            // Act
            var response = await SystemUnderTest.PostAsJsonAsync("x", todoItem);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        [AutoData]
        public async Task Create_ToDoItem_WhenIdIsInvalid(TodoItem todoItem)
        {
            // Arrange
            todoItem.Id = -999;

            // Act
            var response = await SystemUnderTest.PostAsJsonAsync("x", todoItem);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}

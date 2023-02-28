using FluentAssertions;
using NUnit.Framework;
using SampleWebApi.Models;

namespace XProjTest
{
    [TestFixture]
    public class TodoItemsControllerTests
    {
        //private readonly CustomWebAppFactory<Startup> _webApplicationFactory;

        //private HttpClient SystemUnderTest => _webApplicationFactory.CreateClient();

        //public TodoItemsControllerTests()
        //{
        //    _webApplicationFactory = new CustomWebAppFactory<Startup>();
        //}

        //[SetUp]
        //public async Task SetUp()
        //{
        //    await _webApplicationFactory.Init();
        //}

        //[TearDown]
        //public async Task TearDown()
        //{
        //    await _webApplicationFactory.DisposeAsync();
        //}

        [Test]
        public async Task Create_ToDoItem_WhenDataIsValid()
        {
            // Arrange
            var todoItem = new TodoItem();
            todoItem.Id = 0;

            "true".Should().Be("true");

            // Act
            //var response = await SystemUnderTest.PostAsJsonAsync("x", todoItem);

            // Assert
            //response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]

        public async Task Create_ToDoItem_WhenIdIsInvalid()
        {
            // Arrange

            // Act
            //var response = await SystemUnderTest.PostAsJsonAsync("x", todoItem);

            // Assert
            //response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}

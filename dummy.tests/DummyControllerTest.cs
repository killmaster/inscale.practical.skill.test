using System.Collections.Generic;
using dummy.api.Controllers;
using dummy.contracts;
using dummy.proj1.Services.DB;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Threading.Tasks;

namespace dummy.tests;

public class DummyControllerTest
{
    private List<Models.User> _users = new(){
            new Models.User()
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe"
            },
            new Models.User()
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Doe"
            }
        };

    private List<Models.Post> _posts = new(){
            new Models.Post()
            {
                Id = 1,
                UserId = 1,
                Reactions = 1,
                Tags = new string[]
                {
                    "history"
                }
            },
            new Models.Post()
            {
                Id = 2,
                UserId = 1,
                Reactions = 2,
                Tags = new string[]
                {
                    "history"
                }
            },
            new Models.Post()
            {
                Id = 3,
                UserId = 2,
                Reactions = 3,
                Tags = new string[]
                {
                    "mathematics"
                }
            }
        };

    private List<Models.TodoModel> _todos = new(){
            new Models.TodoModel()
            {
                Id = 1,
                UserId = 1,
                Todo = "Buy milk",
                Completed = false
            },
            new Models.TodoModel()
            {
                Id = 2,
                UserId = 1,
                Todo = "Buy eggs",
                Completed = false
            },
            new Models.TodoModel()
            {
                Id = 3,
                UserId = 2,
                Todo = "Buy bread",
                Completed = false
            }
        };

    [Fact]
    public async Task GetUsersWithNReactionsAndXTag_Default_Behaviour_Async()
    {
        // Arrange
        var request = new AtLeastNReactionsAndSpecificTagRequest
        {
            Reactions = 1,
            Tag = "history"
        };

        AtLeastNReactionsAndSpecificTagResponse expectedResponse = new()
        {
            Users = new(){
                _users[0]
            }
        };

        // Just mock some users
        var dbServiceMock = new Mock<IDummyDBService>();
        dbServiceMock.Setup(x => x.AddUser(It.IsAny<Models.User>()));
        foreach (var user in _users)
        {
            await dbServiceMock.Object.AddUser(user);
        }

        // Just mock some posts
        dbServiceMock.Setup(x => x.AddPost(It.IsAny<Models.Post>()));
        foreach (var post in _posts)
        {
            await dbServiceMock.Object.AddPost(post);
        }

        // Just mock the testing logic
        dbServiceMock.Setup(x => x.GetUsersWithNReactionsAndXTag(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new List<Models.User>() { _users[0] });


        var controller = new DummyController(dbServiceMock.Object);

        // Act
        var result = await controller.UsersWithNReactionsAndXTag(request);
        var okResult = result as OkObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.IsType<AtLeastNReactionsAndSpecificTagResponse>(okResult.Value);
        Assert.Equal(expectedResponse.Users.Count, (okResult.Value as AtLeastNReactionsAndSpecificTagResponse).Users.Count);
        Assert.Equal(expectedResponse.Users[0], (okResult.Value as AtLeastNReactionsAndSpecificTagResponse).Users[0]);
    }

    [Fact]
    public async Task TodosOfUsersWithMoreThanNPosts()
    {
        // Arrange
        TodosOfUsersWithMoreThanNPostsRequest request = new()
        {
            NumberOfPosts = 2
        };

        var dbServiceMock = new Mock<IDummyDBService>();
        dbServiceMock.Setup(x => x.TodosOfUsersWithMoreThanNPosts(It.IsAny<int>()))
            .ReturnsAsync(new List<Models.TodoModel>());

        var controller = new DummyController(dbServiceMock.Object);

        // Act
        var result = await controller.TodosOfUsersWithMoreThanNPosts(request);
        var okResult = result as OkObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.IsType<TodosOfUsersWithMoreThanNPostsResponse>(okResult.Value);
    }

    [Fact]
    public async Task PostsOfUsersWithXCardType()
    {
        // Arrange
        PostsOfUsersWithXCardtypeRequest request = new()
        {
            Cardtype = "mastercard"
        };

        var dbServiceMock = new Mock<IDummyDBService>();
        dbServiceMock.Setup(x => x.PostsOfUsersWithXCardType(It.IsAny<string>()))
            .ReturnsAsync(new List<Models.Post>());

        var controller = new DummyController(dbServiceMock.Object);

        // Act
        var result = await controller.PostsOfUsersWithXCardType(request);
        var okResult = result as OkObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.IsType<PostsOfUsersWithXCardtypeResponse>(okResult.Value);
    }
}
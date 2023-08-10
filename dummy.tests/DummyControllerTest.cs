using System.Collections.Generic;
using dummy.api.Controllers;
using dummy.contracts;
using dummy.proj1.Services.DB;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace dummy.tests;

public class DummyControllerTest
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var request = new AtLeastNReactionsAndSpecificTagRequest
        {
            Reactions = 2,
            Tag = "history"
        };

        var expectedResponse = new AtLeastNReactionsAndSpecificTagResponse
        {
            Users = new List<User>()
        };

        var dbServiceMock = new Mock<IDummyDBService>();
        dbServiceMock.Setup(
            x => x.GetUsersWithNReactionsAndXTag(It.IsAny<int>(), It.IsAny<string>()))
            .Returns(expectedResponse.Users);

        var controller = new DummyController(dbServiceMock.Object);

        // Act
        var result = controller.UsersWithNReactionsAndXTag(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResponse = Assert.IsType<contracts.AtLeastNReactionsAndSpecificTagResponse>(okResult.Value);
        Assert.Equal(expectedResponse.Users, actualResponse.Users);
    }
}
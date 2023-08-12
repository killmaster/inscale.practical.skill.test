using System.Collections.Generic;
using dummy.api.Controllers;
using dummy.contracts;
using dummy.proj1.Services.DB;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Linq;
using System.Threading.Tasks;

namespace dummy.tests;

public class DummyControllerTest
{
    [Fact]
    public async Task Test1Async()
    {
        // Arrange
        var request = new AtLeastNReactionsAndSpecificTagRequest
        {
            Reactions = 2,
            Tag = "history"
        };

        var dbServiceMock = new Mock<IDummyDBService>();
        dbServiceMock.Setup(x => x.GetUsersWithNReactionsAndXTag(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(new List<Models.User>());

        var controller = new DummyController(dbServiceMock.Object);

        // Act
        var result = await controller.UsersWithNReactionsAndXTag(request);
        var okResult = result as OkObjectResult;

        // Assert
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.IsType<AtLeastNReactionsAndSpecificTagResponse>(okResult.Value);
    }
}
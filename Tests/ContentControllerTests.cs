using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using Xunit;

public class ContentControllerTests
{
    private readonly Mock<IContentService> _mockContentService;
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly ContentController _controller;

    public ContentControllerTests()
    {
        _mockContentService = new Mock<IContentService>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();

        // Mock HttpClient setup
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
               "SendAsync",
               ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>())
           .ReturnsAsync(new HttpResponseMessage
           {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent("{\"id\":1,\"name\":\"Test User\"}")
           });

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new System.Uri("https://fakeapi.com")
        };

        _mockHttpClientFactory.Setup(_ => _.CreateClient("UserService")).Returns(httpClient);

        _controller = new ContentController(_mockContentService.Object, _mockHttpClientFactory.Object);
    }

    [Fact]
    public async Task GetAllContents_ReturnsOkResult_WithListOfContents()
    {
        // Arrange
        var contents = new List<Content>
        {
            new Content { Id = 1, Title = "Content 1" },
            new Content { Id = 2, Title = "Content 2" }
        };
        _mockContentService.Setup(s => s.GetAllContentsAsync()).ReturnsAsync(contents);

        // Act
        var result = await _controller.GetAllContents();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnContents = Assert.IsType<List<Content>>(okResult.Value);
        Assert.Equal(2, returnContents.Count);
    }

    [Fact]
    public async Task CreateContent_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var content = new Content { Id = 1, Title = "New Content" };
        _mockContentService.Setup(s => s.CreateContentAsync(It.IsAny<Content>())).ReturnsAsync(content);

        // Act
        var result = await _controller.CreateContent(content);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnContent = Assert.IsType<Content>(createdAtActionResult.Value);
        Assert.Equal(content.Id, returnContent.Id);
    }

    [Fact]
    public async Task GetContentById_ContentExists_ReturnsOkResult()
    {
        // Arrange
        var content = new Content { Id = 1, Title = "Existing Content" };
        _mockContentService.Setup(s => s.GetContentByIdAsync(1)).ReturnsAsync(content);

        // Act
        var result = await _controller.GetContentById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnContent = Assert.IsType<Content>(okResult.Value);
        Assert.Equal(content.Id, returnContent.Id);
    }

    [Fact]
    public async Task GetContentById_ContentDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        _mockContentService.Setup(s => s.GetContentByIdAsync(1)).ReturnsAsync((Content)null);

        // Act
        var result = await _controller.GetContentById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateContent_UserServiceCall_Successful()
    {
        // Arrange
        var content = new Content { Id = 1, Title = "Updated Content" };
        _mockContentService.Setup(s => s.UpdateContentAsync(1, content)).ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateContent(1, content);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteContent_ContentExists_ReturnsNoContent()
    {
        // Arrange
        _mockContentService.Setup(s => s.DeleteContentAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteContent(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteContent_ContentDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        _mockContentService.Setup(s => s.DeleteContentAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteContent(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ContentApi.Controllers;
using ContentApi.Helper;
using ContentApi.Models;
using ContentApi.Models.ResponseModels;
using ContentApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using Xunit;

public class ContentControllerTests
{
    private readonly Mock<IContentService> _mockService;
    private readonly Mock<ApiClient> _mockApiClient;
    private readonly ContentController _controller;

    public ContentControllerTests()
    {
        _mockService = new Mock<IContentService>();

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var fakeHttpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new System.Uri("https://fakeapi.com/")
        };

        _mockApiClient = new Mock<ApiClient>(fakeHttpClient);
        _controller = new ContentController(_mockService.Object, _mockApiClient.Object);
    }

    [Fact]
    public async Task GetAllContents_ReturnsOkResult_WithContents()
    {
        // Arrange
        var contents = new List<Content> { new Content { Id = 1, Title = "Sample Content" } };
        _mockService.Setup(s => s.GetAllContentsAsync()).ReturnsAsync(contents);

        // Act
        var result = await _controller.GetAllContents();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseModel<IEnumerable<Content>>>(okResult.Value);
        Assert.Equal(contents, response.Data);
    }

    [Fact]
    public async Task CreateContent_ReturnsCreatedAtActionResult_WithCreatedContent()
    {
        // Arrange
        var newContent = new Content { Id = 1, Title = "New Content" };
        _mockService.Setup(s => s.CreateContentAsync(It.IsAny<Content>())).ReturnsAsync(newContent);

        // Act
        var result = await _controller.CreateContent(newContent);

        // Assert
        var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
        var response = Assert.IsType<ResponseModel<Content>>(createdAtResult.Value);
        Assert.Equal(newContent, response.Data);
    }

    [Fact]
    public async Task GetContentById_ReturnsOkResult_WhenContentExists()
    {
        // Arrange
        var content = new Content { Id = 1, Title = "Sample Content" };
        _mockService.Setup(s => s.GetContentByIdAsync(1)).ReturnsAsync(content);

        // Act
        var result = await _controller.GetContentById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseModel<Content>>(okResult.Value);
        Assert.Equal(content, response.Data);
    }

    [Fact]
    public async Task GetContentById_ReturnsNotFound_WhenContentDoesNotExist()
    {
        // Arrange
        _mockService.Setup(s => s.GetContentByIdAsync(1)).ReturnsAsync((Content)null);

        // Act
        var result = await _controller.GetContentById(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var response = Assert.IsType<ResponseModel<Content>>(notFoundResult.Value);
        Assert.Null(response.Data);
    }

    [Fact]
    public async Task UpdateContent_ReturnsOkResult_WhenSuccessful()
    {
        // Arrange
        var content = new Content { Id = 1, Title = "Updated Content" };
        _mockService.Setup(s => s.UpdateContentAsync(1, content)).ReturnsAsync(true);

        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("User data")
        };

        _mockApiClient.Setup(c => c.GetAsync("users"))
                      .ReturnsAsync(httpResponse);

        // Act
        var result = await _controller.UpdateContent(1, content);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseModel<bool>>(okResult.Value);
        Assert.True(response.Data);
    }

    [Fact]
    public async Task UpdateContent_ReturnsOkResult_WhenApiClientFails()
    {
        // Arrange
        var content = new Content { Id = 1, Title = "Updated Content" };
        _mockService.Setup(s => s.UpdateContentAsync(1, content)).ReturnsAsync(true);

        var httpResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);

        _mockApiClient.Setup(c => c.GetAsync("users"))
                      .ReturnsAsync(httpResponse);

        // Act
        var result = await _controller.UpdateContent(1, content);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseModel<bool>>(okResult.Value);
        Assert.True(response.Data);
    }

    [Fact]
    public async Task DeleteContent_ReturnsOkResult_WhenSuccessful()
    {
        // Arrange
        _mockService.Setup(s => s.DeleteContentAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteContent(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseModel<bool>>(okResult.Value);
        Assert.True(response.Data);
    }

    [Fact]
    public async Task DeleteContent_ReturnsNotFound_WhenContentDoesNotExist()
    {
        // Arrange
        _mockService.Setup(s => s.DeleteContentAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteContent(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var response = Assert.IsType<ResponseModel<bool>>(notFoundResult.Value);
        Assert.False(response.Data);
    }
}
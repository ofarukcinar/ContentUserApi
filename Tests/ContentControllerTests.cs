using System.Net;
using ContentApi.Controllers;
using ContentApi.Helper;
using ContentApi.Models.ResponseModels;
using ContentApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

public class ContentControllerTests
{
    private readonly ApiClient _apiClient;
    private readonly ContentController _controller;
    private readonly Mock<IContentService> _mockContentService;

    public ContentControllerTests()
    {
        _mockContentService = new Mock<IContentService>();

        var mockResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("Mocked User Service Response")
        };

        var mockHttpHandler = new Helper(mockResponse);
        var mockHttpClient = new HttpClient(mockHttpHandler);

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(c => c["UserServiceBaseUrl"]).Returns("http://localhost:5000");

        _apiClient = new ApiClient(mockHttpClient, mockConfiguration.Object);

        _controller = new ContentController(_mockContentService.Object, _apiClient);
    }

    [Fact]
    public async Task GetAllContents_ShouldReturnOk_WithContents()
    {
        // Arrange
        var mockContents = new List<Content>
        {
            new() { Id = 1, Title = "Content 1", Body = "Body 1" },
            new() { Id = 2, Title = "Content 2", Body = "Body 2" }
        };
        _mockContentService.Setup(s => s.GetAllContentsAsync())
            .ReturnsAsync(mockContents);

        // Act
        var result = await _controller.GetAllContents();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseModel<IEnumerable<Content>>>(okResult.Value);
        Assert.Equal(2, response.Data.Count());
    }

    [Fact]
    public async Task CreateContent_ShouldReturnCreatedAtAction_WhenContentIsCreated()
    {
        // Arrange
        var newContent = new Content { Id = 3, Title = "New Content", Body = "New Body" };
        _mockContentService.Setup(s => s.CreateContentAsync(newContent))
            .ReturnsAsync(newContent);

        // Act
        var result = await _controller.CreateContent(newContent);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var response = Assert.IsType<ResponseModel<Content>>(createdResult.Value);
        Assert.Equal("Content created successfully.", response.Message);
    }

    [Fact]
    public async Task CreateContent_ShouldReturnBadRequest_WhenContentIsNotCreated()
    {
        // Arrange
        _mockContentService.Setup(s => s.CreateContentAsync(It.IsAny<Content>()))
            .ReturnsAsync((Content)null);

        // Act
        var result = await _controller.CreateContent(new Content());

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<ResponseModel<Content>>(badRequestResult.Value);
        Assert.Equal("Failed to create content.", response.Message);
    }

    [Fact]
    public async Task GetContentById_ShouldReturnOk_WhenContentExists()
    {
        // Arrange
        var content = new Content { Id = 1, Title = "Content 1", Body = "Body 1" };
        _mockContentService.Setup(s => s.GetContentByIdAsync(1))
            .ReturnsAsync(content);

        // Act
        var result = await _controller.GetContentById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseModel<Content>>(okResult.Value);
        Assert.Equal("Content 1", response.Data.Title);
    }

    [Fact]
    public async Task GetContentById_ShouldReturnNotFound_WhenContentDoesNotExist()
    {
        // Arrange
        _mockContentService.Setup(s => s.GetContentByIdAsync(1))
            .ReturnsAsync((Content)null);

        // Act
        var result = await _controller.GetContentById(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var response = Assert.IsType<ResponseModel<Content>>(notFoundResult.Value);
        Assert.Equal("Content not found.", response.Message);
    }

    [Fact]
    public async Task DeleteContent_ShouldReturnOk_WhenContentIsDeleted()
    {
        // Arrange
        _mockContentService.Setup(s => s.DeleteContentAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteContent(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseModel<bool>>(okResult.Value);
        Assert.True(response.Data);
    }

    [Fact]
    public async Task DeleteContent_ShouldReturnNotFound_WhenContentDoesNotExist()
    {
        // Arrange
        _mockContentService.Setup(s => s.DeleteContentAsync(1))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteContent(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var response = Assert.IsType<ResponseModel<bool>>(notFoundResult.Value);
        Assert.False(response.Data);
    }
}
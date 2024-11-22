using ContentApi.Helper;
using ContentApi.Models.RequestModel;
using ContentApi.Models.ResponseModels;
using ContentApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace ContentApi.Controllers;

[ApiController]
[Route("contents")]
[Produces(MediaTypeNames.Application.Json)]
public class ContentController : ControllerBase
{
    private readonly ApiClient _apiClient;
    private readonly IContentService _service;

    public ContentController(IContentService service, ApiClient apiClient)
    {
        _service = service;
        _apiClient = apiClient;
    }

    /// <summary>
    /// Retrieves all contents.
    /// </summary>
    /// <returns>List of all contents.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<IEnumerable<ContentResponseModel>>))]
    public async Task<IActionResult> GetAllContents()
    {
        var contents = await _service.GetAllContentsAsync();
        return Ok(new ResponseModel<IEnumerable<ContentResponseModel>>(contents));
    }

    /// <summary>
    /// Creates a new content.
    /// </summary>
    /// <param name="content">The content data to create.</param>
    /// <returns>The created content.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResponseModel<ContentResponseModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseModel<string>))]
    public async Task<IActionResult> CreateContent([FromBody] CreateContentRequestModel content)
    {
        var createdContent = await _service.CreateContentAsync(content);
        if (createdContent == null) 
            return BadRequest(new ResponseModel<Content>(null, "Failed to create content."));

        return CreatedAtAction(
            nameof(GetContentById),
            new { id = createdContent.Id },
            new ResponseModel<ContentResponseModel>(createdContent, "Content created successfully.")
        );
    }

    /// <summary>
    /// Retrieves a specific content by ID.
    /// </summary>
    /// <param name="id">The ID of the content.</param>
    /// <returns>The requested content.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<ContentResponseModel>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseModel<string>))]
    public async Task<IActionResult> GetContentById(int id)
    {
        var content = await _service.GetContentByIdAsync(id);
        if (content == null) 
            return NotFound(new ResponseModel<Content>(null, "Content not found."));
        
        return Ok(new ResponseModel<ContentResponseModel>(content));
    }

    /// <summary>
    /// Updates an existing content.
    /// </summary>
    /// <param name="id">The ID of the content to update.</param>
    /// <param name="content">The updated content data.</param>
    /// <returns>Success status of the update operation.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<bool>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseModel<string>))]
    public async Task<IActionResult> UpdateContent(int id, [FromBody] Content content)
    {
        var success = await _service.UpdateContentAsync(id, content);
        if (!success) 
            return NotFound(new ResponseModel<bool>(false, "Content not found or update failed."));

        var response = await _apiClient.GetAsync("users");
        if (!response.IsSuccessStatusCode)
            return Ok(new ResponseModel<bool>(true, "Content updated successfully. Failed to fetch user data."));

        return Ok(new ResponseModel<bool>(true, "Content updated successfully."));
    }

    /// <summary>
    /// Deletes a specific content by ID.
    /// </summary>
    /// <param name="id">The ID of the content to delete.</param>
    /// <returns>Success status of the delete operation.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<bool>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseModel<string>))]
    public async Task<IActionResult> DeleteContent(int id)
    {
        var success = await _service.DeleteContentAsync(id);
        if (!success) 
            return NotFound(new ResponseModel<bool>(false, "Content not found or delete failed."));
        
        return Ok(new ResponseModel<bool>(true, "Content deleted successfully."));
    }
}

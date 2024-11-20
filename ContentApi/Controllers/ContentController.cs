using ContentApi.Helper;
using ContentApi.Models.ResponseModels;
using ContentApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContentApi.Controllers;

[ApiController]
[Route("contents")]
public class ContentController : ControllerBase
{
    private readonly ApiClient _apiClient;
    private readonly IContentService _service;

    public ContentController(IContentService service, ApiClient apiClient)
    {
        _service = service;
        _apiClient = apiClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllContents()
    {
        var contents = await _service.GetAllContentsAsync();
        return Ok(new ResponseModel<IEnumerable<Content>>(contents));
    }

    [HttpPost]
    public async Task<IActionResult> CreateContent([FromBody] Content content)
    {
        var createdContent = await _service.CreateContentAsync(content);
        if (createdContent == null)
        {
            return BadRequest(new ResponseModel<Content>(null, "Failed to create content."));
        }
        return CreatedAtAction(
            nameof(GetContentById),
            new { id = createdContent.Id },
            new ResponseModel<Content>(createdContent, "Content created successfully.")
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetContentById(int id)
    {
        var content = await _service.GetContentByIdAsync(id);
        if (content == null)
        {
            return NotFound(new ResponseModel<Content>(null, "Content not found."));
        }
        return Ok(new ResponseModel<Content>(content));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContent(int id, [FromBody] Content content)
    {
        var success = await _service.UpdateContentAsync(id, content);
        if (!success)
        {
            return NotFound(new ResponseModel<bool>(false, "Content not found or update failed."));
        }

        var response = await _apiClient.GetAsync($"users");
        if (!response.IsSuccessStatusCode)
        {
            return Ok(new ResponseModel<bool>(true, "Content updated successfully. Failed to fetch user data."));
        }

        var userData = await response.Content.ReadAsStringAsync();

        return Ok(new ResponseModel<bool>(true, "Content updated successfully."));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContent(int id)
    {
        var success = await _service.DeleteContentAsync(id);
        if (!success)
        {
            return NotFound(new ResponseModel<bool>(false, "Content not found or delete failed."));
        }
        return Ok(new ResponseModel<bool>(true, "Content deleted successfully."));
    }
}
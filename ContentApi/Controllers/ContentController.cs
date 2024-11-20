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
        return Ok(contents);
    }

    [HttpPost]
    public async Task<IActionResult> CreateContent([FromBody] Content content)
    {
        var createdContent = await _service.CreateContentAsync(content);
        return CreatedAtAction(nameof(GetContentById), new { id = createdContent.Id }, createdContent);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetContentById(int id)
    {
        var content = await _service.GetContentByIdAsync(id);
        if (content == null) return NotFound();
        return Ok(content);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContent(int id, [FromBody] Content content)
    {
        var success = await _service.UpdateContentAsync(id, content);
        if (!success) return NotFound();

        var response = await _apiClient.GetAsync($"users");
        if (response.IsSuccessStatusCode)
        {
            var user = await response.Content.ReadAsStringAsync();
            // Process user data here if needed.
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContent(int id)
    {
        var success = await _service.DeleteContentAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}
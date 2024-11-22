using ContentApi.Models.RequestModel;
using ContentApi.Models.ResponseModels;
using ContentApi.Services;
using Mapster;
using Microsoft.EntityFrameworkCore;

public class ContentService : IContentService
{
    private readonly ContentDbContext _context;

    public ContentService(ContentDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ContentResponseModel>> GetAllContentsAsync()
    {
        var contents = await _context.Contents.ToListAsync();
        return contents.Adapt<IEnumerable<ContentResponseModel>>();
    }


    public async Task<ContentResponseModel> GetContentByIdAsync(int id)
    {
        var content = await _context.Contents.FindAsync(id);

        if (content == null)
            return null;

        return content.Adapt<ContentResponseModel>();
    }


    public async Task<ContentResponseModel> CreateContentAsync(CreateContentRequestModel createContentRequestModel)
    {
        var content = createContentRequestModel.Adapt<Content>();
        content.CreatedAt = DateTime.UtcNow;
        _context.Contents.Add(content);
        await _context.SaveChangesAsync();
        return content.Adapt<ContentResponseModel>();
    }

    public async Task<bool> UpdateContentAsync(int id, Content content)
    {
        var existingContent = await _context.Contents.FindAsync(id);
        if (existingContent == null) return false;

        existingContent.Title = content.Title;
        existingContent.Body = content.Body;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteContentAsync(int id)
    {
        var content = await _context.Contents.FindAsync(id);
        if (content == null) return false;

        _context.Contents.Remove(content);
        await _context.SaveChangesAsync();
        return true;
    }
}
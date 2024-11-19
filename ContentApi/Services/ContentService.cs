using ContentApi.Services;
using Microsoft.EntityFrameworkCore;

public class ContentService : IContentService
{
    private readonly ContentDbContext _context;

    public ContentService(ContentDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Content>> GetAllContentsAsync()
    {
        return await _context.Contents.ToListAsync();
    }

    public async Task<Content> GetContentByIdAsync(int id)
    {
        return await _context.Contents.FindAsync(id);
    }

    public async Task<Content> CreateContentAsync(Content content)
    {
        content.CreatedAt = DateTime.UtcNow;
        _context.Contents.Add(content);
        await _context.SaveChangesAsync();
        return content;
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
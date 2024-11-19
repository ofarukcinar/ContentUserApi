namespace ContentApi.Services;

public interface IContentService
{
    Task<IEnumerable<Content>> GetAllContentsAsync();
    Task<Content> GetContentByIdAsync(int id);
    Task<Content> CreateContentAsync(Content content);
    Task<bool> UpdateContentAsync(int id, Content content);
    Task<bool> DeleteContentAsync(int id);
}
using ContentApi.Models.RequestModel;
using ContentApi.Models.ResponseModels;

namespace ContentApi.Services;

public interface IContentService
{
    Task<IEnumerable<ContentResponseModel>> GetAllContentsAsync();
    Task<ContentResponseModel> GetContentByIdAsync(int id);
    Task<ContentResponseModel> CreateContentAsync(CreateContentRequestModel content);
    Task<bool> UpdateContentAsync(int id, Content content);
    Task<bool> DeleteContentAsync(int id);
}
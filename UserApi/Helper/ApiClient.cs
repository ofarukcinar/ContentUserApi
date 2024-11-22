namespace UserApi.Helper;

public class ApiClient
{
    private readonly string _baseUrl;
    private readonly HttpClient _httpClient;

    public ApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _baseUrl = configuration["ContentSeviceBaseUrl"];
    }

    public async Task<HttpResponseMessage> GetAsync(string endpoint)
    {
        var url = $"{_baseUrl}/{endpoint}";
        return await _httpClient.GetAsync(url);
    }

    // Diğer HTTP metodları için örnek:
    public async Task<HttpResponseMessage> PostAsync(string endpoint, HttpContent content)
    {
        var url = $"{_baseUrl}/{endpoint}";
        return await _httpClient.PostAsync(url, content);
    }

    public async Task<HttpResponseMessage> PutAsync(string endpoint, HttpContent content)
    {
        var url = $"{_baseUrl}/{endpoint}";
        return await _httpClient.PutAsync(url, content);
    }

    public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
    {
        var url = $"{_baseUrl}/{endpoint}";
        return await _httpClient.DeleteAsync(url);
    }
}
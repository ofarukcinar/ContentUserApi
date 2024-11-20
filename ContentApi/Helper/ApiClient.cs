using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ContentApi.Helper;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public ApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _baseUrl = configuration["UserSeviceBaseUrl"];
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
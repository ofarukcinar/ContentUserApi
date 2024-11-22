namespace ContentApi.Helper;

public class ApiClient
{
    private readonly string _baseUrl;
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _baseUrl = configuration["UserServiceBaseUrl"] ??
                   throw new ArgumentNullException("Base URL is not configured.");
    }

    public async Task<HttpResponseMessage> GetAsync(string endpoint)
    {
        if (string.IsNullOrWhiteSpace(endpoint))
            throw new ArgumentException("Endpoint cannot be null or empty.", nameof(endpoint));

        var url = BuildUrl(endpoint);
        return await _httpClient.GetAsync(url);
    }

    public async Task<HttpResponseMessage> PostAsync(string endpoint, HttpContent content)
    {
        if (string.IsNullOrWhiteSpace(endpoint))
            throw new ArgumentException("Endpoint cannot be null or empty.", nameof(endpoint));

        var url = BuildUrl(endpoint);
        return await _httpClient.PostAsync(url, content);
    }

    public async Task<HttpResponseMessage> PutAsync(string endpoint, HttpContent content)
    {
        if (string.IsNullOrWhiteSpace(endpoint))
            throw new ArgumentException("Endpoint cannot be null or empty.", nameof(endpoint));

        var url = BuildUrl(endpoint);
        return await _httpClient.PutAsync(url, content);
    }

    public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
    {
        if (string.IsNullOrWhiteSpace(endpoint))
            throw new ArgumentException("Endpoint cannot be null or empty.", nameof(endpoint));

        var url = BuildUrl(endpoint);
        return await _httpClient.DeleteAsync(url);
    }

    private string BuildUrl(string endpoint)
    {
        return $"{_baseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}";
    }
}
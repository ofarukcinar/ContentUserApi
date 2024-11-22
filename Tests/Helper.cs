using System.IdentityModel.Tokens.Jwt;

public class Helper : HttpMessageHandler
{
    private readonly HttpResponseMessage _mockResponse;

    public Helper(HttpResponseMessage response)
    {
        _mockResponse = response;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(_mockResponse);
    }
}

public static class JwtValidator
{
    public static string GetIssuer(string jwtToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwtToken);
        return token.Issuer;
    }
}
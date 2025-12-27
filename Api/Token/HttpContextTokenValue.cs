using Domain.Security.Tokens;

namespace CashFlowApi.Token;

public class HttpContextTokenValue : ITokenProvider
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public HttpContextTokenValue(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public string TokenOnRequest()
    {
        var authorization = httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();
        return authorization["Bearer ".Length..].Trim();
    }
}
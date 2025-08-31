using System.Globalization;

namespace CashFlowApi.Middleware;

public class CultureMiddleware
{
    private readonly RequestDelegate next;

    public CultureMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var supportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();
        var requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();
        
        var cultureInfo = new CultureInfo(
            string.IsNullOrWhiteSpace(requestedCulture) == false
            && supportedCultures.Exists(culture => culture.Name.Equals(requestedCulture))
                ? requestedCulture
                : "en-US");

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
        
        await next(context);
    }
}
using Microsoft.AspNetCore.Mvc.Filters;

namespace SessionApi.Filters;

public class AllowAllCorsFilterAttribute : Attribute, IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        context.HttpContext.Response.Headers.TryAdd("Access-Control-Allow-Origin", "*");
        await next();
    }
}

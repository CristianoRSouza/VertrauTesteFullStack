using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Api.Middleware;

public class SwaggerRedirectMiddleware
{
    private readonly RequestDelegate _next;

    public SwaggerRedirectMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/")
        {
            context.Response.Redirect("/swagger/index.html");
            return;
        }

        await _next(context);
    }
}

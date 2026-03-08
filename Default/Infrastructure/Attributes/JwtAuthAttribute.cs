using Default.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class JwtAuthAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Cookies.TryGetValue("jwt_token", out var token))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var jwtService = context.HttpContext.RequestServices.GetRequiredService<JwtService>();

        var principal = jwtService.ValidateToken(token);
        if (principal == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userIdClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (!await IsUserExist(userId, context.HttpContext.RequestServices, context.HttpContext.RequestAborted))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        context.HttpContext.Items["UserId"] = userId;

        await next();
    }

    private async Task<bool> IsUserExist(Guid userId, IServiceProvider services, CancellationToken cancellationToken)
    {
        var userService = services.GetRequiredService<UserService>();
        return await userService.IsUserExist(userId, cancellationToken);
    }
}
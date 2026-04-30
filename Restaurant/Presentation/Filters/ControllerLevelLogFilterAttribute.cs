using Microsoft.AspNetCore.Mvc.Filters;

namespace Restaurant.Presentation.Filters;

// Виконується перед і після кожного action методу контролера   
// підключається через атрибут
public class ControllerLevelLogFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ControllerLevelLogFilterAttribute>>();
        logger.LogInformation("ControllerLevelLogFilterAttribute before {Controller}.{Action}", context.RouteData.Values["controller"], context.RouteData.Values["action"]);
        base.OnActionExecuting(context);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ControllerLevelLogFilterAttribute>>();
        logger.LogInformation("ControllerLevelLogFilterAttribute afte {Controller}.{Action}", context.RouteData.Values["controller"], context.RouteData.Values["action"]);
        base.OnActionExecuted(context);
    }
}

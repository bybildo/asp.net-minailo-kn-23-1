using Microsoft.AspNetCore.Mvc.Filters;

namespace Restaurant.Presentation.Filters;

//Виконується перед і після кожного метода де вказаний атрибут
public class ActionLevelLogFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ActionLevelLogFilterAttribute>>();
        logger.LogInformation("ActionLevelLogFilterAttribute before {Controller}.{Action}", context.RouteData.Values["controller"], context.RouteData.Values["action"]);
        base.OnActionExecuting(context);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ActionLevelLogFilterAttribute>>();
        logger.LogInformation("ActionLevelLogFilterAttribute after {Controller}.{Action}", context.RouteData.Values["controller"], context.RouteData.Values["action"]);
        base.OnActionExecuted(context);
    }
}

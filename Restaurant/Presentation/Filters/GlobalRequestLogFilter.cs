using Microsoft.AspNetCore.Mvc.Filters;

namespace Restaurant.Presentation.Filters;

// Виконується перед і після кожним запитом до кожного контроллера
// підключається через конфігурацію в Program.cs
public class GlobalRequestLogFilter : IActionFilter
{
    private readonly ILogger<GlobalRequestLogFilter> _logger;

    public GlobalRequestLogFilter(ILogger<GlobalRequestLogFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("GlobalRequestLogFilter before {Controller}.{Action}", context.RouteData.Values["controller"], context.RouteData.Values["action"]);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("GlobalRequestLogFilter after {Controller}.{Action}", context.RouteData.Values["controller"], context.RouteData.Values["action"]);
    }
}

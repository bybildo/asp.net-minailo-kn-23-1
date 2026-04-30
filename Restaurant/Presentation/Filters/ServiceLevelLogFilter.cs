using Microsoft.AspNetCore.Mvc.Filters;

namespace Restaurant.Presentation.Filters;

// Виконується перед і після кожного action методу контролера   
// підключається через атрибут
// треба додати в di
public class ServiceLevelLogFilter : IActionFilter
{ 
    private readonly ILogger<ServiceLevelLogFilter> _logger;

    public ServiceLevelLogFilter(ILogger<ServiceLevelLogFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("ServiceLevelLogFilter before {Controller}.{Action}", context.RouteData.Values["controller"], context.RouteData.Values["action"]);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("ServiceLevelLogFilter after {Controller}.{Action}", context.RouteData.Values["controller"], context.RouteData.Values["action"]);
    }
}

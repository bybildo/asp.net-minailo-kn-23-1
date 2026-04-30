using Microsoft.AspNetCore.Mvc.Filters;

namespace Restaurant.Presentation.Filters;

// Виконується перед і після кожного action методу контролера   
// підключається через атрибут і передає параметри в конструктор
// треба додати в di
public class TypeLevelLogFilter : IActionFilter
{
    private readonly ILogger<TypeLevelLogFilter> _logger;
    private readonly string _label;

    public TypeLevelLogFilter(ILogger<TypeLevelLogFilter> logger, string label)
    {
        _logger = logger;
        _label = label;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("TypeLevelLogFilter({Label}) before {Controller}.{Action}", _label, context.RouteData.Values["controller"], context.RouteData.Values["action"]);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("TypeLevelLogFilter({Label}) after {Controller}.{Action}", _label, context.RouteData.Values["controller"], context.RouteData.Values["action"]);
    }
}


using Microsoft.AspNetCore.Diagnostics;

namespace myMinimalApiTemplate.Routers;

public class ErrorRouter (ILogger<ErrorRouter> logger ) : BaseRouter
{
    public override void AddRoutes(WebApplication app)
    {
        app.Map("/Error", (HttpContext context) => ProductionErrorHandler(context));
    }

    protected virtual IResult ProductionErrorHandler(HttpContext context) 
    {

        string msg = "Unknow Exception";

        var feaatures = context.Features.Get<IExceptionHandlerFeature>();

        if(feaatures != null) 
        {
            msg = feaatures.Error.Message;
        }

        logger.LogError(feaatures?.Error.ToString());

        return Results.Problem(msg);

    }
}

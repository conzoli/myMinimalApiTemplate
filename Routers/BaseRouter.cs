namespace myMinimalApiTemplate.Routers;

public abstract class BaseRouter {

    public string UrlFragment;
    public string TagName;

    public BaseRouter()
    {
        UrlFragment = string.Empty;
        TagName = string.Empty;
    }

    public abstract void AddRoutes(WebApplication app);

}
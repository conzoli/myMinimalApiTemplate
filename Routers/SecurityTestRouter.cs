
using myMinimalApiTemplate.SecurityLayer;

namespace myMinimalApiTemplate.Routers;

public class SecurityTestRouter : BaseRouter
{


    public SecurityTestRouter()
    {
        UrlFragment = "api/SecurityTest";
        TagName = "SecurityTest";
    }

    public override void AddRoutes(WebApplication app)
    {
        app.MapGet($"/{UrlFragment}/AuthenticateUser/{{name}}/password/{{password}}", 
                        (string name, string password) => AuthenticateUser(name,password))
            .WithTags(TagName)
            .Produces(200)
            .Produces<AppSecurityToken>()
            .Produces(400);
            
    }

    protected virtual IResult AuthenticateUser(string name, string password) 
    {

        IResult ret;
        AppSecurityToken asToken;

        asToken = new SecurityManager().AuthenticateUser(name,password);

        if(asToken.User.IsAuthenticated)
        {
            ret = Results.Ok(asToken);
        }
        else
        {
            ret = Results.BadRequest("Invalid User Name or Password");
        }


        return ret; 

    } 

}

using System.Text.Json.Serialization;

namespace myMinimalApiTemplate.SecurityLayer;

public class AppUser
{

    public AppUser()
    {
        
        userId = Guid.NewGuid();
        UserName = string.Empty;
        Password = string.Empty;
        IsAuthenticated = false;

    }

    public Guid userId {get;set;}

    public string UserName {get;set;}

    [JsonIgnore]
    public string Password {get; set;}

    public bool IsAuthenticated {get; set;}

}

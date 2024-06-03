using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace myMinimalApiTemplate.SecurityLayer;

public class SecurityManager
{

    public SecurityManager()
    {
        
    }


    #region Authenticate User

    public AppSecurityToken AuthenticateUser(string name, string password )
    {

        AppSecurityToken asToken;

        // Validate the user passed in
        // Create the AppSecurityToken object
        asToken = ValidateUser(name, password);

        if(asToken.User.IsAuthenticated)
        {
            LoadUserClaims(asToken);

            SetJwtToken(asToken);
        }

        return asToken;

    }

    #endregion


    #region ValidateUser Method

    protected AppSecurityToken ValidateUser(string name, string password)
    {

        AppSecurityToken asToken = new();

        // Validate User - HARD CODED FOR NOW
        // TODO: Authenticate against a data store

        switch(name.ToLower())
        {
            case "pauls":
                if(password == "password")
                {
                    asToken.User.UserName = name;
                    asToken.User.userId = new Guid("f948829c-85cb-4af1-96dc-159cef7ba575");
                    asToken.User.IsAuthenticated = true;
                }
            break;

            case "johnk":
                if(password == "password")
                {
                    asToken.User.UserName = name;
                    asToken.User.userId = new Guid("6abc36a0-3e2c-4dd6-ac2f-103b28b99c02");
                    asToken.User.IsAuthenticated = true;
                }
            break;
        }

        return asToken;

    }

    #endregion


    #region LoadUserClaims

    protected void LoadUserClaims(AppSecurityToken asToken)
     {

        // Get Claims for a user - HARD CODE for now
        // TODO: Get Claims from Data Store

        switch(asToken.User.UserName.ToLower()) 
        {
            case "pauls":
                asToken.Claims.Add( new AppUserClaim(){
                    UserId = asToken.User.userId,
                    ClaimType = "GetNames",
                    ClaimValue = "true"
                } );

                asToken.Claims.Add( new AppUserClaim(){
                    UserId = asToken.User.userId,
                    ClaimType = "GetAName",
                    ClaimValue = "true"
                } );

                asToken.Claims.Add( new AppUserClaim(){
                    UserId = asToken.User.userId,
                    ClaimType = "Search",
                    ClaimValue = "true"
                } );

            break;

            case "johnk":
                 asToken.Claims.Add( new AppUserClaim(){
                    UserId = asToken.User.userId,
                    ClaimType = "GetName",
                    ClaimValue = "true"
                } );
            break;
        }

     }

    #endregion

    #region SetJwtToken

    protected void SetJwtToken(AppSecurityToken asToken)
    {

        List<Claim> claims = BuildJWTClaims(asToken);

        SecurityTokenDescriptor tokenDescriptor = new ()
        {

            Expires = DateTime.UtcNow.AddMinutes(60 * 24 * 5),
            Issuer = "http://localhost:5801",
            Audience = "myMinimalApiTemplate",
            SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(Encoding.ASCII.GetBytes("Supder!Dupper#Key123@1234567891011Supder!Dupper#Key123@1234567891011Supder!Dupper#Key123@1234567891011Supder!Dupper#Key123@1234567891011Supder!Dupper#Key123@1234567891011Supder!Dupper#Key123@1234567891011")), 
                    SecurityAlgorithms.HmacSha512Signature),
            //add claims
            Subject = new ClaimsIdentity(claims)

        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var bearerToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

        // create a string representation of JWT Token
        asToken.BearerToken = bearerToken;

    }
        
    #endregion

    #region BuildJWTClaims

    protected List<Claim> BuildJWTClaims(AppSecurityToken asToken)
    {

        List<Claim> ret = new (){

            new Claim(JwtRegisteredClaimNames.Sub, asToken.User.UserName),

            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

            new Claim("IsAuthenticated", asToken.User.IsAuthenticated.ToString())

        };

        foreach( var item in asToken.Claims)
        {
            ret.Add(new Claim(item.ClaimType, item.ClaimValue));
        }

        return ret;

    }

    #endregion

}

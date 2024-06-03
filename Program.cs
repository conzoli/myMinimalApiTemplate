using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic;
using myMinimalApiTemplate.Data;
using myMinimalApiTemplate.Data.Sqlite;
using myMinimalApiTemplate.Models;
using myMinimalApiTemplate.Routers;
using myMinimalApiTemplate.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement{ {
        new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
            }
        },
        new List<string>()
    }
    });
});

// register Services:
// register routers
builder.Services.AddScoped<BaseRouter, NameRouter>();
builder.Services.AddScoped<BaseRouter, SecurityTestRouter>();

//register other Services:
builder.Services.AddTransient<IHelloService, HelloService>();
builder.Services.AddSingleton<INameRepository, SqliteNameRepository>();


// Configure Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("myMinimalApiTemplateCorsPolicy",
         builder =>
         {
             builder.AllowAnyOrigin();
             //builder.WithOrigins("http://localhost:5801","http://www.example.com");
         });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(jwtOptions =>
{
    jwtOptions.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidIssuer = "http://localhost:5801",
            ValidAudience = "myMinimalApiTemplate",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Supder!Dupper#Key123@1234567891011Supder!Dupper#Key123@1234567891011Supder!Dupper#Key123@1234567891011Supder!Dupper#Key123@1234567891011Supder!Dupper#Key123@1234567891011Supder!Dupper#Key123@1234567891011")),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.FromMinutes(60 * 24 * 5)  // 5 Tage
        };
});

builder.Services.AddAuthorization( options => 
{

    options.AddPolicy("GetNamesClaim", policy => policy.RequireClaim("GetNames"));

} );



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Enable Cors
app.UseCors("myMinimalApiTemplateCorsPolicy");

app.UseAuthentication();
app.UseAuthorization();


using (var scope = app.Services.CreateScope())
{

    // Rrouters

    var services = scope.ServiceProvider.GetServices<BaseRouter>();

    foreach (var service in services)
    {
        service.AddRoutes(app);
    }

    var DBManager = new SqLiteDBManager();

    DBManager.DataBaseUp();


    var NameRepo = new SqliteNameRepository();

    var NamesList = NameRepo.GetNames().ToList();

    NamesList.ForEach(name => { Console.Write($"UUID: {name.Uuid}\nVorname: {name.FirstName}\nNachname: {name.LastName}\n\n"); });


    var nameByUUID = NameRepo.GetNameByUuid(new Guid("c9fa8e24b5c14395a046bf7377bbdd88"));

    Console.WriteLine($"By UUID: Vorname: {nameByUUID}");


    // Simple Routes

    var HelloService = scope.ServiceProvider.GetService<IHelloService>();

    app.MapGet("/", () => HelloService != null ? HelloService.GetHelloMsg() : "🚨Error🚨");

    app.MapGet("/errorTest", () =>
    {

        return Results.NotFound("Hey, Not Fount Test");

    }).Produces(404).WithTags("SimpleTest");

    app.MapGet("/paramTest/{id}", (int id) =>
    {
        return Results.Ok("Param war: " + id);

    }).Produces(200).WithTags("SimpleTest");

    app.Run();

}



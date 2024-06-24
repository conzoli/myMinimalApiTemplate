
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using myMinimalApiTemplate.Data;
using myMinimalApiTemplate.Models;
using myMinimalApiTemplate.Models.DTO;
using myMinimalApiTemplate.Models.Mapper;

namespace myMinimalApiTemplate.Routers;

public class NameRouter : BaseRouter
{
    private readonly INameRepository _nameRepo;

    public NameRouter(INameRepository nameRepo)
    {
        UrlFragment = "api/Names";
        TagName = "Names";
        _nameRepo = nameRepo;
    }

    public override void AddRoutes(WebApplication app)
    {
        app.MapGet($"/{UrlFragment}", async () => await GetNamesAsync())
            .WithTags(TagName)
            .Produces(200)
            .Produces<List<NameDTO>>()
            .Produces(404)
            .WithName("GetNames")
            .RequireAuthorization("GetNamesOrAdminClaim");


        app.MapGet($"/{UrlFragment}/{{uuid}}", (string uuid) => GetNames(uuid))
             .WithTags(TagName)
             .Produces(200)
             .WithName("GetName");


        app.MapPost($"/{UrlFragment}", ([FromBody] NameDTO name) => PostNames(name))
           .WithTags(TagName)
           .Produces(200)
           .Produces(400)
           .WithName("CreateName")
           .RequireAuthorization("CreateName");

    }


    protected virtual async Task<IResult> GetNamesAsync(){

        List<NameDTO> nameDtoList = [];

        var names = await _nameRepo.GetNamesAsync();

        names.ToList().ForEach( n => {  nameDtoList.Add(n.ToNameDTO()); } );

        return Results.Ok(nameDtoList);

    }


    protected virtual IResult GetNames()
    {

        List<NameDTO> nameDtoList = [];

        //var n1 = new Name { Id = 1, FirstName = "Humer", LastName = "Simpson", uuid = Guid.NewGuid() }.ToNameDTO();
        //var n2 = new Name { Id = 2, FirstName = "Bart", LastName = "Simpson", uuid = Guid.NewGuid() }.ToNameDTO();

        var names = _nameRepo.GetNames().ToList();

        names.ForEach(n => { nameDtoList.Add(n.ToNameDTO()); });

        return Results.Ok(nameDtoList);

    }

    protected virtual IResult GetNames(string uuid)
    {

        var Name = _nameRepo.GetNameByUuid(new Guid(uuid));

        if (Name == null || Name.Id < 0)
        {
            return Results.BadRequest("Name not found");
        }

        return Results.Ok(Name.ToNameDTO());

    }

    protected virtual IResult PostNames(NameDTO nameDto)
    {

        var newUuid = Guid.NewGuid();


        nameDto.Uuid = newUuid.ToString("N");

        var name = nameDto.ToName();

        var result = _nameRepo.CreateName(name);


        //return Results.Created($"/{UrlFragment}/{nameDto.Uuid}",nameDto);

        return Results.CreatedAtRoute("GetName", new { uuid = nameDto.Uuid }, nameDto);

    }

}
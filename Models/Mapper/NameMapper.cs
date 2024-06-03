using myMinimalApiTemplate.Models.DTO;

namespace myMinimalApiTemplate.Models.Mapper;

public static class NameMapper
{

    public static Name ToName(this NameDTO nameDTO)
    {

        var result = new Name()
        {
            FirstName = nameDTO.FirstName,
            LastName = nameDTO.LastName,
            uuid = new Guid(nameDTO.Uuid)
        };

        return result;

    }


    public static NameDTO ToNameDTO(this Name name){

        var result = new NameDTO(){
            FirstName = name.FirstName,
            LastName = name.LastName,
            Uuid = name.Uuid
        };

        return result;

    }


}
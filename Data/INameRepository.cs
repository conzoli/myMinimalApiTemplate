using myMinimalApiTemplate.Models;

namespace myMinimalApiTemplate.Data;


public interface INameRepository {


    Name GetName(int Id);
    Task<Name> GetNameAsync(int Id);

    Name GetNameByUuid(Guid uuid);
    Task<Name> GetNameByUuidAsync(Guid uuid);

    IEnumerable<Name> GetNames();
    Task<IEnumerable<Name>> GetNamesAsync();

    int CreateName(Name name);
    Task<int> CreateNameAsync(Name name);

    bool UpdateName(Name name);
    Task<bool> UpdateNameAsync(Name name);

    bool DeleteName(Name name);
    Task<bool> DeleteNameAsync(Name name);


}
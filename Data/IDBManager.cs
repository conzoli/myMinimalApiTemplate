namespace myMinimalApiTemplate.Data;


public interface IDBManager {

    bool DataBaseUp();

    Task<bool> DataBaseUpAsync();

}
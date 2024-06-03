namespace myMinimalApiTemplate.Data.Sqlite;

using System.Threading.Tasks;
using Dapper;

public class SqLiteDBManager : SqLiteBaseRepository, IDBManager
{
    public bool DataBaseUp()
    {
        
        var RetVal = false;

        if (File.Exists(DbFile)) return true;

        RetVal = CreateTableNamen();

        return RetVal;

    }

    public async Task<bool> DataBaseUpAsync()
    {
        var RetVal = false;

        if (File.Exists(DbFile)) return true;

        RetVal = await CreateTableNamenAsync();

        return RetVal;
    }





    private bool CreateTableNamen(){

        var retVal = false;

        var sql  = @"CREATE Table Names (
                                            Id INTEGER PRIMARY KEY,
                                            Uuid TEXT,
                                            FirstName TEXT,
                                            LastName TEXT
                                                    
                                        );";

        using ( var conn = DataConnection() ) {

            var result = conn.Execute(sql);

            if(result > 0) {
                retVal = true;
            }

        }

        return retVal;

    } 

    private async Task<bool> CreateTableNamenAsync(){

        var retVal = false;

        var sql  = @"CREATE Table Namen (
                                            Id INTEGER PRIMARY KEY,
                                            Uuid TEXT,
                                            FirstName TEXT,
                                            LastName TEXT,
                                                    
                                        );";

        using ( var conn = DataConnection() ) {

            var result = await conn.ExecuteAsync(sql);

            if(result > 0) {
                retVal = true;
            }

        }

        return retVal;

    } 

}
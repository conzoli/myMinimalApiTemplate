namespace myMinimalApiTemplate.Data.Sqlite;
using Dapper;

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using myMinimalApiTemplate.Models;
using System.Threading.Tasks;

public class SqliteNameRepository : SqLiteBaseRepository, INameRepository
{


    public bool DeleteName(Name name)
    {
        var retVal = false;

        var sql = "DELETE FROM namen WHERE [Id]=@Id";

        using (var conn = DataConnection())
        {

            var result = conn.Execute(sql, name);

            if (result > 0)
            {
                retVal = true;
            }

        }

        return retVal;
    }

    public Name GetName(int Id)
    {

        var sql = "SELECT * FROM Namen WHERE [Id]=@Id";

        using var conn = DataConnection();

        var Name = conn.Query<Name>(sql, new { Id }).FirstOrDefault();

        if (Name != null)
        {
            return Name;
        }
        else
        {
            return new Name() { Id = -1 };
        }

    }

    public async Task<Name> GetNameAsync(int Id)
    {
        var sql = "SELECT * FROM Namen WHERE Id=@Id";

        using var conn = DataConnection();

        var Name = await conn.QueryFirstOrDefaultAsync<Name>(sql, new { Id });

        if (Name != null)
        {
            return Name;
        }
        else
        {
            return new Name() { Id = -1 };
        }
    }

    public Name GetNameByUuid(Guid uuid)
    {

        var sql = "SELECT * FROM Names WHERE [Uuid]=@Uuid";

        var Uuid = uuid.ToString("N");

        using var conn = DataConnection();

        var Name = conn.Query<Name>(sql, new { Uuid }).FirstOrDefault();

        if (Name != null)
        {
            return Name;
        }
        else
        {
            return new Name() { Id = -1 };
        }

    }

    public IEnumerable<Name> GetNames()
    {
        var Names = new List<Name>();

        var sql = "SELECT * FROM Names";

        using var conn = DataConnection();

        Names = conn.Query<Name>(sql).ToList();

        return Names;


    }

    public int CreateName(Name name)
    {
        var sql = @"INSERT INTO Names (
                                   [Uuid],
                                   [FirstName], 
                                   [LastName] )
                            VALUES (
                                @Uuid,
                                @FirstName,
                                @LastName
                            )";

        var retVal = -1;

        using var conn = DataConnection();

        var result = conn.Execute(sql, name);

        if(result > 0) {
            retVal = 1;
        }

        return retVal;


    }

    public bool UpdateName(Name name)
    {

        var RetVal = false;
        
        var sql = @"UPDATE Names 
                        SET [Uuid] = @Uuid,
                            [FirstName] = @FirstName, 
                            [LastName] = @LastName 
                        WHERE [Id] = @Id";

        using var conn = DataConnection();

        var result = conn.Execute(sql, name);

        if(result > 0) 
        {
            RetVal = true;
        }

        return RetVal;

    }

    public async Task<Name> GetNameByUuidAsync(Guid uuid)
    {
        var sql = "SELECT * FROM Names WHERE [Uuid]=@Uuid";

        var Uuid = uuid.ToString("N");

        using var conn = DataConnection();

        var Name = await conn.QueryFirstAsync<Name>(sql, new { Uuid });

        if (Name != null)
        {
            return Name;
        }
        else
        {
            return new Name() { Id = -1 };
        }

    }

    public async Task<IEnumerable<Name>> GetNamesAsync()
    {
        var Names = new List<Name>();

        var sql = "SELECT * FROM Names";

        using var conn = DataConnection();

        var results = await conn.QueryAsync<Name>(sql);

        if (results != null)
        {
            Names = results.ToList();
        }


        return Names;

    }

    public async Task<int> CreateNameAsync(Name name)
    {
        var sql = @"INSERT INTO Names (
                                   [Uuid],
                                   [FirstName], 
                                   [LastName] )
                            VALUES (
                                @Uuid,
                                @FirstName,
                                @LastName
                            );
        )";

        var retVal = -1;

        using var conn = DataConnection();

        var result = await conn.ExecuteAsync(sql, name);

        if(result > 0) {
            retVal = 1;
        }

        return retVal;
    }

    public async Task<bool> UpdateNameAsync(Name name)
    {
        var RetVal = false;
        
        var sql = @"UPDATE Names 
                        SET [Uuid] = @Uuid,
                            [FirstName] = @FirstName, 
                            [LastName] = @LastName 
                        WHERE [Id] = @Id";

        using var conn = DataConnection();

        var result = await conn.ExecuteAsync(sql, name);

        if(result > 0) 
        {
            RetVal = true;
        }

        return RetVal;
    }

    public async Task<bool> DeleteNameAsync(Name name)
    {
        var retVal = false;

        if (!File.Exists(DbFile)) return retVal;

        var sql = "DELETE FROM Names WHERE [Id]=@Id";

        using (var conn = DataConnection())
        {

            var result = await conn.ExecuteAsync(sql, name);

            if (result > 0)
            {
                retVal = true;
            }

        }

        return retVal;
    }
}
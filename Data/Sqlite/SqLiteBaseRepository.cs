namespace myMinimalApiTemplate.Data.Sqlite;
using System.Data.SQLite;

public class SqLiteBaseRepository
{

    public static string DbFile
    {
        get
        {
            return Environment.CurrentDirectory + "/data.db";
        }
    }

    public static SQLiteConnection DataConnection()
    {
        return new SQLiteConnection("Data Source=" + DbFile);
    }

}
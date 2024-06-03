namespace myMinimalApiTemplate.Models;

public class Name
{

    public int Id { get; set; } = 0;
    public Guid uuid { get; set; } = new Guid();

    public string Uuid {
        get{
            return uuid.ToString("N");
        }
        set{
            uuid = new Guid(value);
        }
    }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }

}
namespace myMinimalApiTemplate.Services;

public class HelloService : IHelloService
{

    private const string msg = "👋Hello World!👋";

    public string GetHelloMsg()
    {
        return msg;
    }
}

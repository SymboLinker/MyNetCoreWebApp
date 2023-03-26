using FunDb;
using Microsoft.Extensions.DependencyInjection;
using TestHelpers;

namespace MyNetCoreWebApp.Tests;
public class TestBase : IDisposable
{
    protected IGet i { get; }
    protected IFunDb db { get; }
    protected TestServices Services { get; }

    public TestBase()
    {
        Services = new TestServices();
        Services.AddIGet();
        Services.AddFunDb();
        i = Services.GetRequiredService<IGet>();
        db = Services.GetRequiredService<IFunDb>();
    }

    public void Dispose()
    {
    }
}

## MyNetCoreWebApp - Workshops example

This repo is just a test of a few things.

The following class is used as a base class for unit tests:

```csharp
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
```
Notes:
- `FunDb` is not a NuGet package - it is part of this repository.
- [IGet](https://github.com/dotnet-iget/iget#readme) is a [NuGet package](https://www.nuget.org/packages/iget).

This allows the unit tests to look like this, with a very small arrange step:
```csharp
[Theory]
[InlineData("", false)]
[InlineData("A", false)]
[InlineData("A ", false)]
[InlineData(" A", false)]
[InlineData("Ab", true)]
public async Task Validates_name_input_length(string nameInput, bool shouldSucceed)
{
    // Arrange
    var workshopId = await db.InsertAsync(new Workshop
    {
        Name = "Programming for beginners",
        DateTimeStart = DateTime.Now.AddHours(1),
    });

    // Act
    var request = new SubscribeRequest(nameInput, workshopId);
    var result = await i.Get<SubscribeRequestHandler>().HandleAsync(request);

    // Assert
    if (shouldSucceed)
    {
        Assert.True(result.IsSuccess);
    }
    else
    {
        Assert.True(result.IsFail);
        Assert.Equal("You should specify a name of at least 2 characters.", result.ErrorMessage);
    }
}
```
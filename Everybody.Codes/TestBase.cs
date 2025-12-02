using Xunit.Abstractions;

namespace Everybody.Codes;

public class TestBase
{
    protected readonly ITestOutputHelper output;

    public TestBase(ITestOutputHelper testOutputHelper)
    {
        output = testOutputHelper;
    }
}

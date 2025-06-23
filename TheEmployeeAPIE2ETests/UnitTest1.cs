using Microsoft.Playwright;

namespace TheEmployeeAPIE2ETests;

public class UnitTest1
{
    [Fact]
    public async void Test1()
    {
        var playwright = await Playwright.CreateAsync();
    }
}

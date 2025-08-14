using Lab10.Abstraction;
using Lab10.Utilities.Services;
using Xunit;

namespace TestLab10.Tests;

public class TextFormatterServiceTests
{
    private readonly ITextFormatter text = new TextFormatterService();

    [Fact]
    public async Task Pascal()
    {
        var r = await text.ToPascalCaseAsync("hello world");
        Assert.Equal("HelloWorld", r);
    }

    [Fact]
    public async Task Camel()
    {
        var r = await text.ToCamelCaseAsync("HelloWorld");
        Assert.Equal("helloWorld", r);
    }

    [Fact]
    public async Task Snake()
    {
        var r = await text.ToSnakeCaseAsync("HelloWorld");
        Assert.Equal("hello_world", r);
    }

    [Fact]
    public async Task Kebab()
    {
        Assert.Equal(string.Empty, await text.ToKebabCaseAsync(""));
        Assert.Equal(string.Empty, await text.ToKebabCaseAsync(null));
    }
}

using Lab10.Abstractions;
using Lab10.Utilities.Services;
using Xunit;

namespace Lab10.Tests;

public class TextStatisticsServiceTests
{
    private readonly ITextStatistics text = new TextStatisticsService();

    [Fact]
    public async Task CountWords()
    {
        var n = await text.CountWordsAsync("Hello, world! Hello.");
        Assert.Equal(3, n);
    }

    [Fact]
    public async Task CountChars() //excluding white space
    {
        var n = await text.CountCharactersAsync("A B\tC\n");
        Assert.Equal(3, n);
    }

    [Fact]
    public async Task Commonleast()
    {
        var most = await text.MostCommonWordAsync("one ONE two");
        var least = await text.LeastCommonWordAsync("one ONE two");
        Assert.Equal("one", most);
        Assert.Equal("two", least);
    }

    [Fact]
    public async Task Cancellation_Throws()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        await Assert.ThrowsAsync<OperationCanceledException>(() => text.CountWordsAsync("hi", cts.Token));
    }
}
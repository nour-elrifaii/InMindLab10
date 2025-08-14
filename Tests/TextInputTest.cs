using System.Net;
using Lab10.Abstraction;
using Lab10.Utilities.Services;
using Xunit;

namespace Lab10.Tests;

public class TextInputServiceTests
{
    private sealed class FakeHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _http;
        public FakeHandler(Func<HttpRequestMessage, HttpResponseMessage> http) => _http = http;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Task.FromResult(_http(request));
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }
    }

    [Fact]
    public async Task LoadFromUrl()
    {
        var handler = new FakeHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("hello")
        });

        var client = new HttpClient(handler) { BaseAddress = new Uri("https://inmind.org/") };
        ITextInput textIn = new TextInputService(client);

        var text = await textIn.LoadFromUrlAsync("api/data");
        Assert.Equal("hello from web", text);
    }

    [Fact]
    public async Task LoadFromFile()
    {
        var path = Path.GetTempFileName();
        await File.WriteAllTextAsync(path, "local file");
        try
        {
            ITextInput textIn = new TextInputService(new HttpClient(new FakeHandler(_ => new HttpResponseMessage(HttpStatusCode.OK))));
            var text = await textIn.LoadFromFileAsync(path);
            Assert.Equal("local file", text);
        }
        finally { File.Delete(path); }
    }
}


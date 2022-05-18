/*
 * Reference: 
 *  https://csharpindepth.com/Articles/Singleton
 * 
 */

namespace Nautilus.SolutionExplorer.Core.Components.Http;

public class HttpClientManager
{
    private static readonly Lazy<HttpClientManager> _lazyInstance = new Lazy<HttpClientManager>(() => new HttpClientManager());
    private readonly HttpClient _httpClient;

    private HttpClientManager()
    {
        _httpClient = new HttpClient();
    }

    public static HttpClientManager Instance
    {
        get { return _lazyInstance.Value; }
    }

    public async Task<string> GetAsync(string url, bool ensureSuccessStatusCode = false, CancellationToken cancellationToken = default)
    {
        var responseMessage = await _httpClient.GetAsync(url, cancellationToken);
        if (ensureSuccessStatusCode)
        {
            // NOTE: if this fail, it will return HttpRequestException
            responseMessage = responseMessage.EnsureSuccessStatusCode();
        }

        var stringContent = await responseMessage.Content.ReadAsStringAsync();
        return stringContent;
    }
}
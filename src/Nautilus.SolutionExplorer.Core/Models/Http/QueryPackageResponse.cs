namespace Nautilus.SolutionExplorer.Core.Models.Http;

public class QueryPackageResponse : BaseHttpResponse
{
    public QueryPackageResponse()
    {

    }

    public QueryPackageResponse(Exception ex) : base(ex)
    {

    }

    [JsonProperty("@context")]
    public Context Context { get; set; }

    [JsonProperty("totalHits")]
    public ulong TotalHits { get; set; }

    [JsonProperty("lastReopen")]
    public DateTime LastReopen { get; set; }

    [JsonProperty("index")]
    public string Index { get; set; }

    [JsonProperty("data")]
    public IList<Datum> Data { get; set; }
}

public class Context
{
    [JsonProperty("@vocab")]
    public string Vocab { get; set; }

    [JsonProperty("@base")]
    public string Base { get; set; }
}

public class VersionInfo
{
    [JsonProperty("version")]
    public string Version { get; set; }

    [JsonProperty("downloads")]
    public ulong Downloads { get; set; }

    [JsonProperty("@id")]
    public string Id { get; set; }
}

public class Datum
{
    [JsonProperty("@id")]
    public string IdAlias { get; set; }

    [JsonProperty("@type")]
    public string Type { get; set; }

    [JsonProperty("registration")]
    public string Registration { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("version")]
    public string Version { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("summary")]
    public string Summary { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("iconUrl")]
    public string IconUrl { get; set; }

    [JsonProperty("licenseUrl")]
    public string LicenseUrl { get; set; }

    [JsonProperty("projectUrl")]
    public string ProjectUrl { get; set; }

    [JsonProperty("tags")]
    public IList<string> Tags { get; set; }

    [JsonProperty("authors")]
    public IList<string> Authors { get; set; }

    [JsonProperty("totalDownloads")]
    public ulong TotalDownloads { get; set; }

    [JsonProperty("verified")]
    public bool Verified { get; set; }

    [JsonProperty("versions")]
    public IList<VersionInfo> Versions { get; set; }
}

namespace Nautilus.SolutionExplorer.Test;

public class HttpClientTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    [TestCase("Xamarin.Forms", false)]
    public async Task Query_Package_Expected_Successful(string packageName, bool preRelease)
    {
        //
        // Arrange
        var request = NugetPackageHttpRequest.QueryRequest(packageName, preRelease);

        // 
        // Act
        var response = await request.ExecuteAsync();
        var foundVersion = response.GetCurrentVersion(packageName);

        //
        // Assert
        Assert.True(foundVersion.StartsWith("3.", StringComparison.Ordinal) ||
            foundVersion.StartsWith("4.", StringComparison.Ordinal));
    }
}

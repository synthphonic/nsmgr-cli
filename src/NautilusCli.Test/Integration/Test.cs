using System.Linq;
using System.Threading.Tasks;
using Nautilus.Cli.Core.Components.Http;
using Nautilus.Cli.Core.Extensions;
using NUnit.Framework;

namespace Tests
{
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
			Assert.True(foundVersion.StartsWith("3.", System.StringComparison.Ordinal) || 
				foundVersion.StartsWith("4.", System.StringComparison.Ordinal));
		}
	}
}
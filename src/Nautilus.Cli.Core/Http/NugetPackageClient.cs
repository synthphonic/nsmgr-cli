using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Nautilus.Cli.Core.Http
{
	public class NugetPackageClient
	{
		public QueryRequest QueryRequest(string packageName, bool preRelease)
		{
			return new QueryRequest(packageName, preRelease);
		}
	}

	public class QueryRequest
	{
		private readonly string _packageName;
		private readonly bool _preRelease;
		private string _apiTemplate = @"https://api-v2v3search-0.nuget.org/query?q={packageName}&prerelease={preRelease}";

		public QueryRequest(string packageName, bool preRelease)
		{
			_packageName = packageName;
			_preRelease = preRelease;

			_apiTemplate = _apiTemplate.Replace("{packageName}", _packageName);
			_apiTemplate = _apiTemplate.Replace("{prerelease}", _preRelease.ToString());
		}

		public async Task ExecuteAsync(CancellationToken cancellationToken = default)
		{
			var httpClient = new HttpClient();
			var requestMessage = new HttpRequestMessage(HttpMethod.Get, _apiTemplate);

			HttpResponseMessage response = null;

			try
			{
				response = await httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseContentRead, cancellationToken);
			}
			catch (ArgumentNullException argNullEx)
			{
				Console.WriteLine(argNullEx.Message);
			}
			catch (InvalidOperationException invalidOpEx)
			{
				Console.WriteLine(invalidOpEx.Message);
			}
			catch (HttpRequestException httpREx)
			{
				Console.WriteLine(httpREx.Message);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
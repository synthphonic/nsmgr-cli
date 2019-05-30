using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Nautilus.Cli.Core.Models.Http;
using Nautilus.Net.Http;
using Newtonsoft.Json;

namespace Nautilus.Cli.Core.Components.Http
{
	public class NugetPackageQuery
	{
		private readonly string _packageName;
		private readonly bool _preRelease;
		private string _apiTemplate = @"https://api-v2v3search-0.nuget.org/query?q={packageName}&prerelease={preRelease}";

		internal NugetPackageQuery(string packageName, bool preRelease)
		{
			_packageName = packageName;
			_preRelease = preRelease;

			_apiTemplate = _apiTemplate.Replace("{packageName}", _packageName);
			_apiTemplate = _apiTemplate.Replace("{preRelease}", _preRelease.ToString().ToLower());
		}

		public async Task<QueryPackageResponse> ExecuteAsync(CancellationToken cancellationToken = default)
		{
			try
			{
				var httpClient = new HttpClientManager();
				var response = await httpClient.GetAsync(_apiTemplate, false, cancellationToken);

				var deserialized = JsonConvert.DeserializeObject<QueryPackageResponse>(response);

				return deserialized;
			}
			catch (ArgumentNullException argNullEx)
			{
				return new QueryPackageResponse(argNullEx);
			}
			catch (InvalidOperationException invalidOpEx)
			{
				return new QueryPackageResponse(invalidOpEx);
			}
			catch (HttpRequestException httpReqEx)
			{
				return new QueryPackageResponse(httpReqEx);
			}
			catch (Exception ex)
			{
				return new QueryPackageResponse(ex);
			}
		}
	}
}
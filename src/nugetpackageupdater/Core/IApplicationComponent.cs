
namespace SolutionNugetPackagesUpdater.Core
{
	public interface IApplicationComponent
	{
		void Initialize(params object[] paramteters);
		object Execute();
	}
}
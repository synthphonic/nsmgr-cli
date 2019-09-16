using CommandLine;

namespace Nautilus.Cli.Core.Abstraction
{
	public abstract class VerbBase
	{
		[Option("debug", Default = false, Required = false, Hidden = true, HelpText = "Show debugging message including exception message and stacktrace")]
		public virtual bool Debug { get; set; }
	}
}

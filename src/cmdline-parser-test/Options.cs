using CommandLine;

namespace cmdline_parser_test
{
	[Verb("add", HelpText = "Add file contents to index.")]
	class AddOptions
	{
		[Option('t', Required =true, HelpText = "The t option")]
		public bool Truncate { get; set; }
	}

	[Verb("commit", HelpText = "Record changes to the repository.")]
	class CommitOptions
	{

	}

	[Verb("clone", HelpText = "Clone a repository into a new directory.")]
	class CloneOptions
	{

	}
}

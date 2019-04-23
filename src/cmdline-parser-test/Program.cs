using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace cmdline_parser_test
{
	class Program
	{
		static void Main(string[] args)
		{
			Parser.Default.ParseArguments<AddOptions, CommitOptions, CloneOptions>(args)
				.WithParsed<AddOptions>((obj) =>
				{
					Console.WriteLine($"AddOptions Truncate : {obj.Truncate}");
				})
				.WithParsed<CommitOptions>((obj) =>
				{
					Console.WriteLine("CommitOptions");
				})
				.WithParsed<CloneOptions>((obj) =>
				{
					Console.WriteLine("CloneOptions");
				})
				.WithNotParsed(errs =>
				{
					Console.WriteLine("error");
				});
		}
	}
}
using System.Collections.Generic;
using CommandLine;

namespace cmdline_parser_test
{
    class Options
    {
        [Option("help",
            Default = true,
            Required = false,
            HelpText = "")]
        public bool Help { get; set; }

        [Option("path",
            Default = false,
            Required = false,
            HelpText = "The path where the .sln resides")]
        public bool SolutionPath { get; set; }

        [Option("config",
            Default = false,
            Required = false,
            HelpText = "The configuration file to use")]
        public bool Config { get; set; }
    }

    class Options11
    {
        [Option('r', "read", Required = false, HelpText = "Input files to be processed.")]
        public IEnumerable<string> InputFiles { get; set; }

        // Omitting long name, defaults to name of property, ie "--verbose"
        [Option(
          Default = false,
          HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [Option("stdin",
          Default = false,
          HelpText = "Read from stdin")] 
        public bool stdin { get; set; }

        [Value(0, MetaName = "offset", HelpText = "File offset.")]
        public long? Offset { get; set; }
    }
    public class Options2
    {
        [Option('v', "verbose", Required = true, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }
    }
}

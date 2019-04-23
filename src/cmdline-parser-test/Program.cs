using System;
using System.Collections.Generic;
using CommandLine;

namespace cmdline_parser_test
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
              .WithParsed<Options>(opts => RunOptionsAndReturnExitCode(opts))
              .WithNotParsed<Options>((errs) => HandleParseError(errs));
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            Console.WriteLine("Handle error");
        }

        private static void RunOptionsAndReturnExitCode(Options opts)
        {
            Console.WriteLine($"run the option {opts.Help}");
        }

        //static void Main2(string[] args)
        //{
        //    Parser.Default.ParseArguments<Options>(args)
        //                       .WithParsed<Options>(o =>
        //                       {
        //                           if (o.Verbose)
        //                           {
        //                               Console.WriteLine($"Verbose output enabled. Current Arguments: -v {o.Verbose}");
        //                               Console.WriteLine("Quick Start Example! App is in Verbose mode!");
        //                           }
        //                           else
        //                           {
        //                               Console.WriteLine($"Current Arguments: -v {o.Verbose}");
        //                               Console.WriteLine("Quick Start Example!");
        //                           }
        //                       });
        //}

        static void MainOriginal(string[] args)
        {
            //
            // (1) default options
            //
            //var result = Parser.Default.ParseArguments<Options>(args);

            //
            // or (2) build and configure instance
            //
            var parser = new Parser(with => with.EnableDashDash = true);
            var result = parser.ParseArguments<Options>(args);

            Console.WriteLine("Hello World!");


        }
    }
}

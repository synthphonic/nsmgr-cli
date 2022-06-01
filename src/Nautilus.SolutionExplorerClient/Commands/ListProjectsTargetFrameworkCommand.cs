/*
 * 
 * https://github.com/commandlineparser/commandline/wiki
 * 
 */

namespace Nautilus.SolutionExplorerClient.Commands;

[Verb("list-targetframeworks", HelpText = Example_Text)]
internal class ListProjectsTargetFrameworkCommand : CommandBase
{
    private const string Example_Text = $"List out all projects under the solution";

    #region Command Line Setup & Options
    [Option(longName: "solutionfile", shortName: 's', Required = true, HelpText = "The full file path to the .sln file.")]
    public string SolutionFileName { get; set; }

    [Usage()]
    public static IEnumerable<Example> Examples
    {
        get
        {
            var platformPathSample = string.Empty;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                platformPathSample = @"/users/itsme/mysolution.sln";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                platformPathSample = @"C:\myproject\mysolution.sln";
            }

            return new List<Example>
                {
                    new Example(Example_Text, new ListProjectsCommand{ SolutionFileName=platformPathSample} )
                };
        }
    }
    #endregion

    public ListProjectsTargetFrameworkCommand()
    {

    }

    public override void Execute()
    {
        Run().Wait();
    }

    private Task Run()
    {
        Program.DisplayProductInfo();

        var slnFileReader = new SolutionFileReader(SolutionFileName, true);
        Solution solution;

        #region Solution file reader execution section
        try
        {
            solution = slnFileReader.Read();

            Colorful.Console.WriteLine();
            Colorful.Console.Write($"{CliStringFormatter.Format15}", "Solution ");
            Colorful.Console.WriteLine($": {solution.SolutionFileName}", Color.PapayaWhip);
            Colorful.Console.Write("Total Projects : ");
            Colorful.Console.WriteLine($"{solution.Projects.Count()}\n", Color.PapayaWhip);

            Colorful.Console.WriteLine();
            Colorful.Console.WriteLine("Working. Please wait...", Color.DeepSkyBlue);
        }
        catch (SolutionFileException)
        {
            throw;
        }
        catch (Exception)
        {
            throw;
        }
        #endregion

        Colorful.Console.WriteLine();

        WriteFinalizedResultToScreen(solution);

        return Task.CompletedTask;
    }

    private void WriteFinalizedResultToScreen(Solution solution)
    {
        var projectCounter = 1;
        foreach (var project in solution.Projects)
        {
            Colorful.Console.Write($"{CliStringFormatter.Format1}. ", projectCounter);

            if (project.ProjectType == SolutionProjectElement.CSharpProject)
            {
                Colorful.Console.Write(CliStringFormatter.Format54, Color.YellowGreen, $"{project.ProjectName}");
                Colorful.Console.Write($"[{CliStringFormatter.Format5}]", Color.Chocolate, project.TargetFramework);
                Colorful.Console.WriteLine();

                if (project.TargetFramework == ProjectTargetFramework.Unknown)
                {
                    // Todo: do something here? 
                }
            }
            else
            {
                Colorful.Console.Write(CliStringFormatter.Format45, Color.YellowGreen, $"{project.ProjectName}");

                if (project.TargetFramework == ProjectTargetFramework.Unknown)
                {
                    Colorful.Console.Write($"[{CliStringFormatter.Format7}-", Color.Chocolate, project.TargetFramework);
                    Colorful.Console.WriteLine($"{CliStringFormatter.Format9}]", Color.Chocolate, project.ProjectType);
                }
                else
                {
                    Colorful.Console.WriteLine($"[{CliStringFormatter.Format5}]", project.TargetFramework);
                }
            }

            projectCounter++;
        }
    }
}
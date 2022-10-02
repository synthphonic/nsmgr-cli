namespace Nautilus.Cli.Client.Commands;

internal class ListProjectsTargetFrameworkCommand
{
    private readonly FileInfo? _solutionFile;

    public ListProjectsTargetFrameworkCommand(FileInfo? solutionFile)
    {
        _solutionFile = solutionFile;
    }

    public void Execute()
    {
        Run().Wait();
    }

    private Task Run()
    {
        var slnFileReader = new SolutionFileReader(_solutionFile, true);
        Solution solution;

        #region Solution file reader execution section
        try
        {
            solution = slnFileReader.Read();

            Colorful.Console.WriteLine();
            Colorful.Console.Write($"{CliStringFormatter.Format15}", "Solution ");
            Colorful.Console.WriteLine($": {solution.SolutionFile}", Color.PapayaWhip);
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
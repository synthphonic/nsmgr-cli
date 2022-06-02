namespace Nautilus.SolutionExplorer.Core.Utils;

public static class XmlParser
{
    public static ElementInfo ParseFromCliParameter(string cliPropertyName)
    {
        //
        // -t PropertyGroup:Authors=aa,bb,cc
        //

        var formattedElement = cliPropertyName.Split(':');
        var searchElement = formattedElement[0];
        var targetContext = formattedElement[1];
        if (targetContext.Contains("="))
        {
           return ParseWithEqualsSign(searchElement, targetContext);
        }

        return ParseNormal(searchElement, targetContext);
    }

    private static ElementInfo ParseWithEqualsSign(string searchElement, string targetContext)
    {
        var targetElement = targetContext.Split("=")[0];
        var targetValue = targetContext.Split("=")[1];

        var elementInfo = new ElementInfo
        {
            SearchElement = searchElement,
            TargetElement = targetElement,
            TargetValue = targetValue
        };

        return elementInfo;
    }

    private static ElementInfo ParseNormal(string searchElement, string targetContext)
    {
        var targetElement = targetContext.Split("=")[0];

        var elementInfo = new ElementInfo
        {
            SearchElement = searchElement,
            TargetElement = targetElement
        };

        return elementInfo;
    }
}

public class ElementInfo
{
    public string SearchElement { get; internal set; }
    public string TargetElement { get; internal set; }
    public string TargetValue { get; internal set; }
}
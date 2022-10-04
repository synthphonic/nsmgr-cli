namespace Nautilus.SolutionExplorer.Core.Models;

public interface IProjectElementValue
{
    void Create(string elementName, string elementValue);
    void Remove(string elementName);
}
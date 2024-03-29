﻿namespace Nautilus.SolutionExplorer.Core.FileWriters;

// old class name CSharpNETStandardProjectFileWriter

public class CSharpNETCoreProjectFileWriter : IProjectFileWriter
{
    private ProjectMetadata _projectMetadata;
    private string _projectFullPath;

    #region IProjectFilePackageWriter implementations
    public void Initialize(ProjectMetadata projectMetadata)
    {
        _projectMetadata = projectMetadata;

        _projectFullPath = _projectMetadata.ProjectFullPath;
    }

    private XmlDocument LoadXmlDoc()
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.Load(_projectFullPath);

        return xmlDoc;
    }

    public void AddOrUpdateElement(string parentElement, string elementName, string value)
    {
        var allData = XElement.Load(_projectFullPath);
        var parentXElements = allData.Elements(parentElement);
        XElement parentXElement = null;

        if (parentXElements != null)
        {
            parentXElement = parentXElements.FirstOrDefault();
        }

        var found = parentXElement.Descendants(elementName);
        if (found.Any())
        {
            found.FirstOrDefault().Value = value;
        }
        else
        {            
            var newElement = new XElement(elementName, value);
            parentXElement.Add(newElement);
        }

        allData.Save(_projectFullPath);
    }

    public void AddOrUpdateElement(string elementName, string value)
    {
        var xDoc = XDocument.Load(_projectFullPath);
        var firstNode = xDoc.Root.FirstNode;

        //
        //var versionNode = firstNode.where
        XElement firstXElement = firstNode as XElement;
        XElement versionElement = firstXElement.Element(elementName);

        if (versionElement == null)
        {
            var newVersionElement = new XElement("Version")
            {
                Value = value
            };
            firstXElement.Add(newVersionElement);
        }
        else
        {
            versionElement.Value = value;
        }

        xDoc.Save(_projectFullPath);
    }

    public void UpdatePackageReference(string packageName, string newVersion)
    {
        var xmlDoc = LoadXmlDoc();

        var xpathString = $"//PackageReference[@Include='{packageName}']";
        var node = xmlDoc.DocumentElement.SelectSingleNode(xpathString);
        var versionNode = node.Attributes["Version"];
        versionNode.Value = newVersion;

        var fileInfo = new FileInfo(_projectFullPath);
        var fullBackupFile = Path.Combine(fileInfo.DirectoryName, $"{fileInfo.Name}.backup");

        // Copy original file as backup
        File.Copy(_projectFullPath, fullBackupFile, true);

        // save edited content to original file
        xmlDoc.Save(_projectFullPath);
    }

    public void DeleteElement(string parentElement, string elementName)
    {
        var allData = XElement.Load(_projectFullPath);
        var parentXElements = allData.Elements(parentElement);
        XElement parentXElement = null;

        if (parentXElements != null)
        {
            parentXElement = parentXElements.FirstOrDefault();
        }

        var found = parentXElement.Descendants(elementName);
        if (found.Any())
        {
            found.FirstOrDefault().Remove();
            allData.Save(_projectFullPath);
        }
    }
    #endregion
}

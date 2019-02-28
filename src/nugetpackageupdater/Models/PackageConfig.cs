using System.Collections.Generic;
using System.Xml.Serialization;

namespace SolutionNugetPackagesUpdater.Models
{
	[XmlRoot(ElementName = "packages")]
	public class PackageConfig
	{
		[XmlElement(ElementName = "package")]
		public List<Package> Packages { get; set; }
	}

	[XmlRoot(ElementName = "package")]
	public class Package
	{
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }
		[XmlAttribute(AttributeName = "targetFramework")]
		public string TargetFramework { get; set; }
	}
}
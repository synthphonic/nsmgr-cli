using Nautilus.Cli.Core.Logging;
using NUnit.Framework;

namespace Tests
{
	public class Logging
	{
		[SetUp]
		public void Setup()
		{
			LoggingManager.Instance.Initialize("shahlog.txt", true);
		}

		[Test]
		public void TestLogToFile1()
		{
			LoggingManager.Instance.WriteInformation("Information is written");

			LoggingManager.Instance.WriteWarning("This is a Warning!!!");

			LoggingManager.Instance.WriteError("This is an Error message");

			LoggingManager.Instance.WriteFatal("This is a Fatal message");

			LoggingManager.Instance.Disable();
		}
	}
}

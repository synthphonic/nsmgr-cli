using System;
using System.IO;
using Nautilus.Cli.Core.Exceptions;
using Nautilus.Cli.Core.Logging;
using NautilusCliTest.Helpers;
using NUnit.Framework;

namespace Tests
{
	public class LoggingTests
	{
		[SetUp]
		public void Setup()
		{

		}

		[Test]
		public void TestLogToFile_Throws_NotInitialized_LoggingException()
		{

			//
			// Act
			//
			var loggingEx = Assert.Throws<LoggingException>(() =>
			{
				LoggingManager.Instance.WriteInformation("Information is written");
			});
		}

		[Test]
		public void TestLogToFile_Expected_Successful()
		{
			//
			// Arrange
			//
			const string LogFileName = "LoggingUnitTest.txt";
			string fullFilePath = FileHelper.GetDesktopFullFileName(LogFileName);
			LoggingManager.Instance.Initialize(LogFileName, true);

			//
			// Act
			//
			LoggingManager.Instance.WriteInformation("Information is written");
			LoggingManager.Instance.WriteWarning("This is a Warning!!!");
			LoggingManager.Instance.WriteError("This is an Error message");
			LoggingManager.Instance.WriteFatal("This is a Fatal message");
			LoggingManager.Instance.Close();

			//
			// Assert
			//
			var fileExists = File.Exists(fullFilePath);
			Assert.IsTrue(fileExists, $"File '{LogFileName}' didn't exists, when it should!");

			FileHelper.DeleteFile(fullFilePath);
		}

	}
}

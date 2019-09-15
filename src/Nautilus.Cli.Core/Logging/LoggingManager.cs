using System;
using System.IO;
using Serilog;
using Serilog.Core;

namespace Nautilus.Cli.Core.Logging
{
	public sealed class LoggingManager
	{
		private static readonly LoggingManager _instance = new LoggingManager();
		private Logger _logger;
		private string _fileName;

		public static LoggingManager Instance
		{
			get { return _instance; }
		}

		public void Initialize(string fileName, bool deleteExistingFile)
		{
			if (_logger != null)
				return;

			if (deleteExistingFile)
			{
				if (File.Exists(fileName))
				{
					File.Delete(fileName);
				}

			}

			_fileName = fileName;

			_logger = new LoggerConfiguration()
				.WriteTo.File(_fileName, shared: true, fileSizeLimitBytes: null)
				.CreateLogger();
		}

		public void WriteInformation(string message)
		{
			_logger.Write(Serilog.Events.LogEventLevel.Information, message);
		}

		public void WriteWarning(string message)
		{
			_logger.Write(Serilog.Events.LogEventLevel.Warning, message);
		}

		public void WriteError(string message)
		{
			_logger.Write(Serilog.Events.LogEventLevel.Error, message);
		}

		public void WriteFatal(string message)
		{
			_logger.Write(Serilog.Events.LogEventLevel.Fatal, message);
		}

		public void WriteVerbose(string message)
		{
			_logger.Write(Serilog.Events.LogEventLevel.Verbose, message);
		}

		public void WriteVerbose(Exception ex, string message)
		{
			_logger.Write(Serilog.Events.LogEventLevel.Verbose, ex, message);
		}

		public void Disable()
		{
			if (_logger != null)
			{
				_logger.Dispose();
				_logger = null;
			}
		}
	}
}
using System;
using System.IO;
using Nautilus.Cli.Core.Exceptions;
using Serilog;
using Serilog.Core;
using static System.Environment;

namespace Nautilus.Cli.Core.Logging
{
	public sealed class LoggingManager
	{
		private static readonly LoggingManager _instance = new LoggingManager();
		private Logger _logger;
		private string _fileName;
		private string _fullFileName;
		private SpecialFolder _desktopSpecialFolder = SpecialFolder.Desktop;

		public static LoggingManager Instance
		{
			get { return _instance; }
		}

		/// <summary>
		/// Initializes the log instance
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="deleteExistingFile"></param>
		public void Initialize(string fileName, bool deleteExistingFile)
		{
			if (_logger != null)
				return;

			var writeToFolder = GetFolderPath(_desktopSpecialFolder);
			_fileName = fileName;
			_fullFileName = Path.Combine(writeToFolder, _fileName);

			if (deleteExistingFile)
			{
				if (File.Exists(_fullFileName))
				{
					File.Delete(_fullFileName);
				}
			}

			_logger = new LoggerConfiguration()
				.WriteTo.File(_fullFileName, shared: true, fileSizeLimitBytes: null)
				.CreateLogger();
		}

		public void WriteInformation(string message)
		{
			ThrowExceptionifLoggerIsNull();
			_logger.Write(Serilog.Events.LogEventLevel.Information, message);
		}

		public void WriteWarning(string message)
		{
			ThrowExceptionifLoggerIsNull();
			_logger.Write(Serilog.Events.LogEventLevel.Warning, message);
		}

		public void WriteError(string message)
		{
			ThrowExceptionifLoggerIsNull();
			_logger.Write(Serilog.Events.LogEventLevel.Error, message);
		}

		public void WriteFatal(string message)
		{
			ThrowExceptionifLoggerIsNull();
			_logger.Write(Serilog.Events.LogEventLevel.Fatal, message);
		}

		public void WriteVerbose(string message)
		{
			ThrowExceptionifLoggerIsNull();
			_logger.Write(Serilog.Events.LogEventLevel.Verbose, message);
		}

		public void WriteVerbose(Exception ex, string message)
		{
			ThrowExceptionifLoggerIsNull();
			_logger.Write(Serilog.Events.LogEventLevel.Verbose, ex, message);
		}

		/// <summary>
		/// Closes the log file and dispose any connection to the file log
		/// </summary>
		public void Close()
		{
			if (_logger != null)
			{
				_logger.Dispose();
				_logger = null;
			}
		}

		private void ThrowExceptionifLoggerIsNull()
		{
			if (_logger == null)
			{
				throw new LoggingException("Log manager was not initialized. Make sure to call Initialize() first");
			}
		}
	}
}
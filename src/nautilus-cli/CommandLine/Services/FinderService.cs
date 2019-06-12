//using System.Drawing;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading;
//using NautilusCLI.Core.Models;
//using NautilusCLI.Core.Utils;
//using System;
//using Console = Colorful.Console;
//using NugetPckgUpdater.Core;

//namespace Nautilus.Cli.Client.CLIServices
//{
//	public class FinderService : IDisposable
//    {
//        private readonly ConfigurationService _configurationService;
//        private bool disposedValue = false; // To detect redundant calls        
//        private FileStream _fs;
//        private StreamWriter _writer;

//        public FinderService(ConfigurationService configurationService)
//        {
//            _configurationService = configurationService;
//        }

//        ~FinderService()
//        {
//            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
//            Dispose(false);
//        }

//        public void Execute()
//        {
//            var parentPath = _configurationService.GetPath();
//            IEnumerable<string> files;

//            try
//            {
//                files = DirectoryUtil.GetFiles(_configurationService.GetPath(),
//                    _configurationService.GetSearchPatterns(),
//                    _configurationService.GetExcludeFoldersPattern(),
//                    SearchOption.AllDirectories);

//                files.ToList().Sort();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//            var targetFile = _configurationService.GetOutputFile();

//            if (File.Exists(targetFile))
//            {
//                File.Delete(targetFile);
//                Thread.Sleep(300);
//            }

//            OpenStreamWriter(targetFile);

//            foreach (var file in files)
//            {
//                var projectTypeManager = new ProjectTypeManager(file);
//                var projectType = projectTypeManager.ProjectType();
//                Console.WriteLine($"{Path.GetFileName(file)} {projectType.ToString()} ", Color.RoyalBlue);

//                #region comment out kejap
//                var fileFinder = new FileReader(file);
//                var fileContentObject = fileFinder.ReadFile();

//                if (fileContentObject is PackageConfig)
//                {
//                    var finder = new PackageConfigFileFinder(fileContentObject as PackageConfig);
//                    var packages = finder.FindAll();

//                    if (!packages.Any())
//                    {
//                        WriteToTextFile($"[NONE] {file}");
//                    }
//                    else
//                    {
//                        WriteToTextFile(file);

//                        foreach (var package in packages)
//                        {
//                            var textToWrite = $"{package.Id} {package.TargetFramework} {package.Version}";
//                            WriteToTextFile(textToWrite);
//                        }
//                    }
//                }
//                else if (fileContentObject is IList<PackageReferenceItemModel>)
//                {
//                    var packageRefs = fileContentObject as IList<PackageReferenceItemModel>;

//                    if (!packageRefs.Any())
//                    {
//                        WriteToTextFile($"[NONE] {file}");
//                    }
//                    else
//                    {
//                        WriteToTextFile(file);

//                        foreach (var package in packageRefs)
//                        {
//                            var textToWrite = $"{package.Include} {package.Version}";
//                            WriteToTextFile(textToWrite);
//                        }
//                    }
//                }

//                WriteToTextFile(string.Empty);
//                #endregion
//            }
//        }

//        private void OpenStreamWriter(string targetFile)
//        {
//            _fs = File.Open(targetFile, FileMode.Append, FileAccess.Write);
//            _writer = new StreamWriter(_fs);

//            Thread.Sleep(300);
//        }

//        private void WriteToTextFile(string text)
//        {
//            _writer.WriteLine(text);
//            _writer.Flush();
//        }

//        #region IDisposable Support
//        protected virtual void Dispose(bool disposing)
//        {
//            if (!disposedValue)
//            {
//                if (disposing)
//                {
//                    if (_writer != null)
//                    {
//                        _writer.Close();
//                        _writer.Dispose();
//                    }

//                    if (_fs != null)
//                    {
//                        _fs.Close();
//                        _fs.Dispose();
//                    }
//                }

//                _writer = null;
//                _fs = null;

//                disposedValue = true;
//            }
//        }

//        // This code added to correctly implement the disposable pattern.
//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }
//        #endregion
//    }
//}
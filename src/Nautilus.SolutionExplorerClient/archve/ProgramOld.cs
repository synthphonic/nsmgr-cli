﻿//using System.Drawing;
//using Colorful;
//using NautilusCLI.Core.Models;
//using NautilusCLI.Core.Services;

//namespace Nautilus.SolutionExplorerClient
//{
//	class ProgramOld
//	{
//		static void Main2(string[] args)
//		{
//			var configService = new ConfigurationService();

//			if (args.Length == 0)
//			{
//                try
//                {
//                    if (!configService.Validate())
//                    {
//                        Console.WriteLine($"{Configuration.FileName} is not yet created", Color.Red);
//                        return;
//                    }

//                    configService.LoadConfiguration();
//                    using (var finder = new FinderService(configService))
//                    {
//                        finder.Execute();
//                    }

//                    // process here
//                }
//                catch (System.Exception ex)
//                {
//                    Console.WriteLine($"{ex.GetType().Name}\n{ex.Message}", Color.Red);
//                }
//            }
//            else
//			{
//				if (args[0].ToLower().Equals("--createappconfig"))
//				{
//					configService.CreateConfigFile();
//					Console.WriteLine($"{Configuration.FileName} file created", Color.AliceBlue);
//				}

//				return;
//			}
//		}
//	}
//}
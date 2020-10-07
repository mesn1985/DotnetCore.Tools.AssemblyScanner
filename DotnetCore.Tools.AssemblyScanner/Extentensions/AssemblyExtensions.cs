using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotnetCore.Tools.AssemblyScanner.Extentions
{
	public static class AssemblyExtensions
	{
		internal static bool AssemblyIsValidCLR(this string path)
		{
			try
			{
				AssemblyName.GetAssemblyName(path);
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}
		internal static Assembly LoadAssemblyIfPossible(this AssemblyName assemblyName)
		{
			try
			{
				return Assembly.Load(assemblyName);
			}
			catch (Exception e)
			{
				return null;
			}

		}
		internal static IEnumerable<AssemblyName> GetValiAssemblyNamesFromPaths(this string[] paths)
		{
			var validAssemblyNames = new List<AssemblyName>();

			foreach (string path in paths)
			{
				try
				{
					var assemblyName = AssemblyName.GetAssemblyName(path);
					validAssemblyNames.Add(assemblyName);
				}
				catch (Exception e)
				{
					Console.WriteLine($"Failed to load {path}");
				}
			}

			return validAssemblyNames;
		}

	}
}

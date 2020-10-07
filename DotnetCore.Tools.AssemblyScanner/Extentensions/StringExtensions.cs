using System;
using System.Collections.Generic;

namespace DotnetCore.Tools.AssemblyScanner.Extentions
{
    internal static class StringExtensions
    {
        internal static bool IsNotExcludedRootAssemblyName(this string assemblyName,ISet<string> excludeRootAssemblyNames)
        {
            var assemblyRootNameDelimitIndex = assemblyName.IndexOf(".", StringComparison.Ordinal);

            if (assemblyRootNameDelimitIndex < 0)
                return false;

            var assemblyRootName = assemblyName.Substring(0, assemblyRootNameDelimitIndex);

            return !excludeRootAssemblyNames.Contains(assemblyName);
        }
    }
}

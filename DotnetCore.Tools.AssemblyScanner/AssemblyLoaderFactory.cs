using System;
using System.Collections.Generic;

namespace DotnetCore.Tools.AssemblyScanner
{
    public static class AssemblyLoaderFactory
    {
        public static AssemblyLoader CreateAssemblyLoader(
             ISet<string> loadedAssemblyNames,
            ISet<string> excludedAssemblyNames,
            Action<string> logMethod
            ) => new AssemblyLoaderImpl(loadedAssemblyNames, excludedAssemblyNames, logMethod);
         

    }
}

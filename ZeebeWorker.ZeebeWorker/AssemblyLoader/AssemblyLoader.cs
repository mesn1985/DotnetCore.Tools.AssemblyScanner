using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ZeebeWorker.TypeScanner.Extensions;
using ZeebeWorker.ZeebeWorker.Extensions;

namespace ZeebeWorker.ZeebeWorker.AssemblyLoader
{
    internal class AssemblyLoader
    {
        private readonly ISet<string> loadedAssemblyNames;
        private readonly ISet<string> excludedRootAssemblyNames;

        public AssemblyLoader(ISet<string> loadedAssemblyNames, ISet<string> excludedRootAssemblyNames)
        {
            this.loadedAssemblyNames = loadedAssemblyNames;
            this.excludedRootAssemblyNames = excludedRootAssemblyNames;
        }

        public void LoadAllDLLAssembliesFromProjectBinFolderToAppDomain()
        {
            //TODO
            #warning All file assemblies are loaded, should be changed to better time complexity

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                loadedAssemblyNames.Add(assembly.FullName);
            
            Directory
                    .GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                    .Where(path => path.AssemblyIsValidCLR())
                    .Select(path => AssemblyName.GetAssemblyName(path))
                    .Where(assemblyName => AssemblyShouldBeLoaded(assemblyName.FullName))
                    .ToList()
                    .ForEach(assemblyName => 
                        LoadAssemblyDependencyTree(Assembly.Load(assemblyName)));
        }
        private void LoadAssemblyDependencyTree(Assembly assembly)
        {

            assembly
                .GetReferencedAssemblies()
                .Where(refferedAssembly => AssemblyShouldBeLoaded(refferedAssembly.FullName))
                .ToList()
                .ForEach(assemblyName =>
                {
	                var assembly = assemblyName.LoadAssemblyIfPossible();

	                if (assembly != null)
	                {
		                LoadAssemblyDependencyTree(assembly);
		                loadedAssemblyNames.Add(assembly.FullName);
                    }
                });
        }

        private bool AssemblyShouldBeLoaded(string assemblyName)
            => assemblyName.IsNotExcludedRootAssemblyName(excludedRootAssemblyNames)
               && !loadedAssemblyNames.Contains(assemblyName);
    }
}

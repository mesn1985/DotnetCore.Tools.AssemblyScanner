using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using DotnetCore.Tools.AssemblyScanner.Extentions;

namespace DotnetCore.Tools.AssemblyScanner {
    internal class AssemblyLoaderImpl : AssemblyLoader
    {
        private readonly ISet<string> loadedAssemblyNames;
        private readonly ISet<string> excludedRootAssemblyNames;
        private readonly Action<string> logMethod;

        public AssemblyLoaderImpl(
            ISet<string> loadedAssemblyNames,
            ISet<string> excludedAssemblyNames,
            Action<string> logMetheod)
        {
            this.logMethod = logMethod ?? System.Console.WriteLine;
            this.loadedAssemblyNames = loadedAssemblyNames ?? new HashSet<string>();
            this.excludedRootAssemblyNames = excludedAssemblyNames ?? new HashSet<string>();
        }

        public void LoadAllDLLAssembliesFromProjectBinFolderToAppDomain()
        {
            try
            {
                AddAlreadyLoadedAssemblyNameStringsToSetOfloadedAssemblyNames();

                var dllFilePaths = GetAllFilePathsWithCLRValidMSILcode();

                var assemblyNamesToLoad = GetAllAssemblyNamesThatShouldBeLoadedIntoTheAppDomain(dllFilePaths);

                LoadAllAssembliesFrom(assemblyNamesToLoad);
            }
            catch (Exception e) {
                logMethod(e.Message);
            }
        }
        private void LoadAllAssembliesFrom(IEnumerable<AssemblyName> assemblyNamesToLoad) {
            foreach (var AssemblyName in assemblyNamesToLoad)
                LoadAssemblyDependencyTree(Assembly.Load(AssemblyName));
        
        }
        
        private IEnumerable<AssemblyName> GetAllAssemblyNamesThatShouldBeLoadedIntoTheAppDomain(IEnumerable<string> dllFilePaths) 
                => dllFilePaths.Select(path => AssemblyName.GetAssemblyName(path))
                    .Where(assemblyName => AssemblyShouldBeLoaded(assemblyName.FullName));
        
        private IEnumerable<String> GetAllFilePathsWithCLRValidMSILcode()
            => Directory
               .GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
               .Where(path => path.LeadsToAssemblyWithValidCLRIL());
        
        private void LoadAssemblyDependencyTree(Assembly assembly)
            => assembly
                .GetReferencedAssemblies()
                .Where(refferedAssembly => AssemblyShouldBeLoaded(refferedAssembly.FullName))
                .ToList()
                .ForEach(assemblyName => LoadAssembly(assemblyName));

        private void LoadAssembly(AssemblyName assemblyName)
        {
            var assembly = assemblyName.LoadAssemblyIfPossible();

            if (assembly != null)
            {
                LoadAssemblyDependencyTree(assembly);
                loadedAssemblyNames.Add(assembly.FullName);
            }

        }

        private bool AssemblyShouldBeLoaded(string assemblyName)
            => assemblyName.IsNotExcludedRootAssemblyName(excludedRootAssemblyNames)
               && !loadedAssemblyNames.Contains(assemblyName);

        private void AddAlreadyLoadedAssemblyNameStringsToSetOfloadedAssemblyNames() {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                loadedAssemblyNames.Add(assembly.FullName);
        }
    
    }
}

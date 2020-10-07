namespace DotnetCore.Tools.AssemblyScanner
{
    public interface AssemblyLoader
    {
        void LoadAllDLLAssembliesFromProjectBinFolderToAppDomain();
    }
}
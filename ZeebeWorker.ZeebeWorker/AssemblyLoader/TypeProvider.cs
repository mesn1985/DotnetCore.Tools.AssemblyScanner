using System;
using System.Collections.Generic;
using System.Linq;
using static System.Attribute;

namespace ZeebeWorker.ZeebeWorker.AssemblyLoader
{
    internal class TypeProvider
    {
        public IEnumerable<Type> GetAllTypesInAppDomain()
            => AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => 
	            assembly.GetTypes());
        public IEnumerable<Type> GetAllTypesInAppDomainWithAttribute<TAttribute>()
            => GetAllTypesInAppDomain().Where(type => 
	            GetCustomAttribute(type, typeof(TAttribute)) != null);
    }
}

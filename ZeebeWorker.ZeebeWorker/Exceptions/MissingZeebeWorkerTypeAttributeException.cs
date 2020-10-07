using System;
using System.Runtime.CompilerServices;

namespace ZeebeWorker.ZeebeWorker.Exceptions
{
    [Serializable]
    internal class MissingZeebeWorkerTypeAttributeException : Exception
    {
        public MissingZeebeWorkerTypeAttributeException(Type type,[CallerMemberName]string caller = "")
            :base(
	            $"{type.Name} threw exception in {caller}, because ZeebeWorkerType Attribute was not applied to deriving class"
	            )
        {
            
        }
    }
}

using System;
using System.Runtime.CompilerServices;

namespace ZeebeWorker.ZeebeWorker.Exceptions
{	
	[Serializable]
	internal class MissingConfigurationException : Exception
	{
		public MissingConfigurationException(Type type, [CallerMemberName]string caller = "") 
			:base(
				$"{type.Name} threw exception in {caller}, because ZeebeWorker Section was not found in app.settings"
				)
		{
			
		}
	}
}

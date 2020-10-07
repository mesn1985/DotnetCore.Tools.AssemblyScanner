using System;
using System.Reflection;
using ZeebeWorker.Attribute;
using ZeebeWorker.ZeebeWorker.Exceptions;
using ZeebeWorker.ZeebeWorker.Models;

namespace ZeebeWorker.ZeebeWorker.Extensions
{
	internal static class WorkerModelExtensions
	{
		public static WorkerModel BuildWorkerModel(this ZeebeWorkerBase workerBase)
		{
			return
				new WorkerModel
				{
					WorkerName = $"Worker{Guid.NewGuid()}",
					ServiceType = workerBase.GetType().GetCustomAttribute<ZeebeWorkerType>().Type
					              ?? throw new MissingZeebeWorkerTypeAttributeException(workerBase.GetType())
				};
		}

	}
}

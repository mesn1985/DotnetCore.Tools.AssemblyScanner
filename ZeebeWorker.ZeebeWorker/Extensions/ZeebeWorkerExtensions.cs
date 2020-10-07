using System;
using ZeebeWorker.ZeebeWorker;

namespace ZeebeWorker.Attribute
{
    internal static class ZeebeWorkerExtensions
    {
        public static ZeebeWorkerType GetZeebeWorkerTypeAttributeFrom(this Type type)
        => System.Attribute.GetCustomAttribute(type, typeof(ZeebeWorkerType)) as ZeebeWorkerType;
    }
}

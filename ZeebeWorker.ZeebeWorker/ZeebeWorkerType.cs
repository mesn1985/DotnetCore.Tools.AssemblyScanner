using System;

namespace ZeebeWorker.ZeebeWorker
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ZeebeWorkerType : System.Attribute
    {
        public string Type { get; set; }
    }
}

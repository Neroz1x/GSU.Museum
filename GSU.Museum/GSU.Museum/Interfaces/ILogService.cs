using System.Reflection;

namespace GSU.Museum.Shared.Interfaces
{
    /// <summary>
    /// Service for logging
    /// </summary>
    public interface ILogService
    {
        void Initialize(Assembly assembly, string assemblyName);
    }
}

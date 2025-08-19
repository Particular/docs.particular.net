using System.Threading.Tasks;

namespace Shared
{
    /// <summary>
    /// Abstraction for retrieving settings
    /// You can provide different implementations for testing or running locally
    /// </summary>
    public interface IConfigurationProvider
    {
        /// <summary>
        /// Returns a setting value
        /// </summary>
        /// <param name="key">Name of the setting parameter</param>
        /// <returns>Value of the setting parameter</returns>
        public Task<string> GetConfiguration(string key);
    }
}

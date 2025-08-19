using System;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

// -------------------------------------------------------------------
// If you want to use this class in your project, then
// - Install package Azure.Security.KeyVault.Secrets
// -- dotnet add package Azure.Security.KeyVault.Secrets
// - Install package Azure.Identity
// -- dotnet add package Azure.Identity
// -------------------------------------------------------------------
namespace Shared
{
    /// <summary>
    /// Provides configuration using KeyVault
    /// Authenticates using built-in Azure mechanisms
    /// </summary>
    /// <param name="keyVaultUri">Uri of the KeyVault to extract settings from</param>
    /// <param name="tokenCredential">Optional parameter for extracting the token. DefaultAzureCredential will be used if not provided</param>
    public class KeyVaultBasedConfigurationProvider(string keyVaultUri, TokenCredential tokenCredential = null) : IConfigurationProvider
    {
        private string KeyVaultUri { get; } = keyVaultUri;
        private TokenCredential TokenCredential { get; } = tokenCredential;

        /// <summary>
        /// Extracts a setting value from KeyVault
        /// </summary>
        /// <param name="key">Name of the setting parameter</param>
        /// <returns>Value of the setting parameter</returns>
        #region config
        public async Task<string> GetConfiguration(string key)
        {
            // We take the provided TokenCredential or use the default one
            // The default one uses many mechanisms to authenticate, e.g., environment variables, VisualStudio, Azure CLI, Azure PowerShell
            TokenCredential actualTokenCredential = TokenCredential ?? new DefaultAzureCredential();
            SecretClient client = new SecretClient(new Uri(KeyVaultUri), actualTokenCredential);

            // We use the client to download the setting
            Response<KeyVaultSecret> secretResponse = await client.GetSecretAsync(key);

            // We can now extract the setting value
            var secret = secretResponse.Value;

            return secret.Value;
        }
        #endregion
    }
}

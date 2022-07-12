using System.Collections.Generic;

namespace Dev.Services.Security;

public static class SecurityDefaults
{
    /// <summary>
    /// Gets or sets an encryption key
    /// </summary>
    public static string EncryptionKey => "1234567890123456";

    /// <summary>
    /// Gets or sets a list of admin area allowed IP addresses
    /// </summary>
    public static List<string> AdminAreaAllowedIpAddresses => null;

    /// <summary>
    /// Gets or sets a value indicating whether honeypot is enabled on the registration page
    /// </summary>
    public static bool HoneypotEnabled => false;

    /// <summary>
    /// Gets or sets a honeypot input name
    /// </summary>
    public static string HoneypotInputName => "hpinput";

    /// <summary>
    /// Get or set the blacklist of static file extension for plugin directories
    /// </summary>
    public static string PluginStaticFileExtensionsBlacklist => string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether to allow non-ASCII characters in headers
    /// </summary>
    public static bool AllowNonAsciiCharactersInHeaders => true;
}

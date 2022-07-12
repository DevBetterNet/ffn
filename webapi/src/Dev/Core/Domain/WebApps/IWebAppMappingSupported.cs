namespace Dev.Core.Domain.WebApps;

/// <summary>
/// Represents an entity which supports webApp mapping
/// </summary>
public partial interface IWebAppMappingSupported
{
    /// <summary>
    /// Gets or sets a value indicating whether the entity is limited/restricted to certain webApps
    /// </summary>
    bool LimitedToWebApps { get; set; }
}

using Dev.Core.Domain.Localization;
using System;

namespace Dev.Core.Domain.Configuration;

/// <summary>
/// Represents a setting
/// </summary>
public partial class Setting : BaseEntity, ILocalizedEntity
{
    public Setting()
    {
    }

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="name">Name</param>
    /// <param name="value">Value</param>
    /// <param name="webAppId">WebApp identifier</param>
    public Setting(string name, string value, Guid webAppId = new Guid())
    {
        Name = name;
        Value = value;
        WebAppId = webAppId;
    }

    /// <summary>
    /// Gets or sets the name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the value
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Gets or sets the webApp for which this setting is valid. 0 is set when the setting is for all webApps
    /// </summary>
    public Guid WebAppId { get; set; }

    /// <summary>
    /// To string
    /// </summary>
    /// <returns>Result</returns>
    public override string ToString()
    {
        return Name;
    }
}

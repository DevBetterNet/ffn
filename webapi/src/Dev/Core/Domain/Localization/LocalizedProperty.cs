using System;

namespace Dev.Core.Domain.Localization;

public class LocalizedProperty : BaseEntity
{
    /// <summary>
    /// Gets or sets the entity identifier
    /// </summary>
    public Guid EntityId { get; set; }

    /// <summary>
    /// Gets or sets the language identifier
    /// </summary>
    public Guid LanguageId { get; set; }

    /// <summary>
    /// Gets or sets the locale key group
    /// </summary>
    public string LocaleKeyGroup { get; set; }

    /// <summary>
    /// Gets or sets the locale key
    /// </summary>
    public string LocaleKey { get; set; }

    /// <summary>
    /// Gets or sets the locale value
    /// </summary>
    public string LocaleValue { get; set; }
}

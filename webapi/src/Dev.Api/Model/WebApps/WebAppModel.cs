using Dev.Api.Framework.Models;
using System;

namespace Dev.Api.Model.WebApps;

public record WebAppModel : BaseEntityModel
{
    /// <summary>
    /// Gets or sets the store name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the store URL
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether SSL is enabled
    /// </summary>
    public bool SslEnabled { get; set; }

    /// <summary>
    /// Gets or sets the comma separated list of possible HTTP_HOST values
    /// </summary>
    public string Hosts { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the default language for this store; 0 is set when we use the default language display order
    /// </summary>
    public Guid DefaultLanguageId { get; set; }

    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }
}

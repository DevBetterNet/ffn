using System;

namespace Dev.Core.Domain.WebApps
{
    public class WebAppMapping : BaseEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Gets or sets the entity name
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets the webApp identifier
        /// </summary>
        public Guid WebAppId { get; set; }
    }
}

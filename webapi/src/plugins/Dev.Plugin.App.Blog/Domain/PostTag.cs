using Dev.Core;

namespace Dev.Plugin.App.Blog.Domain;

public class PostTag : BaseEntity
{
    /// <summary>
    /// Gets or sets the name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the tagged product count
    /// </summary>
    public int BlogPostCount { get; set; }
}

using System;

namespace Dev.Api.Framework.Models;

public record BaseEntityModel
{
    public virtual Guid Id { get; set; }
}

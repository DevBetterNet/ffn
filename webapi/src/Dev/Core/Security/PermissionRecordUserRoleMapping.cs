using System;

namespace Dev.Core.Security
{
    public class PermissionRecordUserRoleMapping : BaseEntity
    {
        public Guid PermissionRecordId { get; set; }

        public Guid UserRoleId { get; set; }
    }
}

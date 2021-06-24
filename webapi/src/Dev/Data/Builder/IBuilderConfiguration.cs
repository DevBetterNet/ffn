using Microsoft.EntityFrameworkCore;

namespace Dev.Data.Builder
{
    public interface IBuilderConfiguration
    {
        void ApplyConfiguration(ModelBuilder modelBuilder);
    }
}
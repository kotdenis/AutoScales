using AutoScales.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoScales.Data.Extensions
{
    public static class SaveContextExtensions
    {
        public static void OnBeforeSaving(this DbContext context)
        {
            var now = DateTime.UtcNow;

            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is BaseEntity baseEntity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            baseEntity.Created = now;
                            baseEntity.Updated = now;
                            break;
                        case EntityState.Modified:
                            baseEntity.Updated = now;
                            break;
                    }
                }
            }
        }
    }
}

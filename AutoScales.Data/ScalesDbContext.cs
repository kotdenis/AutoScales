using AutoScales.Data.Entities;
using AutoScales.Data.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AutoScales.Data
{
    public class ScalesDbContext : IdentityDbContext<User, Role, int>
    {
        public ScalesDbContext(DbContextOptions<ScalesDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Transport>();
            builder.Entity<Journal>();
            builder.Entity<TransportQuantity>();
        }

        public override Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            this.OnBeforeSaving();
            return base.SaveChangesAsync(ct);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
    }
}

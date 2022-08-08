using AutoScales.Data.Entities;
using AutoScales.Data.Repositories.Interfaces;

namespace AutoScales.Data.Repositories.Implementation
{
    public class JournalRepository : GenericRepository<Journal>, IJournalRepository
    {
        public JournalRepository(ScalesDbContext dbContext) : base(dbContext)
        {

        }
    }
}

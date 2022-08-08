using AutoScales.Data.Entities;
using AutoScales.Data.Repositories.Interfaces;

namespace AutoScales.Data.Repositories.Implementation
{
    public class TransportQuantityRepository : GenericRepository<TransportQuantity>, ITransportQuantityRepository
    {
        public TransportQuantityRepository(ScalesDbContext dbContext) : base(dbContext) { }

    }
}

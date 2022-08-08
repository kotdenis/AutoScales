using AutoScales.Data.Entities;
using AutoScales.Data.Repositories.Interfaces;

namespace AutoScales.Data.Repositories.Implementation
{
    public class WeighingRepository : GenericRepository<Transport>, IWeighingRepository
    {
        public WeighingRepository(ScalesDbContext context) : base(context)
        {

        }
    }
}

using AutoScales.Data.Entities;
using AutoScales.Data.Repositories.Interfaces;

namespace AutoScales.Data.Repositories.Implementation
{
    public class AuthRepository : GenericRepository<User>, IAuthRepository
    {
        public AuthRepository(ScalesDbContext scalesDbContext) : base(scalesDbContext)
        {

        }
    }
}

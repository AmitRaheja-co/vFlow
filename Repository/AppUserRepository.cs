using vFlow.Data;
using vFlow.Models;
using vFlow.Repository.IRepository;

namespace vFlow.Repository
{
    public class AppUserRepository : Repository<AppUser>, IAppUserRepository
    {
        private readonly ApplicationDbContext _db;

        public AppUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
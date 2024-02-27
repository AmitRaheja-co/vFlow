using vFlow.Data;
using vFlow.Models;
using vFlow.Repository.IRepository;

namespace vFlow.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly ApplicationDbContext _db;

        public PostRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Post obj)
        {
            var objFromDb = _db.Posts.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Body = obj.Body;
                objFromDb.Tags = obj.Tags;
            }

        }
    }
}

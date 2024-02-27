using vFlow.Data;
using vFlow.Models;
using vFlow.Repository.IRepository;

namespace vFlow.Repository
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private readonly ApplicationDbContext _db;
        public CommentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Edit(Comment obj)
        {
            var objFromDb = _db.Comments.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Body = obj.Body;
            }
        }
    }
}

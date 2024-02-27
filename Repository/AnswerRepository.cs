using vFlow.Data;
using vFlow.Models;
using vFlow.Repository.IRepository;

namespace vFlow.Repository
{
    public class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        private readonly ApplicationDbContext _db;
        public AnswerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Edit(Answer ans)
        {
            var objFromDb = _db.Answers.FirstOrDefault(u => u.Id == ans.Id);
            if (objFromDb != null)
            {
                objFromDb.Body = ans.Body;
            }
        }
    }
}

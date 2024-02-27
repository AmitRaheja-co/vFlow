using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using vFlow.Data;
using vFlow.Repository.IRepository;
using vFlow.Models;

namespace vFlow.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IPostRepository Posts{get; private set;}

        public IAnswerRepository Answers { get; private set;}

        public ICommentRepository Comments {  get; private set;}

        public IAppUserRepository AppUsers { get; private set;}

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Posts = new PostRepository(_db);
            Answers = new AnswerRepository(_db);
            Comments = new CommentRepository(_db);
            AppUsers = new AppUserRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
using vFlow.Models;

namespace vFlow.Repository.IRepository
{
    public interface ICommentRepository : IRepository<Comment>
    {
        public void Edit(Comment obj);
    }
}

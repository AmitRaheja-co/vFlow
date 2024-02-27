using vFlow.Models;

namespace vFlow.Repository.IRepository
{
    public interface IPostRepository :IRepository<Post>
    {
        void Update(Post obj);
    }
}

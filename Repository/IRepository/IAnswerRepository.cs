using vFlow.Models;

namespace vFlow.Repository.IRepository
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        public void Edit(Answer ans);
    }
}

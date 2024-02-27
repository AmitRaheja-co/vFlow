namespace vFlow.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IPostRepository Posts { get; }
        IAnswerRepository Answers { get; }
        ICommentRepository Comments { get; }
        IAppUserRepository AppUsers { get; }
        void Save();
    }
}

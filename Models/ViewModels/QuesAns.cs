namespace vFlow.Models.ViewModels
{
    public class QuesAns
    {
        public List<Post> posts { get; set; }
        public List<Answer> Answers { get; set; }

        public List<Comment> Comments { get; set; }

        public List<AppUser> AppUsers { get; set; }

        public QuesAns() {
            Comments = new List<Comment>();
        }


    }
}

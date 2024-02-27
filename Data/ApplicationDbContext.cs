using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using vFlow.Models;
 
namespace vFlow.Data

{

    public class ApplicationDbContext : IdentityDbContext<AppUser>

    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)

        {

        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {

            base.OnModelCreating(modelBuilder);

           

        }

    }

    }


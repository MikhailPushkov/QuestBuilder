using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace QuestBuild_MVC.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Institute> Institutes { get; set; }
        public DbSet<Faculties> Faculties { get; set; }
        public DbSet<Chairs> Chairs { get; set; }
        public DbSet<Teachers> Teachers { get; set; }
        public DbSet<Subjects> Subjects { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<Answers> Answers { get; set; }
        public DbSet<TypeOfQuestions> TypeOfQuestions { get; set; }
        public DbSet<Themes> Themes { get; set; }
        public DbSet<Students> Students { get; set; }
        public DbSet<Groups> Groups { get; set; }
        public DbSet<Works> Works { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }
}
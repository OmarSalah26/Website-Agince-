using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace RoleProject.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string city { get; set; }
        public string street { get; set; }

        public string photoAdmin{ get; set; }
        [NotMapped]
        public HttpPostedFileBase photo_path { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("WebsiteAgienceConnectionString", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
        }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Agince> Agince { get; set; }
       
        public DbSet<Car_properties> Car_properties { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Car_And_Properites> Car_And_Properites { get; set; }
        public DbSet<ReciveDate> ReciveDates { get; set; }

       
       
       
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
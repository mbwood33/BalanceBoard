// Data/ApplicationDbContext.cs
// Defines the Entity Framework Core DbContext for the BalanceBoard application.
// It includes configuration for ASP.NET Core Identity using the custom ApplicationUser
// and a DbSet for managing WeightLog entities.

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;    // Needed for IdentityDbContext
using Microsoft.EntityFrameworkCore;    // Needed for DbContext and DbContextOptions
using BalanceBoard.Data;    // Needed for ApplicationUser


namespace BalanceBoard.Data
{
    // Inherit from IdentityDbContext, specifying your custom ApplicationUser class
    class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // DbSet for your custom models that you want to store in the database
        // public DbSet<WeightLog> WeightLogs { get; set; }

        // Constructor required by Entity Framework Core
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Optional: Override OnModelCreating if you need to configure relationships,
        // table names, or other model mapping details beyond conventions.
        // For example, configuring the relationship between ApplicationUser and WeightLog.
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);  // Important: Call the base method first

            // Example of configuring the relationship if WeightLog needs a foreign key to ApplicationUser
            // This might be needed if you add a foreign key property (e.g., UserId) to WeightLog
            // and configure it here. However, IdentityUser's ID is a string (Guid),
            // while your current WeightLog has an int UserId. We will need to update WeightLog.cs
            // to use string UserId to match ApplicationUser.Id.

            // If you updated WeightLog.cs to have 'public string? UserId { get; set; }'
            // and 'public ApplicationUser? User { get; set; }' (optional navigation property),
            // you might configure the relationship like this:
            /*
            builder.Entity<WeightLog>()
                .HasOne(log => log.User) // WeightLog has one User
                .WithMany() // User has many WeightLogs (or WithMany(user => user.WeightLogs) if you add a collection to ApplicationUser)
                .HasForeignKey(log => log.UserId); // The foreign key is UserId
            */

            // Note: The default Identity schema uses "AspNetUsers", "AspNetRoles", etc. for table names.
            // If you want different table names, you can configure them here.
        }
    }
}

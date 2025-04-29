// Data/ApplicationsUser.cs
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema; // Needed for [Column] attribute

namespace BalanceBoard.Data
{
    // Inherit from IdentityUser to get standard Identity properties (Id, UserName, Email, etc.)
    public class ApplicationUser : IdentityUser
    {
        // Add custom properties for your user profile data
        // These map to columns in the AspNetUsers table created by Identity/EF Core Migrations

        public string? Name { get; set; }   // User's full name
        public DateTime? DateOfBirth { get; set; }  // User's date of birth

        // Use Column attribute if your database column name differs from the property name (optional here, but good practice)
        [Column(TypeName = "numeric")]  // Specify the database column type for height
        public double? Height { get; set; }

        // Note: Other properties like Email, PhoneNumber, UserName are inherited from IdentityUser
        // UserStats, GoalSettings, and MacroResults models are not properties of the user themselves,
        // but rather data used in calculations or temporary storage.
    }
}

// Models/ApplicationUser.cs
// Represents a user in the Identity system, extending the default IndentityUser to include additional profile properties.

using System;   // Needed for DateTime
using Microsoft.AspNetCore.Identity;    // Needed for IdentityUser

namespace BalanceBoard.Models
{
    /// <summary>
    /// CUstom user class for ASP.NET Core Identity, including additional profile fields.
    /// Inherits from IdentityUser which provides core user properties.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        // Add any custom profile properties here
        // IdentityUser already provides Id, UserName, Email, PasswordHash, etc.

        /// <summary>
        /// Gets or sets the user's full name.
        /// Nullable if not reuired during initial registration, but can be required via validation.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the user's date of birth.
        /// Nullable to allow for optional input or pending profile completion.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the user's height in inches.
        /// Nullable to allow for optional input or pending profile completion.
        /// </summary>
        public decimal? Height { get; set; }

        // Note: Add other custom properties needed for your application here (e.g., unit preferences)
        public string? PreferredUnitSystem { get; set; }    // Example: "Imperial" or "Metric"
    }
}

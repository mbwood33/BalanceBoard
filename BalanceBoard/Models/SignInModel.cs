// Models/SignInModel.cs
// Represents the data required for a user to log in.

using System.ComponentModel.DataAnnotations;    // Provides attributes for validation

namespace BalanceBoard.Models
{
    /// <summary>
    /// Represents the data model fro user sign-in.
    /// </summary>
    public class SignInModel
    {
        /// <summary>
        /// Gets or sets the user's email address
        /// Required field
        /// </summary>
        [Required(ErrorMessage = "Email is required.")] // Data validation attribute
        [EmailAddress(ErrorMessage = "Invalid email format.")]  // Ensure the input is a valid email address format
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's password
        /// Required field
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]  // Data validation attribute
        public string Password { get; set; } = string.Empty;
    }
}

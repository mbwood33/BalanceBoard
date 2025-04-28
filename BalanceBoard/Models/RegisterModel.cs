// Models/RegisterModel.cs
// Represents the data requried for a user to register a new account.
// TODO: Implement metric units

using System;   // Needed for DateTime
using System.ComponentModel.DataAnnotations;    // Provides attributes for validation

namespace BalanceBoard.Models
{
    /// <summary>
    /// Represents the data model for user registration.
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// Gets or sets the user's unique identifier in the database.
        /// This is typically set by the database upon creation.
        /// </summary>
        public int Id { get; set; } // Id property for storing the user's database ID

        /// <summary>
        /// Gets or sets the user's email address.
        /// Required field with email format validation.
        /// </summary>
        [Required(ErrorMessage = "Email is required.")]     // Ensures the Email field is not left empty
        [EmailAddress(ErrorMessage = "Invalid email address.")] // Validates that the Email field contains a valid email format
        public string Email { get; set; } = string.Empty;   // Property for user email; initialized to an empty string

        /// <summary>
        /// Gets or sets the user's chosen password.
        /// Required field.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]  // Ensures the Password field is not left empty
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")] // Example: Add minimum length requirement
        // Consider adding more robust password validation requirements here (e.g., require digits, uppercase, etc.)
        public string Password { get; set; } = string.Empty;    // Property for user password; initialized to an empty string

        /// <summary>
        /// Gets or sets the user's full name.
        /// Required field.
        /// </summary>
        [Required(ErrorMessage = "Name is required.")]      // Ensures the Name field is not left empty
        public string Name { get; set; } = string.Empty;    // Property for user name; initialized to an empty string

        /// <summary>
        /// Gets or sets the user's date of birth.
        /// Required field. Using nullable DateTime to allow for picker not having a value initially.
        /// </summary>
        [Required(ErrorMessage = "Date of Birth is required.")]         // Ensures the Date of Birth field is provided
        // TODO: Add a custom validation attribute to ensure the date/time is in the past/no more recent than "now".
        public DateTime? DateOfBirth { get; set; } // Nullable DateTime for date of birth;

        /// <summary>
        /// Gets or sets the user's height in inches.
        /// Required field with a range validation. Using nullable decimal for numeric input.
        /// </summary>
        [Required(ErrorMessage = "Height (in) is required.")]   // Ensures that the Height field is not left empty
        [Range(36, 96, ErrorMessage = "Height must be between {1} and {2} inches.")]  // Validates that the Height is between 36 and 96 inches (adjust range as needed)
        public decimal? Height { get; set; }    // Nullable decimal for height input
    }
}

// Models/WeightLog.cs
// TODO: Implement metric units (including input validation for metric units)

using System;
using System.ComponentModel.DataAnnotations;

namespace BalanceBoard.Models
{
    /// <summary>
    /// Represents a single weight and body composition log entry for a user.
    /// </summary>
    public class WeightLog
    {
        // Measurement record Id; use int for the record ID to match the database SERIAL type
        public int Id { get; set; }

        // Foreign key linked to the User who created this log
        public int UserId { get; set; }


        /// <summary>
        /// The date and time the measurement was taken
        /// Required field
        /// </summary>
        [Required(ErrorMessage = "Timestamp is required.")]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        /// <summary>
        /// The user's weight in pounds
        /// Requires field with a range validation
        /// </summary>
        [Required(ErrorMessage = "Weight is required.")]
        [Range(1, 1000, ErrorMessage = "Weight must be between {1} and {2} lbs.")]
        public double? Weight { get; set; }

        /// <summary>
        /// The user's height in inches
        /// This is typically stored in the user's profile, but included here for BMI calculation context
        /// TODO: Implement BMI calculation more efficiently so height isn't required in the WeightLog class
        /// </summary>
        [Required]
        [Range(36, 120, ErrorMessage = "Height must be between {1} and {2} inches.")]   // Validation might be better on the User model
        public double? Height { get; set; }

        /// <summary>
        /// The user's body fat percentage
        /// Optional field with a range validation
        /// </summary>
        [Range(0, 100, ErrorMessage = "Body fat percentage must be between {1} and {2}.")]
        public double? BodyFatPercentage { get; set; }

        /// <summary>
        /// Calculated Body Mass Index (BMI) based on Weight and Height
        /// Formula: (Weight in lbs) / (Height in inches)^2 * 703
        /// </summary>
        public double? BMI => (Weight.HasValue && Height.HasValue && Height.Value > 0)
            ? (Weight.Value / (Height.Value * Height.Value)) * 703
            : null;
        
        /// <summary>
        /// Calculated Body Fat Weight in pounds based on Weight and Body Fat Percentage
        /// Formula: Weight * (Body Fat Percentage / 100)
        /// Rounded to 2 decimal places
        /// </summary>
        public double? BodyFatWeight => BodyFatPercentage.HasValue && Weight.HasValue
            ? Math.Round(Weight.Value * (BodyFatPercentage.Value / 100), 2)
            : null;

        /// <summary>
        /// Calculated Lean Mass in pounds based on Weight and Body Fat Weight
        /// Formula: Weight - Body Fat Weight
        /// Rounded to 2 decimal places
        /// </summary>
        public double? LeanMass => BodyFatWeight.HasValue && Weight.HasValue
            ? Math.Round(Weight.Value - BodyFatWeight.Value, 2)
            : null;

        /// <summary>
        /// Helper property to get the timestamp as a local DateTime, useful for display in the UI
        /// </summary>
        public DateTime LocalTimeStamp => Timestamp.ToLocalTime();
    }
}

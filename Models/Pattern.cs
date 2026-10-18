using System.ComponentModel.DataAnnotations;

namespace BlazorPattern.Models
{
    /// <summary>
    /// Model class for the Pattern entity.
    /// </summary>
    /// <remarks>
    /// The validation attributes are the single source of truth: <c>EditForm</c> uses them for
    /// client-side feedback and <see cref="Services.PatternService"/> re-validates them on the server.
    /// </remarks>
    public class Pattern
    {
        /// <summary>Identifier assigned by the repository on creation.</summary>
        public int Id { get; set; }

        /// <summary>Display name of the pattern.</summary>
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(200, ErrorMessage = "Name must have at most {1} characters.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>Optional free-text description.</summary>
        [StringLength(500, ErrorMessage = "Description must have at most {1} characters.")]
        public string? Description { get; set; }
    }
}

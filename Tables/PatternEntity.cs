using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorPattern.Models.Entities
{
    /// <summary>
    /// Template entity class for Pattern table.
    /// This class is a skeleton to be used as a structural model.
    /// Add or adjust properties as needed in your project.
    /// </summary>
    [Serializable]
    [Table("pattern")]  // name of the table in your database (lowercase or as convention)
    public class PatternEntity
    {
        #region Public Properties

        /// <summary>
        /// Primary key: Id of the Pattern.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Name of the Pattern.
        /// </summary>
        [Required(ErrorMessage = "Name is required.")]
        [Column("name", TypeName = "varchar(200)")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description or additional info.
        /// </summary>
        [Column("description", TypeName = "varchar(500)")]
        public string? Description { get; set; }

        /// <summary>
        /// Creation timestamp.
        /// </summary>
        [Column("created_at", TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Update timestamp (nullable if not updated yet).
        /// </summary>
        [Column("updated_at", TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }

        #endregion Public Properties
    }
}

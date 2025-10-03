using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorPattern.Models;

namespace BlazorPattern.Services
{
    /// <summary>
    /// Interface for the service that handles Pattern entity operations.
    /// Defines the public methods that the service will expose.
    /// </summary>
    public interface IPatternService
    {
        /// <summary>
        /// Retrieves all patterns.
        /// </summary>
        /// <returns>A collection of Pattern objects.</returns>
        Task<IEnumerable<Pattern>> GetAllAsync();

        /// <summary>
        /// Retrieves a single pattern by its identifier.
        /// </summary>
        /// <param name="id">Identifier of the Pattern.</param>
        /// <returns>The Pattern object, or null if not found.</returns>
        Task<Pattern> GetByIdAsync(int id);

        /// <summary>
        /// Creates a new Pattern.
        /// </summary>
        /// <param name="item">The Pattern to create.</param>
        /// <returns>The created Pattern object.</returns>
        Task<Pattern> CreateAsync(Pattern item);

        /// <summary>
        /// Updates an existing Pattern.
        /// </summary>
        /// <param name="item">The Pattern with updated data.</param>
        /// <returns>The updated Pattern object.</returns>
        Task<Pattern> UpdateAsync(Pattern item);

        /// <summary>
        /// Deletes a Pattern by its identifier.
        /// </summary>
        /// <param name="id">Identifier of the Pattern to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(int id);
    }
}

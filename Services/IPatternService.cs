using BlazorPattern.Models;

namespace BlazorPattern.Services
{
    /// <summary>
    /// Interface for the service that handles Pattern entity operations.
    /// Defines the public methods that the UI layer consumes.
    /// </summary>
    public interface IPatternService
    {
        /// <summary>
        /// Retrieves all patterns.
        /// </summary>
        /// <returns>A read-only list of Pattern objects ordered by id.</returns>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<IReadOnlyList<Pattern>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a single pattern by its identifier.
        /// </summary>
        /// <param name="id">Identifier of the Pattern.</param>
        /// <returns>The Pattern object, or <c>null</c> if not found.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> is not positive.</exception>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<Pattern?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new Pattern.
        /// </summary>
        /// <param name="item">The Pattern to create. Its <see cref="Pattern.Id"/> is ignored.</param>
        /// <returns>The created Pattern object, including its generated id.</returns>
        /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException"><paramref name="item"/> is invalid.</exception>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<Pattern> CreateAsync(Pattern item, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing Pattern.
        /// </summary>
        /// <param name="item">The Pattern with updated data.</param>
        /// <returns>The updated Pattern object.</returns>
        /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException"><paramref name="item"/> is invalid.</exception>
        /// <exception cref="KeyNotFoundException">The pattern does not exist.</exception>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task<Pattern> UpdateAsync(Pattern item, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a Pattern by its identifier.
        /// </summary>
        /// <param name="id">Identifier of the Pattern to delete.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> is not positive.</exception>
        /// <exception cref="KeyNotFoundException">The pattern does not exist.</exception>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}

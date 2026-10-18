using BlazorPattern.Models;

namespace BlazorPattern.Services
{
    /// <summary>
    /// Storage abstraction for Pattern entities. Implementations must return copies,
    /// never the instances they keep internally, so callers cannot mutate stored state.
    /// </summary>
    public interface IPatternRepository
    {
        /// <summary>Returns every pattern ordered by <see cref="Pattern.Id"/>.</summary>
        Task<IReadOnlyList<Pattern>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>Returns the pattern with the given id, or <c>null</c> when it does not exist.</summary>
        Task<Pattern?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>Stores a new pattern, assigning its <see cref="Pattern.Id"/>, and returns the stored copy.</summary>
        Task<Pattern> AddAsync(Pattern item, CancellationToken cancellationToken = default);

        /// <summary>Replaces an existing pattern. Returns <c>false</c> when it does not exist.</summary>
        Task<bool> UpdateAsync(Pattern item, CancellationToken cancellationToken = default);

        /// <summary>Removes a pattern. Returns <c>false</c> when it does not exist.</summary>
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}

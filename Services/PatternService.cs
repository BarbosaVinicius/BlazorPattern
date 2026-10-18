using System.ComponentModel.DataAnnotations;
using BlazorPattern.Models;

namespace BlazorPattern.Services
{
    /// <summary>
    /// Implementation of the service for handling Pattern entities.
    /// Includes business logic, validations, etc.
    /// </summary>
    public class PatternService : IPatternService
    {
        private readonly IPatternRepository _repository;
        private readonly ILogger<PatternService> _logger;

        public PatternService(IPatternRepository repository, ILogger<PatternService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public Task<IReadOnlyList<Pattern>> GetAllAsync(CancellationToken cancellationToken = default) =>
            _repository.GetAllAsync(cancellationToken);

        public Task<Pattern?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            ValidateId(id);
            return _repository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<Pattern> CreateAsync(Pattern item, CancellationToken cancellationToken = default)
        {
            ValidateItem(item);

            var created = await _repository.AddAsync(item, cancellationToken);
            _logger.LogInformation("Pattern {Id} created.", created.Id);
            return created;
        }

        public async Task<Pattern> UpdateAsync(Pattern item, CancellationToken cancellationToken = default)
        {
            ValidateItem(item);
            ValidateId(item.Id);

            if (!await _repository.UpdateAsync(item, cancellationToken))
                throw new KeyNotFoundException($"Pattern {item.Id} not found.");

            _logger.LogInformation("Pattern {Id} updated.", item.Id);
            return item;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            ValidateId(id);

            if (!await _repository.DeleteAsync(id, cancellationToken))
                throw new KeyNotFoundException($"Pattern {id} not found.");

            _logger.LogInformation("Pattern {Id} deleted.", id);
        }

        private static void ValidateId(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than zero.");
        }

        // Re-runs the DataAnnotations declared on Pattern so rules hold even if the UI is bypassed.
        private static void ValidateItem(Pattern item)
        {
            ArgumentNullException.ThrowIfNull(item);
            Validator.ValidateObject(item, new ValidationContext(item), validateAllProperties: true);
        }
    }
}

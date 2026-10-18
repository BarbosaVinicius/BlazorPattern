using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorPattern.Models;
using Microsoft.Extensions.Logging;

namespace BlazorPattern.Services
{
    /// <summary>
    /// Implementation of the service for handling Pattern entities.
    /// Includes business logic, validations, etc.
    /// </summary>
    public class PatternService : IPatternService
    {
        private const int MaxNameLength = 200;

        private readonly IPatternRepository _repository;
        private readonly ILogger<PatternService> _logger;

        public PatternService(IPatternRepository repository, ILogger<PatternService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public Task<IEnumerable<Pattern>> GetAllAsync() => _repository.GetAllAsync();

        public Task<Pattern> GetByIdAsync(int id)
        {
            ValidateId(id);
            return _repository.GetByIdAsync(id);
        }

        public async Task<Pattern> CreateAsync(Pattern item)
        {
            ValidateItem(item);

            var created = await _repository.AddAsync(item);
            _logger.LogInformation("Pattern {Id} created.", created.Id);
            return created;
        }

        public async Task<Pattern> UpdateAsync(Pattern item)
        {
            ValidateItem(item);
            ValidateId(item.Id);

            if (!await _repository.UpdateAsync(item))
                throw new KeyNotFoundException($"Pattern {item.Id} not found.");

            _logger.LogInformation("Pattern {Id} updated.", item.Id);
            return item;
        }

        public async Task DeleteAsync(int id)
        {
            ValidateId(id);

            if (!await _repository.DeleteAsync(id))
                throw new KeyNotFoundException($"Pattern {id} not found.");

            _logger.LogInformation("Pattern {Id} deleted.", id);
        }

        private static void ValidateId(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than zero.");
        }

        private static void ValidateItem(Pattern item)
        {
            ArgumentNullException.ThrowIfNull(item);

            if (string.IsNullOrWhiteSpace(item.Name))
                throw new ArgumentException("Name is required.", nameof(item));

            if (item.Name.Length > MaxNameLength)
                throw new ArgumentException($"Name must have at most {MaxNameLength} characters.", nameof(item));
        }
    }
}

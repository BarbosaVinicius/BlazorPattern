using System.Collections.Concurrent;
using BlazorPattern.Models;

namespace BlazorPattern.Services
{
    /// <summary>
    /// Thread-safe in-memory repository seeded with sample data. Register as singleton; replace with a
    /// database-backed implementation of <see cref="IPatternRepository"/> when persistence is needed.
    /// </summary>
    public class InMemoryPatternRepository : IPatternRepository
    {
        private readonly ConcurrentDictionary<int, Pattern> _items = new();
        private int _lastId;

        /// <summary>Creates the repository with a few sample patterns so the UI is not empty on first run.</summary>
        public InMemoryPatternRepository()
        {
            foreach (var (name, description) in new[]
            {
                ("Repository", "Hides data access behind a collection-like interface."),
                ("Service Layer", "Holds business rules and validation between UI and storage."),
                ("Dependency Injection", "Supplies collaborators from the outside instead of creating them."),
            })
            {
                var id = Interlocked.Increment(ref _lastId);
                _items[id] = new Pattern { Id = id, Name = name, Description = description };
            }
        }

        public Task<IReadOnlyList<Pattern>> GetAllAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<Pattern>>(_items.Values.OrderBy(p => p.Id).Select(Clone).ToList());

        public Task<Pattern?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
            Task.FromResult(_items.TryGetValue(id, out var item) ? Clone(item) : null);

        public Task<Pattern> AddAsync(Pattern item, CancellationToken cancellationToken = default)
        {
            var stored = Clone(item);
            stored.Id = Interlocked.Increment(ref _lastId);
            _items[stored.Id] = stored;
            return Task.FromResult(Clone(stored));
        }

        public Task<bool> UpdateAsync(Pattern item, CancellationToken cancellationToken = default)
        {
            // Atomic: only replaces when the key is still present, so a concurrent delete is not resurrected.
            var updated = _items.TryGetValue(item.Id, out var current) && _items.TryUpdate(item.Id, Clone(item), current);
            return Task.FromResult(updated);
        }

        public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default) =>
            Task.FromResult(_items.TryRemove(id, out _));

        private static Pattern Clone(Pattern p) => new() { Id = p.Id, Name = p.Name, Description = p.Description };
    }
}

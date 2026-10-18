using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlazorPattern.Models;

namespace BlazorPattern.Services
{
    /// <summary>
    /// Thread-safe in-memory repository. Register as singleton; replace with a
    /// database-backed implementation of IPatternRepository when persistence is needed.
    /// </summary>
    public class InMemoryPatternRepository : IPatternRepository
    {
        private readonly ConcurrentDictionary<int, Pattern> _items = new();
        private int _lastId;

        public Task<IEnumerable<Pattern>> GetAllAsync() =>
            Task.FromResult<IEnumerable<Pattern>>(_items.Values.OrderBy(p => p.Id).Select(Clone).ToList());

        public Task<Pattern> GetByIdAsync(int id) =>
            Task.FromResult(_items.TryGetValue(id, out var item) ? Clone(item) : null);

        public Task<Pattern> AddAsync(Pattern item)
        {
            var stored = Clone(item);
            stored.Id = Interlocked.Increment(ref _lastId);
            _items[stored.Id] = stored;
            return Task.FromResult(Clone(stored));
        }

        public Task<bool> UpdateAsync(Pattern item)
        {
            if (!_items.ContainsKey(item.Id))
                return Task.FromResult(false);

            _items[item.Id] = Clone(item);
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(int id) => Task.FromResult(_items.TryRemove(id, out _));

        private static Pattern Clone(Pattern p) => new() { Id = p.Id, Name = p.Name };
    }
}

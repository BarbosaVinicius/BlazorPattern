using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorPattern.Models;

namespace BlazorPattern.Services
{
    /// <summary>
    /// Storage abstraction for Pattern entities.
    /// </summary>
    public interface IPatternRepository
    {
        Task<IEnumerable<Pattern>> GetAllAsync();
        Task<Pattern> GetByIdAsync(int id);
        Task<Pattern> AddAsync(Pattern item);
        Task<bool> UpdateAsync(Pattern item);
        Task<bool> DeleteAsync(int id);
    }
}

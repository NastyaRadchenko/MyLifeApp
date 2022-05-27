using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IDiaryRepository
    {
        Task<IEnumerable<DiaryEntry>> GetAllEntriesAsync(bool trackChanges);
        Task<DiaryEntry> GetEntryByIdAsync(Guid entryId, bool trackChanges);
        Task<IEnumerable<DiaryEntry>> GetEntriesByUserIdAsync(Guid userId, bool trackChanges);
        void CreateEntry(DiaryEntry entry);
        void DeleteEntry(DiaryEntry entry);
    }
}

using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class DiaryRepository : RepositoryBase<DiaryEntry>, IDiaryRepository
    {
        public DiaryRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<DiaryEntry>> GetAllEntriesAsync(bool trackChanges) =>
        await FindAll(trackChanges)
         .OrderBy(c => c.Date)
         .ToListAsync();

        public async Task<DiaryEntry> GetEntryByIdAsync(Guid entryId, bool trackChanges) =>
            await FindByCondition(e => e.Id.Equals(entryId), trackChanges)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<DiaryEntry>> GetEntriesByUserIdAsync(Guid userId, bool trackChanges) =>
            await FindByCondition(e => e.UserId.Equals(userId), trackChanges)
            .ToListAsync();

        public void CreateEntry(DiaryEntry entry) => Create(entry);
        public void DeleteEntry(DiaryEntry entry) => Delete(entry);
    }
}

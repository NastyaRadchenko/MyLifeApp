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
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<Book> GetBookByIdAsync(Guid bookId, bool trackChanges) =>
            await FindByCondition(e => e.Id.Equals(bookId), trackChanges)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(Guid userId, Guid categoryId, bool trackChanges) =>
            await FindByCondition(e => e.UserId.Equals(userId) && e.CategoryId.Equals(categoryId), trackChanges)
            .ToListAsync();

        public async Task<IEnumerable<Book>> GetBooksByUserIdAsync(Guid userId, bool trackChanges) =>
            await FindByCondition(e => e.UserId.Equals(userId), trackChanges)
            .ToListAsync();

        public void CreateBook(Book book) => Create(book);
        public void DeleteBook(Book book) => Delete(book);
    }
}

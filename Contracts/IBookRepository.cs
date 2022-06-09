using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IBookRepository
    {
        Task<Book> GetBookByIdAsync(Guid bookId, bool trackChanges);
        Task<IEnumerable<Book>> GetBooksByCategoryAsync(Guid userId, Guid categoryId, bool trackChanges);
        Task<IEnumerable<Book>> GetBooksByUserIdAsync(Guid userId, bool trackChanges);
        void CreateBook(Book book);
        void DeleteBook(Book book);
    }
}

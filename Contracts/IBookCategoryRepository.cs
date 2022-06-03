using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IBookCategoryRepository
    {
        Task<IEnumerable<BookCategory>> GetAllCategoriesAsync(bool trackChanges);
    }
}

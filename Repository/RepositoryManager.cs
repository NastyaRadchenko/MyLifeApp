using Contracts;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryContext _repositoryContext;

        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private IDiaryRepository _diaryRepository;
        private IStateRepository _stateRepository;
        private IBookCategoryRepository _bookCategoryRepository;
        private IBookRepository _bookRepository;
        private IPurchaseCategoryRepository _purchaseCategoryRepository;
        private IPurchaseRepository _purchaseRepository;
        private IRecipeRepository _recipeRepository;
        private IStageRepository _stageRepository;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }
        public IUserRepository User
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_repositoryContext);
                return _userRepository;
            }
        }

        public IRoleRepository Role
        {
            get
            {
                if (_roleRepository == null)
                    _roleRepository = new RoleRepository(_repositoryContext);
                return _roleRepository;
            }
        }

        public IDiaryRepository Diary
        {
            get
            {
                if (_diaryRepository == null)
                    _diaryRepository = new DiaryRepository(_repositoryContext);
                return _diaryRepository;
            }
        }

        public IStateRepository State
        {
            get
            {
                if (_stateRepository == null)
                    _stateRepository = new StateRepository(_repositoryContext);
                return _stateRepository;
            }
        }

        public IBookRepository Book
        {
            get
            {
                if (_bookRepository == null)
                    _bookRepository = new BookRepository(_repositoryContext);
                return _bookRepository;
            }
        }

        public IBookCategoryRepository BookCategory
        {
            get
            {
                if (_bookCategoryRepository == null)
                    _bookCategoryRepository = new BookCategoryRepository(_repositoryContext);
                return _bookCategoryRepository;
            }
        }

        public IPurchaseCategoryRepository PurchaseCategory
        {
            get
            {
                if (_purchaseCategoryRepository == null)
                    _purchaseCategoryRepository = new PurchaseCategoryRepository(_repositoryContext);
                return _purchaseCategoryRepository;
            }
        }

        public IPurchaseRepository Purchase
        {
            get
            {
                if (_purchaseRepository == null)
                    _purchaseRepository = new PurchaseRepository(_repositoryContext);
                return _purchaseRepository;
            }
        }

        public IRecipeRepository Recipe
        {
            get
            {
                if (_recipeRepository == null)
                    _recipeRepository = new RecipeRepository(_repositoryContext);
                return _recipeRepository;
            }
        }

        public IStageRepository Stage
        {
            get
            {
                if (_stageRepository == null)
                    _stageRepository = new StageRepository(_repositoryContext);
                return _stageRepository;
            }
        }
        public Task SaveAsync() => _repositoryContext.SaveChangesAsync();
    }
}

﻿using Contracts;
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

        public Task SaveAsync() => _repositoryContext.SaveChangesAsync();
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Users.API.Dtos.InputModels;
using Users.API.Dtos.ViewModels;
using Users.API.Services.Interfaces;
using Users.Domain.Entities;
using Users.Domain.Interfaces.Repositories;

namespace Users.API.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;

        public UsersService(IUsersRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        public async Task<UserViewModel> CreateUser(UserInputModel model)
        {
            var userExists = await _usersRepository.GetByEmailAsync(
                model.Email
            );

            if (userExists != null)
            {
                return null;
            }

            var user = _mapper.Map<User>(model);

            user = await _usersRepository.CreateAsync(user);

            return _mapper.Map<UserViewModel>(user);
        }

        public async Task<UserViewModel> GetUserById(Guid id)
        {
            return _mapper.Map<UserViewModel>(
                await _usersRepository.GetByIdAsync(id.ToString())
            );
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUsers()
        {
            return _mapper.Map<IEnumerable<UserViewModel>>(
                await _usersRepository.GetAllAsync()
            );
        }

        public async Task UpdateUser(UserInputModel model, Guid id)
        {
            var userExists = await _usersRepository.GetByIdAsync(id.ToString());

            if (userExists == null)
            {
                return;
            }

            var user = new User()
            {
                Id = userExists.Id,
                FullName = model.FullName,
                Email = model.Email,
                CreatedAt = userExists.CreatedAt,
                UpdatedAt = DateTime.Now,
                DeletedAt = null
            };

            await _usersRepository.UpdateAsync(user);
        }
    }
}

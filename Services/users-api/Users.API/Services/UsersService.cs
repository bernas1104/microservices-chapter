using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Users.API.Dtos.InputModels;
using Users.API.Dtos.ViewModels;
using Users.API.Services.Interfaces;
using Users.Domain.Entities;
using Users.Domain.Interfaces.MessageBus;
using Users.Domain.Interfaces.Repositories;

namespace Users.API.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMessageBusClient _messageBus;
        private readonly IMapper _mapper;

        public UsersService(
            IUsersRepository usersRepository,
            IMessageBusClient messageBus,
            IMapper mapper
        )
        {
            _usersRepository = usersRepository;
            _messageBus = messageBus;
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

            var user = User.Create(model.FullName, model.Email);

            user = await _usersRepository.CreateAsync(user);

            foreach(var @event in user.Events)
            {
                var routingKey = "user-created";

                _messageBus.Publish(
                    message: @event,
                    routingKey: routingKey,
                    exchange: "user-service"
                );
            }

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
            var user = await _usersRepository.GetByIdAsync(id.ToString());

            if (user == null)
            {
                return;
            }

            user.Update(model.Email);

            await _usersRepository.UpdateAsync(user);

            foreach(var @event in user.Events)
            {
                var routingKey = "user-updated";

                _messageBus.Publish(
                    message: @event,
                    routingKey: routingKey,
                    exchange: "user-service"
                );
            }
        }
    }
}

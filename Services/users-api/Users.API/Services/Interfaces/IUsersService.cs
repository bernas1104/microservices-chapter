using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Users.API.Dtos.InputModels;
using Users.API.Dtos.ViewModels;

namespace Users.API.Services.Interfaces
{
    public interface IUsersService
    {
        Task<UserViewModel> CreateUser(UserInputModel model);
        Task UpdateUser(UserInputModel model, Guid id);
        Task<UserViewModel> GetUserById(Guid id);
        Task<IEnumerable<UserViewModel>> GetAllUsers();
    }
}

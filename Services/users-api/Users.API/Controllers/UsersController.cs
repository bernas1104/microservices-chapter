using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Users.API.Dtos.ViewModels;
using Users.API.Dtos.InputModels;
using Users.API.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Users.API.Controllers
{
    [ApiController]
    [Route("api/v1/{controller}")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost]
        public async Task<ActionResult<UserViewModel>> Create(
            [FromBody] UserInputModel model
        )
        {
            if (model == null)
            {
                return BadRequest();
            }

            var createdUser = await _usersService.CreateUser(model);

            if (createdUser == null)
            {
                return BadRequest();
            }

            return Created(
                $"api/v1/users/{createdUser.Id}",
                createdUser
            );
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserViewModel>> GetById(
            [FromRoute] Guid id
        )
        {
            var user = await _usersService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetAll()
        {
            return Ok(await _usersService.GetAllUsers());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(
            [FromBody] UserInputModel model,
            Guid id
        )
        {
            if (model == null || id == null)
            {
                return BadRequest();
            }

            await _usersService.UpdateUser(model, id);

            return NoContent();
        }
    }
}

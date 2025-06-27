using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.Presentation.Constants;
using RealEstate.Presentation.DTOs.User;

namespace RealEstate.Presentation.Controllers
{
    [Route(ApiRoutes.UsersEndpoint)]
    [ApiController]
    [Authorize]
    public class UserController(IUserService _userService, IValidator<CreateUserDto> _createUserValidator) : ControllerBase
    {
        [HttpGet]
        public async Task<List<UserDto>> GetAll(CancellationToken ct)
        {
            var userModels = await _userService.GetAllAsync(ct);
            var userDtos = userModels.Adapt<List<UserDto>>();
            return userDtos;
        }

        [HttpGet("{userId:guid}")]
        [AllowAnonymous]
        public async Task<UserDto> GetOne(Guid userId, CancellationToken ct)
        {
            var userModels = await _userService.GetByIdAsync(userId, ct);

            var userDto = userModels.Adapt<UserDto>();

            return userDto;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<UserDto> CreateUser([FromBody] CreateUserDto userForCreationDto, CancellationToken ct)
        {
            await _createUserValidator.ValidateAndThrowAsync(userForCreationDto, ct);

            var userModel = userForCreationDto.Adapt<UserModel>();

            var createdUserModel = await _userService.CreateAsync(userModel, ct);

            var createdUserDto = createdUserModel.Adapt<UserDto>();

            return createdUserDto;
        }

        [HttpDelete("{userId:guid}")]
        public async Task<StatusCodeResult> DeleteUser(Guid userId, CancellationToken ct)
        {
            await _userService.DeleteAsync(userId, ct);

            return StatusCode(204);
        }
    }
}

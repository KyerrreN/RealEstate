using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.BLL.Exceptions;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Models;
using RealEstate.Presentation.Constants;
using RealEstate.Presentation.DTOs;

namespace RealEstate.Presentation.Controllers
{
    [Route(ApiRoutes.UsersEndpoint)]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost]
        public async Task<UserDto> CreateUser([FromBody] UserForCreationDto userForCreationDto, CancellationToken ct)
        {
            var userModel = userForCreationDto.Adapt<UserModel>();

            var createdUserModel = await _userService.CreateAsync(userModel, ct);

            var createdUserDto = createdUserModel.Adapt<UserDto>();

            return createdUserDto;
        }
    }
}

using Insura.Media.Solusi.Common.Command;
using Insura.Media.Solusi.Common.Query;
using Insura.Media.Solusi.Service;
using Microsoft.AspNetCore.Mvc;

namespace Insura.Media.Solusi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public void CreateUser([FromBody] CreateUserCommand command)
        {
            userService.CreateUser(command);
        }

        [HttpGet]
        public IActionResult GetUserPage([FromQuery] UserQuery query) 
        {
            return Ok(userService.GetUsersByPage(query));
        }

        [HttpPut]
        [Route("{id}")]
        public void UpdateUser(int id, [FromBody] UpdateUserCommand command)
        {
            userService.UpdateUser(id, command);
        }

        [HttpDelete]
        [Route("{id}")]
        public void DeleteUser(int id)
        {
            userService.DeleteUser(id);
        }
    }
}

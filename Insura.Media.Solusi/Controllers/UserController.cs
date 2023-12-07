using Insura.Media.Solusi.Common.Command;
using Insura.Media.Solusi.Common.Query;
using Insura.Media.Solusi.Models;
using Insura.Media.Solusi.Service;
using Microsoft.AspNetCore.Mvc;
using RabbitQueue.Configuration;
using RabbitQueue.Service;

namespace Insura.Media.Solusi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IBus busControl;
        private readonly RabbitConnection rabbitConnection;

        public UserController(IUserService userService, IBus busControl, RabbitConnection rabbitConnection)
        {
            this.userService = userService;
            this.busControl = busControl;
            this.rabbitConnection = rabbitConnection;
        }

        [HttpPost]
        public void CreateUser([FromBody] CreateUserCommand command)
        {
            userService.CreateUser(command);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPage([FromQuery] UserQuery query) 
        {
            var response = userService.GetUsersByPage(query);
            if (response != null)
            {
                var logActivity = new CreateLogActivityCommand()
                {
                    Name = "System",
                    Activity = $"Get User Pagination on page {response.Page} of {response.TotalPages}",
                    Time = DateTime.Now,
                };

                await busControl.SendAsync(rabbitConnection.queueName, logActivity);
            }

            return Ok(response);
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

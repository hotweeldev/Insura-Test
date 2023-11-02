using Insura.Media.Solusi.Common.Command;
using Insura.Media.Solusi.Models;
using Insura.Media.Solusi.Service;
using Microsoft.AspNetCore.Mvc;

namespace Insura.Media.Solusi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserTaskController : ControllerBase
    {
        private readonly ITaskService taskService;
        public UserTaskController(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        [HttpPost]
        public void CreateUserTask(CreateTaskCommand command)
        {
            taskService.CreateTask(command);
        }

        [HttpGet]
        public ActionResult<List<UserTask>> GetTasks()
        {
            return Ok(taskService.GetAllUserTask());
        }
    }
}

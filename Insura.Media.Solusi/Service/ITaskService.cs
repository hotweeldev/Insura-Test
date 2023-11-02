using Insura.Media.Solusi.Common.Command;
using Insura.Media.Solusi.Models;

namespace Insura.Media.Solusi.Service
{
    public interface ITaskService
    {
        void CreateTask(CreateTaskCommand command);
        List<UserTask> GetAllUserTask();
    }
}

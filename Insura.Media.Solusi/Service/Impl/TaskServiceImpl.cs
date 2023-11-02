using AutoMapper;
using Insura.Media.Solusi.Common.Command;
using Insura.Media.Solusi.Exceptions;
using Insura.Media.Solusi.Models;
using Insura.Media.Solusi.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Insura.Media.Solusi.Service.Impl
{
    public class TaskServiceImpl : ITaskService
    {
        private const string UserTakCacheKey = "usertask";

        private readonly DataContext db;
        private readonly IMapper mapper;
        private readonly IMemoryCache memoryCache;

        public TaskServiceImpl(DataContext db, IMapper mapper, IMemoryCache memoryCache)
        {
            this.db = db;
            this.mapper = mapper;
            this.memoryCache = memoryCache;
        }

        public void CreateTask(CreateTaskCommand command)
        {
            var user = db.Users.FirstOrDefault(x => x.Id == command.UserId);
            if (user == null)
            {
                throw new NotFoundException("User not found!");
            }

            var task = mapper.Map<UserTask>(command);
            task.User = user;
            task.TaskProgress = Common.Enums.TaskProgress.New;
            task.TaskStart = DateTime.Now;
            db.UserTasks.Add(task);
            db.SaveChanges();
        }

        public List<UserTask> GetAllUserTask()
        {
            var options = new MemoryCacheEntryOptions()
              .SetSlidingExpiration(TimeSpan.FromSeconds(10))
              .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

            if (memoryCache.TryGetValue(UserTakCacheKey, out List<UserTask>? result))
                return result;

            var response = db.UserTasks.Include(task => task.User).ToList();
            memoryCache.Set(UserTakCacheKey, response, options);
            return response;
        }
    }
}

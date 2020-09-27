using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Todolist.Dtos;
using Todolist.Factories;
using Todolist.Models;

namespace Todolist.Services
{
    public class TasksService : ITasksService
    {
        private readonly IContextFactory _factory;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITasklistsService _tasklistsService;

        public TasksService(IContextFactory contextFactory, IMapper mapper, UserManager<IdentityUser> userManager, 
            IHttpContextAccessor httpContextAccessor, ITasklistsService tasklistsService)
        {
            _factory = contextFactory;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _tasklistsService = tasklistsService;
        }
        
        public List<TaskDto> GetTasklistTasksDto(int tasklistId)
        {
            var tasklist = _tasklistsService.GetTasklistData(tasklistId);

            if (tasklist == null)
            {
                return null;
            }
            
            return tasklist
                .Tasks
                .Select(x => new TaskDto
                {
                    Description = x.Description,
                    Id = x.Id,
                    Iscompleted = x.Iscompleted,
                    TasklistId = x.TasklistId
                })
                .ToList();
        }
        
        public List<TaskDto> GetTasklistTasksByUrlDto(string tasklistUrl)
        {
            var tasklist = _tasklistsService.GetTasklistByUrl(tasklistUrl);

            if (tasklist == null)
            {
                return null;
            }
            
            return tasklist
                .Tasks
                .Select(x => new TaskDto
                {
                    Description = x.Description,
                    Id = x.Id,
                    Iscompleted = x.Iscompleted,
                    TasklistId = x.TasklistId
                })
                .ToList();
        }

        public Task GetUserTaskData(int id)
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                return null;
            }
            
            var userId = _userManager.GetUserId(user);
            using (var context = _factory.GetContext())
            {
                return context
                    .Tasklists
                    .Where(x => x.User == userId)
                    .SelectMany(x => x.Tasks)
                    .FirstOrDefault(x => x.Id == id);
            }
        }
        
        public Task GetTaskData(int id)
        {            
            using (var context = _factory.GetContext())
            {
                return context
                    .Tasks
                    .FirstOrDefault(x => x.Id == id);
            }
        }

        public Task CreateTask(AddTaskDto newTask)
        {
            var task = _mapper.Map<Task>(newTask);
            using (var context = _factory.GetContext())
            {
                context.Tasks.Add(task);
                context.SaveChanges();

                return task;
            }
        }

        public void UpdateTask(Task task, UpdateTaskDto updatedTask)
        {
            _mapper.Map(updatedTask, task);
            using (var context = _factory.GetContext())
            {
                context.Tasks.Update(task);
                context.SaveChanges();
            }
        }
        
        public void DeleteTask(Task task)
        {
            using (var context = _factory.GetContext())
            {
                context.Tasks.Remove(task);
                context.SaveChanges();
            }
        }
    }
}
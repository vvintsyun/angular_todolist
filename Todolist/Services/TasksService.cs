using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Todolist.Dtos;
using Todolist.Models;

namespace Todolist.Services
{
    public class TasksService : ITasksService
    {
        private readonly Context _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITaskListsService _taskListsService;
        private readonly ILogger<ITasksService> _logger;

        public TasksService(IMapper mapper, UserManager<IdentityUser> userManager, Context dbContext,
            IHttpContextAccessor httpContextAccessor, ITaskListsService taskListsService, ILogger<ITasksService> logger)
        {
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _taskListsService = taskListsService;
            _logger = logger;
            _dbContext = dbContext;
        }
        
        public List<TaskDto> GetTaskTasksByTaskList(int taskListId)
        {
            //permission filters
            
            List<TaskDto> tasks;
            try
            {
                tasks = _dbContext
                    .Set<Task>()
                    .Where(x => x.TaskListId == taskListId)
                    .ProjectTo<TaskDto>()
                    .ToList();
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                throw;
            }

            return tasks;
        }
        
        public List<TaskDto> GetTaskListTasksByUrlDto(string taskListUrl)
        {
            var taskList = _taskListsService.GetTaskListByUrl(taskListUrl);

            if (taskList == null)
            {
                return null;
            }
            
            return taskList.Tasks;
        }
        
        public Task GetTask(int id, bool allowForAnonymous = false)
        {
            Task task;
            if (allowForAnonymous)
            {
                try
                {
                    task = _dbContext
                        .Set<Task>()
                        .FirstOrDefault(x => x.Id == id);
                }
                catch(Exception ex)
                {
                    _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                    throw;
                }
            }
            else
            {
                var user = _httpContextAccessor.HttpContext.User;
                if (!user.Identity.IsAuthenticated)
                {
                    return null;
                }
            
                var userId = _userManager.GetUserId(user);

                try
                {
                    task = _dbContext
                        .Set<Task>()
                        .Where(x => x.TaskList.UserId == userId)
                        .FirstOrDefault(x => x.Id == id);
                }
                catch(Exception ex)
                {
                    _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                    throw;
                }
            }

            return task;
        }

        public void CreateTask(AddTaskDto newTask)
        {
            var taskList = _taskListsService.GetTaskList(newTask.TaskListId);
            if (taskList == null)
            {
                throw new Exception("Task list doesn't exist.");
            }

            var createDto = new CreateTaskDto
            {
                Description = newTask.Description,
                IsCompleted = newTask.IsCompleted,
                TaskList = taskList
            };
            
            var task = new Task(createDto);
            try
            {
                _dbContext.Add(task);
                _dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                throw;
            }
        }

        public void UpdateTask(UpdateTaskDto updatedTask)
        {
            var task = GetTask(updatedTask.Id);
            _mapper.Map(updatedTask, task);
            
            try
            {
                _dbContext.Update(task);
                _dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                throw;
            }
        }
        
        public void DeleteTask(int id)
        {
            var task = GetTask(id);

            if (task == null)
            {
                throw new Exception("Task doesn't exist.");
            }
            
            try
            {
                _dbContext.Remove(task);
                _dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                throw;
            }
        }

        public void UpdateCompleted(UpdateTaskCompletedDto updatedCompleted)
        {
            var task = GetTask(updatedCompleted.Id, true);
            task.IsCompleted = updatedCompleted.IsCompleted;
            
            try
            {
                _dbContext.Update(task);
                _dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                throw;
            }
        }
    }
}
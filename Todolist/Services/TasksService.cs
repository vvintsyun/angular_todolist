using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Todolist.Dtos;
using Task = Todolist.Models.Task;

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
        
        public async Task<List<TaskDto>> GetTaskTasksByTaskList(int taskListId)
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                throw new Exception("User is not authenticated.");
            }
            var userId = _userManager.GetUserId(user);

            List<TaskDto> tasks;
            try
            {
                tasks = await _dbContext
                    .Set<Task>()
                    .Where(x => x.TaskListId == taskListId)
                    .Where(x => x.TaskList.UserId == userId)
                    .ProjectTo<TaskDto>()
                    .ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                throw;
            }

            return tasks;
        }
        
        public async Task<List<TaskDto>> GetTaskListTasksByUrlDto(string taskListUrl)
        {
            var taskList = await _taskListsService.GetTaskListByUrl(taskListUrl);

            return taskList?.Tasks;
        }
        
        public async Task<Task> GetTask(int id, bool allowForAnonymous = false)
        {
            Task task;
            if (allowForAnonymous)
            {
                try
                {
                    task = await _dbContext
                        .Set<Task>()
                        .FirstOrDefaultAsync(x => x.Id == id);
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
                    task = await _dbContext
                        .Set<Task>()
                        .Where(x => x.TaskList.UserId == userId)
                        .FirstOrDefaultAsync(x => x.Id == id);
                }
                catch(Exception ex)
                {
                    _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                    throw;
                }
            }

            return task;
        }

        public async System.Threading.Tasks.Task CreateTask(AddTaskDto newTask)
        {
            var taskList = await _taskListsService.GetTaskList(newTask.TaskListId);
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
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                throw;
            }
        }

        public async System.Threading.Tasks.Task UpdateTask(UpdateTaskDto updatedTask)
        {
            var task = await GetTask(updatedTask.Id);
            _mapper.Map(updatedTask, task);
            
            try
            {
                _dbContext.Update(task);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                throw;
            }
        }
        
        public async System.Threading.Tasks.Task DeleteTask(int id)
        {
            var task = await GetTask(id);

            if (task == null)
            {
                throw new Exception("Task doesn't exist.");
            }
            
            try
            {
                _dbContext.Remove(task);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                throw;
            }
        }

        public async System.Threading.Tasks.Task UpdateCompleted(UpdateTaskCompletedDto updatedCompleted)
        {
            var task = await GetTask(updatedCompleted.Id, true);
            task.IsCompleted = updatedCompleted.IsCompleted;
            
            try
            {
                _dbContext.Update(task);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                throw;
            }
        }
    }
}
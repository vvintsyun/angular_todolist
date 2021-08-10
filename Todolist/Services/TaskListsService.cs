using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Todolist.Dtos;
using Todolist.Extensions;
using Todolist.Models;
using Task = System.Threading.Tasks.Task;

namespace Todolist.Services
{
    public class TaskListsService : ITaskListsService
    {
        private readonly Context _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ITaskListsService> _logger;

        public TaskListsService(IMapper mapper, UserManager<IdentityUser> userManager, 
            IHttpContextAccessor httpContextAccessor, Context dbContext, ILogger<ITaskListsService> logger)
        {
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public IEnumerable<TaskListDto> GetUserTaskLists()
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                throw new Exception("User is not authenticated.");
            }
            
            var userId = _userManager.GetUserId(user);
            List<TaskListDto> taskLists;
            try
            {
                taskLists = _dbContext
                    .Set<TaskList>()
                    .Where(tl => tl.UserId == userId)
                    .ProjectTo<TaskListDto>()
                    .ToList();
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                throw;
            }

            return taskLists;
        }

        public TaskListDto GetTaskListByUrl(string url)
        {
            var id = url.DecodeLongByHashIds();
            TaskListDto taskList;
            try
            {
                taskList = _dbContext
                    .Set<TaskList>()
                    .ProjectTo<TaskListDto>()
                    .FirstOrDefault(tl => tl.Id == id);
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                throw;
            }
            
            return taskList;
        }

        public TaskListDto GetTaskListData(int id)
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                throw new Exception("User is not authenticated.");
            }
            
            var userId = _userManager.GetUserId(user);
            
            TaskListDto taskList;
            try
            {
                taskList = _dbContext
                    .Set<TaskList>()
                    .Where(x => x.UserId == userId)
                    .ProjectTo<TaskListDto>()
                    .FirstOrDefault(x => x.Id == id);
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                throw;
            }
            
            return taskList;
        }

        public void UpdateTaskList(UpdateTaskListDto updatedTaskList)
        {
            var taskList = GetTaskList(updatedTaskList.Id);
            if (taskList == null)
            {
                throw new Exception("Task list doesn't exist.");
            }
            
            _mapper.Map(updatedTaskList, taskList);
            
            try
            {
                _dbContext.Update(taskList);
                _dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                throw;
            }
        }

        //[Authorize] ????
        public async Task CreateTaskList(AddTaskListDto newTaskList)
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                throw new Exception("User is not authenticated.");
            }
            
            newTaskList.User = await _userManager.GetUserAsync(user);
            var taskList = new TaskList(newTaskList);
            try
            {
                _dbContext.Add(taskList);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                throw;
            }
        }

        public void DeleteTaskList(int id)
        {
            try
            {
                var taskList = GetTaskList(id);

                if (taskList == null)
                {
                    throw new Exception("Task list doesn't exist.");
                }
                
                _dbContext.Remove(taskList);
                _dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                throw;
            }
        }

        public string GetTaskListUrl(int id)
        {
            var taskList = GetTaskList(id);

            return taskList?.Id.EncryptId();
        }
        
        public TaskList GetTaskList(int id)
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                throw new Exception("User is not authenticated.");
            }
            
            var userId = _userManager.GetUserId(user);
            
            TaskList taskList;
            try
            {
                taskList = _dbContext
                    .Set<TaskList>()
                    .Where(x => x.UserId == userId)
                    .FirstOrDefault(x => x.Id == id);
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                throw;
            }

            return taskList;
        }
    }
}
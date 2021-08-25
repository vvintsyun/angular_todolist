using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        
        public async Task<IEnumerable<TaskListDto>> GetUserTaskLists(CancellationToken ct)
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                throw new Exception("User is not authenticated.");
            }
            
            var userId = _userManager.GetUserId(user);
            TaskListDto[] taskLists;
            try
            {
                taskLists = await _dbContext
                    .Set<TaskList>()
                    .Where(tl => tl.UserId == userId)
                    .ProjectTo<TaskListDto>()
                    .ToArrayAsync(ct);
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                throw;
            }

            return taskLists;
        }

        public async Task<TaskListDto> GetTaskListByUrl(string url, CancellationToken ct)
        {
            var id = url.DecodeLongByHashIds();
            TaskListDto taskList;
            try
            {
                taskList = await _dbContext
                    .Set<TaskList>()
                    .ProjectTo<TaskListDto>()
                    .FirstOrDefaultAsync(tl => tl.Id == id, ct);
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                throw;
            }
            
            return taskList;
        }

        public async Task<TaskListDto> GetTaskListData(int id, CancellationToken ct)
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
                taskList = await _dbContext
                    .Set<TaskList>()
                    .Where(x => x.UserId == userId)
                    .ProjectTo<TaskListDto>()
                    .FirstOrDefaultAsync(x => x.Id == id, ct);
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                throw;
            }
            
            return taskList;
        }

        public async Task UpdateTaskList(UpdateTaskListDto updatedTaskList, CancellationToken ct)
        {
            var taskList = await GetTaskList(updatedTaskList.Id, ct);
            if (taskList == null)
            {
                throw new Exception("Task list doesn't exist.");
            }
            
            _mapper.Map(updatedTaskList, taskList);
            
            try
            {
                _dbContext.Update(taskList);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                throw;
            }
        }

        public async Task CreateTaskList(AddTaskListDto newTaskList, CancellationToken ct)
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
                await _dbContext.SaveChangesAsync(ct);
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteTaskList(int id, CancellationToken ct)
        {
            try
            {
                var taskList = await GetTaskList(id, ct);

                if (taskList == null)
                {
                    throw new Exception("Task list doesn't exist.");
                }
                
                _dbContext.Remove(taskList);
                await _dbContext.SaveChangesAsync(ct);
            }
            catch(Exception ex)
            {
                _logger.LogError($"{DateTime.Now:g}: {ex.Message}");
                throw;
            }
        }

        public async Task<string> GetTaskListUrl(int id, CancellationToken ct)
        {
            var taskList = await GetTaskList(id, ct);

            return taskList?.Id.EncryptId();
        }
        
        public async Task<TaskList> GetTaskList(int id, CancellationToken ct)
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
                taskList = await _dbContext
                    .Set<TaskList>()
                    .Where(x => x.UserId == userId)
                    .FirstOrDefaultAsync(x => x.Id == id, ct);
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
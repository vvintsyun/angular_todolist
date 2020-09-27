using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Todolist.Dtos;
using Todolist.Extensions;
using Todolist.Factories;
using Todolist.Models;

namespace Todolist.Services
{
    public class TasklistsService : ITasklistsService
    {
        private readonly IContextFactory _factory;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TasklistsService(IContextFactory contextFactory, IMapper mapper, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _factory = contextFactory;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<Tasklist> GetUserTasklists()
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
                    .Include(x => x.Tasks)
                    .Where(x => x.User == userId)
                    .ToList();
            }
        }
        
        public List<TasklistDto> GetUserTasklistsDto()
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
                    .Where(tl => tl.User == userId)
                    .ProjectTo<TasklistDto>()
                    .ToList();
            }
        }

        public Tasklist GetTasklistByUrl(string url)
        {
            var id = url.DecodeLongByHashIds();
            using (var context = _factory.GetContext())
            {
                return context
                    .Tasklists
                    .Include(x => x.Tasks)
                    .FirstOrDefault(tl => tl.Id == id);
            }
        }

        public Tasklist GetTasklistData(int id)
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
                    .Include(x => x.Tasks)
                    .Where(x => x.User == userId)
                    .FirstOrDefault(x => x.Id == id);
            }
        }

        public void UpdateTasklist(Tasklist tasklist, UpdateTasklistDto updatedTasklist)
        {
            _mapper.Map(updatedTasklist, tasklist);
            using (var context = _factory.GetContext())
            {
                context.Tasklists.Update(tasklist);
                context.SaveChanges();
            }
        }

        public Tasklist CreateTasklist(AddTasklistDto newTasklist)
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                return null;
            }
            
            var tasklist = _mapper.Map<Tasklist>(newTasklist);
            tasklist.User = _userManager.GetUserId(user);
            using (var context = _factory.GetContext())
            {
                context.Tasklists.Add(tasklist);
                context.SaveChanges();

                return tasklist;
            }
        }

        public void DeleteTasklist(Tasklist tasklist)
        {
            using (var context = _factory.GetContext())
            {
                context.Tasklists.Remove(tasklist);
                context.SaveChanges();
            }
        }

        public string GetTasklistUrl(int id)
        {
            var tasklist = GetTasklistData(id);

            if (tasklist == null)
            {
                return null;
            }

            return tasklist.Id.EncryptId();
        }
    }
}
using AutoMapper;
using Todolist.Dtos;
using Todolist.Models;

namespace Todolist.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<AddTaskListDto, TaskList>();
            CreateMap<AddTaskDto, Task>();
            CreateMap<UpdateTaskListDto, TaskList>();
            CreateMap<UpdateTaskDto, Task>();
            
            CreateMap<Task, TaskDto>();
            CreateMap<TaskList, TaskListDto>()
                .ForMember(x => x.Tasks, x => x.MapFrom(xx => xx.Tasks));
        }
    }
}
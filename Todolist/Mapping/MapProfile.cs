using AutoMapper;
using Todolist.Dtos;
using Todolist.Models;

namespace Todolist.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<AddTasklistDto, Tasklist>();
            CreateMap<AddTaskDto, Task>();
            CreateMap<UpdateTasklistDto, Tasklist>();
            CreateMap<UpdateTaskDto, Task>();
        }
    }
}
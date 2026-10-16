using AutoMapper;
using TaskApi.DTOs;
using TaskApi.Models;

namespace TaskApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
           
            CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.TasksCount,
                    opt => opt.MapFrom(src => src.Tasks.Count));

            CreateMap<CreateProjectDto, Project>();

            CreateMap<UpdateProjectDto, Project>();

            CreateMap<Executor, ExecutorDto>()
                .ForMember(dest => dest.TasksCount,
                    opt => opt.MapFrom(src => src.Tasks.Count));

            CreateMap<CreateExecutorDto, Executor>();

            CreateMap<UpdateExecutorDto, Executor>();

            CreateMap<TaskItem, TaskItemDto>()
                .ForMember(dest => dest.ProjectName,
                    opt => opt.MapFrom(src => src.Project != null ? src.Project.Name : null))
                .ForMember(dest => dest.ExecutorName,
                    opt => opt.MapFrom(src => src.Executor != null ? src.Executor.Name : null));

            CreateMap<CreateTaskItemDto, TaskItem>();

            CreateMap<UpdateTaskItemDto, TaskItem>();
        }
    }
}
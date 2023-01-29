using AutoMapper;
using SleekFlowTodoListCore.Domain.Database.Todos;

namespace SleekFlowTodoListAPI.Controllers.Todos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Index
            CreateMap<Todo, Index.Model>()
                .ForMember(c => c.Status, opt => opt.MapFrom(src => ((TodoStatusEnum)src.Status).ToString()));

            // Details
            CreateMap<Todo, Details.Model>()
                .ForMember(c => c.Status, opt => opt.MapFrom(src => ((TodoStatusEnum)src.Status).ToString()));

            // Create
            CreateMap<Create.Request, Todo>(MemberList.Source);
            CreateMap<Todo, Create.Model>()
                .ForMember(c => c.Status, opt => opt.MapFrom(src => ((TodoStatusEnum)src.Status).ToString()));

            // Delete
            CreateMap<Todo, Delete.Model>()
                .ForMember(c => c.Status, opt => opt.MapFrom(src => ((TodoStatusEnum)src.Status).ToString()));

            // Update
            CreateMap<Update.Request, Todo>(MemberList.Source);
            CreateMap<Todo, Update.Model>()
                .ForMember(c => c.Status, opt => opt.MapFrom(src => ((TodoStatusEnum)src.Status).ToString()));
        }
    }
}

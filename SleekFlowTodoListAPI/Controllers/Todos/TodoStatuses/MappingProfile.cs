using AutoMapper;

namespace SleekFlowTodoListAPI.Controllers.Todos.TodoStatuses
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Index
            CreateMap<KeyValuePair<int, string>, Index.Model>()
                .ConstructUsing(x => new Index.Model { Code = x.Key, Name = x.Value });
        }
    }
}

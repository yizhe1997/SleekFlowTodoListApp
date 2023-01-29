using AutoMapper;
using SleekFlowTodoListCore.Domain.Database.Users;

namespace SleekFlowTodoListAPI.Controllers.Users
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			// Index
			CreateMap<User, Index.Model>();

			// Details
			CreateMap<User, Details.Model>();

			// Create
			CreateMap<Create.Request, User>(MemberList.Source)
				.ForMember(c => c.EmailConfirmed, opt => opt.MapFrom(src => true));
			CreateMap<User, Create.Model>();

			// Delete
			CreateMap<User, Delete.Model>();

			// Update
			CreateMap<Update.Request, User>(MemberList.Source);
			CreateMap<User, Update.Model>();
		}
	}
}

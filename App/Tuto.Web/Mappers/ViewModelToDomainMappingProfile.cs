using AutoMapper;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.ViewModels;
using Tuto.Web.ViewModels.Account;
using Tuto.Web.ViewModels.Account.Register;

namespace Tuto.Web.Mappers
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<TutorRegisterViewModel, Tutor>();

            Mapper.CreateMap<HelpedRegisterViewModel, Helped>();

            Mapper.CreateMap<UserLoginViewModel, User>();

            Mapper.CreateMap<User, UserLoginViewModel>();

            // HELP REQUESTS
            Mapper.CreateMap<HelpRequestAddViewModel, HelpRequest>();
            Mapper.CreateMap<HelpRequestListDetailsViewModel, HelpRequest>();

            // INDIVIDUAL SESSION
            Mapper.CreateMap<IndividualSessionViewModel, IndividualSession>();

            // TUTOR LIST
            Mapper.CreateMap<TutorListViewModel, Tutor>();
        }
    }
}
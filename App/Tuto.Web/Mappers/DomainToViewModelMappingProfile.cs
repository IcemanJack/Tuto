using System.Linq;
using AutoMapper;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.Models.GroupSessions;
using Tuto.DataLayer.Models.Notifications.Manager;
using Tuto.DataLayer.Models.Notifications.Shared;
using Tuto.DataLayer.Models.Notifications.Tutor;
using Tuto.DataLayer.Models.Users;
using Tuto.DataLayer.ModelUtilities;
using Tuto.Web.Controllers.Manager.Reports;
using Tuto.Web.ViewModels.Account;
using Tuto.Web.ViewModels.Account.Edit;
using Tuto.Web.ViewModels.Account.Register;
using Tuto.Web.ViewModels.GroupSession;
using Tuto.Web.ViewModels.HelpRequest;
using Tuto.Web.ViewModels.HelpRequestMgr;
using Tuto.Web.ViewModels.Notifications;
using Tuto.Web.ViewModels.Reports;
using Tuto.Web.ViewModels.TutorsMgr;

namespace Tuto.Web.Mappers
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            // REGISTER
            Mapper.CreateMap<Tutor, TutorRegisterViewModel>();

            Mapper.CreateMap<Helped, HelpedRegisterViewModel>();

            // LOGIN
            Mapper.CreateMap<User, UserLoginViewModel>();

            // EDIT
            Mapper.CreateMap<Helped, EditHelpedViewModel>()
                .ForMember(t => t.newName, opt => opt.MapFrom(h => h.name))
                .ForMember(t => t.newLastName, opt => opt.MapFrom(h => h.lastName))
                .ForMember(t => t.jsonSchedule, opt => opt.MapFrom(h => ScheduleUtilities.getJsonFromScheduleBlocks(h.scheduleBlocks)));

            Mapper.CreateMap<Tutor, EditTutorViewModel>()
                .ForMember(t => t.newName, opt => opt.MapFrom(h => h.name))
                .ForMember(t => t.newLastName, opt => opt.MapFrom(h => h.lastName))
                .ForMember(t => t.jsonSchedule, opt => opt.MapFrom(h => ScheduleUtilities.getJsonFromScheduleBlocks(h.scheduleBlocks)));

            Mapper.CreateMap<Manager, EditManagerViewModel>()
                .ForMember(t => t.newName, opt => opt.MapFrom(h => h.name))
                .ForMember(t => t.newLastName, opt => opt.MapFrom(h => h.lastName));

            // HELP REQUESTS
            Mapper.CreateMap<HelpRequest, HelpRequestListDetailsViewModel>();

            Mapper.CreateMap<HelpRequest, TutorHelpRequestListEntryViewModel>()
                .ForMember(t => t.currentState, opt => opt.MapFrom(h => h.getState()))
                .ForMember(t => t.expectedDate, opt => opt.MapFrom(h => h.individualSession.date))
                .ForMember(t => t.helpedFullname, opt => opt.MapFrom(h => h.helped.name + " " + h.helped.lastName))
                .ForMember(t => t.place, opt => opt.MapFrom(h => h.individualSession.place))
                .ForMember(t => t.course, opt => opt.MapFrom(h => h.course.courseName))
                .ForMember(t => t.reportIsEmpty, opt => opt.MapFrom(h => h.individualSession.tutorMessage == null))
                .ForMember(t => t.tutorHasConfirmed, opt => opt.MapFrom(h => h.tutorHasConfirmed));

            Mapper.CreateMap<HelpRequest, ManagerHelpRequestsListViewModel.ToBeConfirmedHelpRequestViewModel>()
                .ForMember(t => t.courseName, opt => opt.MapFrom(h => h.course.courseName))
                .ForMember(t => t.helpedFirstName, opt => opt.MapFrom(h => h.helped.name))
                .ForMember(t => t.helpedLastName, opt => opt.MapFrom(h => h.helped.lastName));

            Mapper.CreateMap<HelpRequest, ManagerHelpRequestsListViewModel.ConfirmedHelpRequestViewModel>()
                .ForMember(t => t.courseName, opt => opt.MapFrom(h => h.course.courseName))
                .ForMember(t => t.helpedFirstName, opt => opt.MapFrom(h => h.helped.name))
                .ForMember(t => t.helpedLastName, opt => opt.MapFrom(h => h.helped.lastName))
                .ForMember(t => t.individualSessionDate, opt => opt.MapFrom(h => h.individualSession.date))
                .ForMember(t => t.tutorFirstName, opt => opt.MapFrom(h => h.tutor.name))
                .ForMember(t => t.tutorLastName, opt => opt.MapFrom(h => h.tutor.lastName))
                .ForMember(t => t.individualSessionPlace, opt => opt.MapFrom(h => h.individualSession.place));

            Mapper.CreateMap<HelpRequest, ManagerHelpRequestsListViewModel.FinishedHelpRequestViewModel>()
                .ForMember(t => t.courseName, opt => opt.MapFrom(h => h.course.courseName))
                .ForMember(t => t.helpedFirstName, opt => opt.MapFrom(h => h.helped.name))
                .ForMember(t => t.helpedLastName, opt => opt.MapFrom(h => h.helped.lastName))
                .ForMember(t => t.individualSessionDate, opt => opt.MapFrom(h => h.individualSession.date))
                .ForMember(t => t.tutorFirstName, opt => opt.MapFrom(h => h.tutor.name))
                .ForMember(t => t.tutorLastName, opt => opt.MapFrom(h => h.tutor.lastName))
                .ForMember(t => t.individualSessionPlace, opt => opt.MapFrom(h => h.individualSession.place))
                .ForMember(t => t.individualSessionId, opt =>opt.MapFrom(h => h.individualSession.id))
                .ForMember(t => t.tutorRepportMessage, opt => opt.MapFrom(h => h.individualSession.tutorMessage))
                .ForMember(t => t.helpedRepportMessage, opt => opt.MapFrom(h => h.individualSession.helpedMessage));
                
            Mapper.CreateMap<Tutor, ManagerHelpRequestTutorAssignmentViewModels.TutorAvailableForGivenBlockViewModel>()
                .ForMember(t => t.firstName, opt => opt.MapFrom(t => t.name))
                .ForMember(t => t.lastName, opt => opt.MapFrom(t => t.lastName))
                .ForMember(t => t.tutorUserId, opt => opt.MapFrom(t => t.id))
                .ForMember(t => t.monthlyWorkingHours, opt => opt.MapFrom(t => t.calculateMonthlyWorkedHours()));

            Mapper.CreateMap<HelpRequest, HelpedHelpRequestListViewModel.HelpRequestViewModel>()
                .ForMember(t => t.currentState, opt => opt.MapFrom(h => h.getState()))
                .ForMember(t => t.courseName, opt => opt.MapFrom(h => h.course.courseName))
                .ForMember(t => t.createdTime, opt => opt.MapFrom(h => h.createdTime))
                .ForMember(t => t.reportIsEmpty, opt => opt.MapFrom(h => h.individualSession.helpedMessage == null));

            Mapper.CreateMap<HelpRequest, HelpedHelpRequestListViewModel.AssignedRequestViewModel>()
                .ForMember(t => t.currentState, opt => opt.MapFrom(h => h.getState()))
                .ForMember(t => t.helpedHasConfirmed, opt => opt.MapFrom(h => h.helpedHasConfirmed))
                .ForMember(t => t.courseName, opt => opt.MapFrom(h => h.course.courseName))
                .ForMember(t => t.createdTime, opt => opt.MapFrom(h => h.createdTime))
                .ForMember(t => t.individualSessionDate, opt => opt.MapFrom(h => h.individualSession.date))
                .ForMember(t => t.tutorFirstName, opt => opt.MapFrom(h => h.tutor.name))
                .ForMember(t => t.tutorLastName, opt => opt.MapFrom(h => h.tutor.lastName))
                .ForMember(t => t.individualSessionPlace, opt => opt.MapFrom(h => h.individualSession.place));

            Mapper.CreateMap<HelpRequest, ManagerHelpRequestTutorAssignmentViewModels.AssignHelpRequestViewModel>()
                .ForMember(t => t.helpedFirstName, opt => opt.MapFrom(h => h.helped.name))
                .ForMember(t => t.helpedLastName, opt => opt.MapFrom(h => h.helped.lastName))
                .ForMember(t => t.helpRequestCreatedTime, opt => opt.MapFrom(h => h.createdTime))
                .ForMember(t => t.helpRequestState, opt => opt.MapFrom(h => h.currentState))
                .ForMember(t => t.helpRequestScheduleJson, opt => opt.MapFrom(h => h.scheduleJson))
                .ForMember(t => t.helpRequestCourseName, opt => opt.MapFrom(h => h.course.courseName))
                .ForMember(t => t.helpRequestComment, opt => opt.MapFrom(h => h.comment))
                .ForMember(t => t.helpRequestMisunderstoodNotions, opt => opt.MapFrom(h => h.misunderstoodNotions))
                .ForMember(t => t.helpRequestId, opt => opt.MapFrom(h => h.id));

            Mapper
                .CreateMap<Tutor, ManagerHelpRequestTutorAssignmentViewModels.AssignHelpRequestViewModel.TutorListEntryViewModel>()
                .ForMember(t => t.firstName, opt => opt.MapFrom(t => t.name))
                .ForMember(t => t.monthlyWorkingHours, opt => opt.MapFrom(t => t.calculateMonthlyWorkedHours()))
                .ForMember(t => t.skilledCourses, opt => opt.MapFrom(t => t.getCoursesSkillsString()));

            // GROUP SESSION
            Mapper.CreateMap<Tutor, GroupSessionAssignTutorViewModel>()
                .ForMember(t => t.name, opt => opt.MapFrom(h => h.name + " " + h.lastName))
                .ForMember(t => t.workedHours, opt => opt.MapFrom(h => h.calculateMonthlyWorkedHours()))
                .ForMember(t => t.id, opt => opt.MapFrom(h => h.id));

            Mapper.CreateMap<AssignedGroupSession, GroupSessionListViewModel>()
                .ForMember(t => t.groupSessionDate, opt => opt.MapFrom(h => h.getDate()))
                .ForMember(t => t.weekDayStr, opt => opt.MapFrom(h => h.startScheduleBlock.weekDay.ToString()))
                .ForMember(t => t.startTime, opt => opt.MapFrom(h => h.startScheduleBlock.startTime))
                .ForMember(t => t.endTime, opt => opt.MapFrom(h => (h.startScheduleBlock.startTime + 2)));

            Mapper.CreateMap<AssignedGroupSession, GroupSessionListViewModel.ManagerViewModel>()
                .ForMember(t => t.groupSessionDate, opt => opt.MapFrom(h => h.getDate()))
                .ForMember(t => t.weekDayStr, opt => opt.MapFrom(h => h.startScheduleBlock.weekDay.ToString()))
                .ForMember(t => t.startTime, opt => opt.MapFrom(h => h.startScheduleBlock.startTime))
                .ForMember(t => t.endTime, opt => opt.MapFrom(h => (h.startScheduleBlock.startTime + 2)))
                .ForMember(t => t.hasAssignedTutor, opt => opt.MapFrom(h => h.tutor != null))
                .ForMember(t => t.tutorFirstName, opt => opt.MapFrom(h => h.tutor != null ? h.tutor.name : ""))
                .ForMember(t => t.tutorLastName, opt => opt.MapFrom(h => h.tutor != null ? h.tutor.lastName : ""));

            // INDIVIDUAL SESSION
            Mapper.CreateMap<IndividualSession, IndividualSessionViewModel>()
                .ForMember(t => t.helpedLastName, opt => opt.MapFrom(h => h.helpRequest.helped.lastName))
                .ForMember(t => t.helpedName, opt => opt.MapFrom(h => h.helpRequest.helped.name))
                .ForMember(t => t.tutorLastName, opt => opt.MapFrom(h => h.helpRequest.tutor.lastName))
                .ForMember(t => t.tutorName, opt => opt.MapFrom(h => h.helpRequest.tutor.name));

            // TUTORLIST
            Mapper.CreateMap<Tutor, TutorListViewModel>()
                .ForMember(t => t.lastName, opt => opt.MapFrom(h => h.lastName))
                .ForMember(t => t.name, opt => opt.MapFrom(h => h.name))
                .ForMember(t => t.mail, opt => opt.MapFrom(h => h.mail));

            // TUTOR DETAILS
            Mapper.CreateMap<Tutor, TutorDetailsViewModel>()
                .ForMember(t => t.id, opt => opt.MapFrom(h => h.id))
                .ForMember(t => t.lastName, opt => opt.MapFrom(h => h.lastName))
                .ForMember(t => t.name, opt => opt.MapFrom(h => h.name))
                .ForMember(t => t.mail, opt => opt.MapFrom(h => h.mail))
                .ForMember(t => t.tutorAvailableForGroupSession, opt => opt.MapFrom(h => h.tutorAvailableForGroupSession))
                .ForMember(t => t.tutorAvailableForIndividualSession, opt => opt.MapFrom(h => h.tutorAvailableForIndividualSession))
                .ForMember(t => t.workedHours, opt => opt.MapFrom(h => h.calculateMonthlyWorkedHours()))
                .ForMember(t => t.tutorState, opt => opt.MapFrom(h => h.tutorState))
                .ForMember(t => t.coursesSkills, opt => opt.MapFrom(h => h.coursesSkills))
                .ForMember(t => t.groupSessions, opt => opt.MapFrom(h => h.groupSessions))
                .ForMember(t => t.helpRequests, opt => opt.MapFrom(h => h.helpRequests))
                .ForMember(t => t.jsonSchedule, opt => opt.MapFrom(h => ScheduleUtilities.getJsonFromScheduleBlocks(h.scheduleBlocks)));

            // HOME
            Mapper.CreateMap<User, HomeViewModel>()
                .ForMember(hv => hv.userFirstName, opt => opt.MapFrom(u => u.name))
                .ForMember(hv => hv.userLastName, opt => opt.MapFrom(u => u.lastName));

            // NOTIFICATIONS
            Mapper.CreateMap<AssignedToHelpRequestTask, AssignedToHelpRequestTaskViewModel.HelpedViewModel>()
                .ForMember(t => t.helpRequestId, opt => opt.MapFrom(h => h.concernedHelpRequest.id))
                .ForMember(t => t.individualSessionDatetime, opt => opt.MapFrom(h => h.concernedHelpRequest.individualSession.date))
                .ForMember(t => t.helpRequestCreatedDatetime, opt => opt.MapFrom(h => h.concernedHelpRequest.createdTime));

            Mapper.CreateMap<AssignedToHelpRequestTask, AssignedToHelpRequestTaskViewModel.TutorViewModel>()
                .ForMember(t => t.helpRequestId, opt => opt.MapFrom(h => h.concernedHelpRequest.id))
                .ForMember(t => t.individualSessionDatetime, opt => opt.MapFrom(h => h.concernedHelpRequest.individualSession.date))
                .ForMember(t => t.helpRequestCreatedDatetime, opt => opt.MapFrom(h => h.concernedHelpRequest.createdTime))
                .ForMember(t => t.helpedFirstname, opt => opt.MapFrom(h => h.concernedHelpRequest.helped.name))
                .ForMember(t => t.helpedLastname, opt => opt.MapFrom(h => h.concernedHelpRequest.helped.lastName));

            Mapper.CreateMap<AssignedToGroupSessionAlert, AssignedToGroupSessionAlertViewModel.TutorViewModel>()
                .ForMember(t => t.groupSessionPlannedDate, opt => opt.MapFrom(h => h.concernedGroupSession.getDate().
                                                                                    AddDays((double)h.concernedGroupSession.startScheduleBlock.weekDay)
                                                                                    .AddHours(h.concernedGroupSession.startScheduleBlock.startTime)))
                .ForMember(t => t.groupSessionPlace, opt => opt.MapFrom(h => h.concernedGroupSession.place));

            Mapper.CreateMap<TutorHasRegisteredTask, TutorHasRegisteredTaskViewModel.ManagerViewModel>()
                .ForMember(t => t.tutorId, opt => opt.MapFrom(h => h.registeredTutor.id))
                .ForMember(t => t.tutorFirstName, opt => opt.MapFrom(h => h.registeredTutor.name))
                .ForMember(t => t.tutorLastName, opt => opt.MapFrom(h => h.registeredTutor.lastName));

            Mapper.CreateMap<HelpRequestToAssignAlert, HelpRequestToAssignAlertViewModel.ManagerViewModel>()
                .ForMember(t => t.helpRequestId, opt => opt.MapFrom(h => h.helpRequest.id))
                .ForMember(t => t.helpedFirstName, opt => opt.MapFrom(h => h.helpRequest.helped.name))
                .ForMember(t => t.helpedLastName, opt => opt.MapFrom(h => h.helpRequest.helped.lastName));

            Mapper.CreateMap<HelpedHasRegisteredAlert, HelpedHasRegisteredAlertViewModel.ManagerViewModel>()
                .ForMember(t => t.helpedFirstName, opt => opt.MapFrom(h => h.helpedUser.name))
                .ForMember(t => t.helpedLastName, opt => opt.MapFrom(h => h.helpedUser.lastName));

            // REPORTS
            Mapper.CreateMap<MonthlyWorkedHoursReport, MonthlyWorkedHoursReportViewModel>() // TODO : Refactor!!
                .ForMember(t => t.columnsTitle, opt => opt.MapFrom(h => h.getColumnsTitle()))
                .ForMember(t => t.tableData, opt => opt.MapFrom(h => h.ToList()));

            Mapper.CreateMap<Tutor, MonthlyWorkedHoursReportEntry>()
                .ForMember(t => t.tutorFirstName, opt => opt.MapFrom(h => h.name))
                .ForMember(t => t.tutorLastName, opt => opt.MapFrom(h => h.lastName))
                .ForMember(t => t.workedHours, opt => opt.MapFrom(h => h.calculateMonthlyWorkedHours()));
        }
    }
}
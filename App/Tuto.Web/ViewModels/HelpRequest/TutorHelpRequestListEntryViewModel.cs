using System;
using System.ComponentModel.DataAnnotations;
using Tuto.DataLayer.Enums;

namespace Tuto.Web.ViewModels.HelpRequest
{
    public class TutorHelpRequestListEntryViewModel
    {
        public int id { get; set; }

        public HelpRequestState currentState { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayTutorHelpRequestExpectedDate")]
        [DisplayFormat(DataFormatString = "{0:d/M/yyyy HH:mm:ss}")]
        public DateTime expectedDate { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayTutorHelpRequestEntryPlace")]
        public String place { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayTutorHelpRequestEntryHelpedFullName")]
        public String helpedFullname { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayTutorHelpRequestCourse")]
        public String course { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayHelpRequestDateTime")]
        [DisplayFormat(DataFormatString = "{0:d/M/yyyy HH:mm:ss}")]
        public DateTime createdTime { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayHelpRequestReport")]
        public bool reportIsEmpty { get; set; }

        public bool tutorHasConfirmed { get; set; }

        public string getStateString()
        {
            switch (this.currentState)
            {
                case HelpRequestState.NOT_ASSIGNED:
                    return Resources.Resources.StateStringNotAssigned;
                case HelpRequestState.TO_BE_CONFIRMED:
                    return Resources.Resources.StateStringToBeConfirmed;
                case HelpRequestState.CONFIRMED:
                    return Resources.Resources.StateStringConfirmed;
                case HelpRequestState.FINISHED:
                    return Resources.Resources.StateStringFinished;
                case HelpRequestState.BILAN:
                    return Resources.Resources.StateStringBilan;
            }

            return "";
        }
    }
}
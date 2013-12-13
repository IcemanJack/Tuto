using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tuto.DataLayer.Enums;

namespace Tuto.Web.ViewModels.HelpRequest
{
    public class HelpedHelpRequestListViewModel
    {
        public List<AssignedRequestViewModel> assignedHelpRequests { get; set; }
        public List<HelpRequestViewModel> allHelpRequests { get; set; }

        public class AssignedRequestViewModel : HelpRequestViewModel
        {
            public string tutorFirstName { get; set; }
            public string tutorLastName { get; set; }

            [DisplayFormat(DataFormatString = "{0:d/M/yyyy HH:mm:ss}")]
            public DateTime individualSessionDate { get; set; }

            public string individualSessionPlace { get; set; }

            public bool helpedHasConfirmed { get; set; }
        }

        public class HelpRequestViewModel
        {
            public int id { get; set; }

            [DisplayFormat(DataFormatString = "{0:d/M/yyyy HH:mm:ss}")]
            public DateTime createdTime { get; set; }

            public string courseName { get; set; }

            public HelpRequestState currentState { get; set; }

            public bool reportIsEmpty { get; set; }
        }
    }
}
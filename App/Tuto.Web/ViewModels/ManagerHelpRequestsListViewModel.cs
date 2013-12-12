using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tuto.Web.ViewModels
{
    public class ManagerHelpRequestsListViewModel
    {
        public List<ToBeConfirmedHelpRequestViewModel> toBeConfirmedSystemHelpRequests { get; set; }
        public List<ConfirmedHelpRequestViewModel> confirmedSystemHelpRequests { get; set; }
        public List<FinishedHelpRequestViewModel> finishedSystemHelpRequests { get; set; }

        public class ConfirmedHelpRequestViewModel
        {
            public int id;
            public string helpedFirstName { get; set; }
            public string helpedLastName { get; set; }

            public string tutorFirstName { get; set; }
            public string tutorLastName { get; set; }

            public bool helpedHasConfirmed { get; set; }
            public bool tutorHasConfirmed { get; set; }

            [DisplayFormat(DataFormatString = "{0:d/M/yyyy HH:mm:ss}")]
            public DateTime createdTime { get; set; }

            [DisplayFormat(DataFormatString = "{0:d/M/yyyy HH:mm:ss}")]
            public DateTime individualSessionDate { get; set; }

            public string individualSessionPlace { get; set; }

            public string courseName { get; set; }
        }

        public class FinishedHelpRequestViewModel : ConfirmedHelpRequestViewModel
        {
            public int individualSessionId { get; set; }
            public string tutorRepportMessage { get; set; }
            public string helpedRepportMessage { get; set; }
        }

        public class ToBeConfirmedHelpRequestViewModel
        {
            public int id;
            public string helpedFirstName { get; set; }
            public string helpedLastName { get; set; }

            [DisplayFormat(DataFormatString = "{0:d/M/yyyy HH:mm:ss}")]
            public DateTime createdTime { get; set; }

            public string courseName { get; set; }
        }
    }
}
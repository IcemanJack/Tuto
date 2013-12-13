using System;
using System.ComponentModel.DataAnnotations;
using Tuto.DataLayer.Enums;

namespace Tuto.Web.ViewModels.TutorsMgr
{
        public class TutorListViewModel
        {
            public int id { get; set; }

            [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayUserName")]
            public string name { get; set; }

            [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayUserLastName")]
            public string lastName { get; set; }

            [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayUserMail")]
            public string mail { get; set; }

            [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayTutorAvailableForGroupSession")]
            public Boolean tutorAvailableForGroupSession { get; set; }

            [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayTutorAvailableForIndividualSession")]
            public Boolean tutorAvailableForIndividualSession { get; set; }

            [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayTutorState")]
            public TutorState tutorState { get; set; }

            public bool containsReport { get; set; }
        }
    
}
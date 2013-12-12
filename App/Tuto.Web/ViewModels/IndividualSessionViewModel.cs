using System;
using System.ComponentModel.DataAnnotations;

namespace Tuto.Web.ViewModels
{
    public class IndividualSessionViewModel
    {
        [Display(ResourceType = typeof (Resources.Resources), Name = "DisplayTutorName")]
        public string tutorName { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "DisplayTutorLastName")]
        public string tutorLastName { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "DisplayUserName")]
        public string helpedName { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "DisplayUserLastName")]
        public string helpedLastName { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "DisplaySessionDate")]
        public DateTime date { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "DisplaySessionPlace")]
        public String place { get; set; }

    }
}
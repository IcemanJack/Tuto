using System;
using System.ComponentModel.DataAnnotations;
using Tuto.DataLayer.Enums;
using Tuto.DataLayer.Models;

namespace Tuto.Web.ViewModels.HelpRequest
{
    public class HelpRequestListDetailsViewModel
    {
        [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayHelpRequestDateTime")]
        [DisplayFormat(DataFormatString = "{0:d/M/yyyy HH:mm:ss}")]
        public DateTime createdTime { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayHelpRequestState")]
        public HelpRequestState currentState { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayHelpRequestCourseName")]
        public Course course { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayHelpRequestMisunderstoodNotions")]
        public string misunderstoodNotions { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "DisplayHelpRequestComments")]
        public string comment { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using Tuto.DataLayer.Enums;
using Tuto.DataLayer.Reports;

namespace Tuto.Web.Controllers.Manager.Reports
{
    public class MonthlyWorkedHoursReportEntry : AbstractTableEntry
    {
        [Display(ResourceType = typeof(Resources.Resources), Name = "TutorFirstName")]
        public string tutorFirstName { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "TutorLastName")]
        public string tutorLastName { get; set; }

        [Display(ResourceType = typeof(Resources.Resources), Name = "TutorWorkedHours")]
        public int workedHours { get; set; }
    }

    public class MonthlyWorkedHoursReport : TableAbstractReport<MonthlyWorkedHoursReportEntry>
    {
        public MonthlyWorkedHoursReport() : base(ReportsType.MONTHLY_WORKED_HOURS)
        { }
    }
}
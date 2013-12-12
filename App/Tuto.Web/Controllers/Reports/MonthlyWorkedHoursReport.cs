using System.ComponentModel.DataAnnotations;
using Tuto.DataLayer.Enums;
using Tuto.DataLayer.Reports;

namespace Tuto.Web.Controllers.Reports
{
    public class MonthlyWorkedHoursReportEntry : AbstractTableEntry
    {
        [Display(Name="Prénom tuteur")]
        public string tutorFirstName { get; set; }

        [Display(Name="Nom famille tuteur")]
        public string tutorLastName { get; set; }

        [Display(Name="Heures travaillées")]
        public int workedHours { get; set; }
    }

    public class MonthlyWorkedHoursReport : TableAbstractReport<MonthlyWorkedHoursReportEntry>
    {
        public MonthlyWorkedHoursReport() : base(ReportsType.MONTHLY_WORKED_HOURS)
        { }
    }
}
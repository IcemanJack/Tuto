using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using RazorPDF;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Config;
using Tuto.Web.Controllers.Manager.Reports;
using Tuto.Web.ViewModels.Reports;

namespace Tuto.Web.Controllers.Manager
{
    public class ReportsMgrController : DefaultController
    {
        public ReportsMgrController(WebAppLaunchContext context) : base(context)
        {
            this.setAccessType(PageAccessType.TYPE_MANAGER);
        }

        public ReportsMgrController() : this(new WebAppLaunchContext())
        { }

        public ActionResult show()
        {
            if (!this.isUserAllowed())
            {
                return this.kickUser();
            }

            return this.View("Show");
        }

        public PdfResult tutorsMonthlyWorkedHours()
        {
            var finalReport = new MonthlyWorkedHoursReport();
            
            // get all tutors
            var everyWorkingTutors = this.appContext.getRepository().getAll<Tutor>().ToList();

            foreach(Tutor workingTutor in everyWorkingTutors)
            {
                finalReport.addDataRow(Mapper.Map<MonthlyWorkedHoursReportEntry>(workingTutor));
            }
            var finalMonthlyReport = Mapper.Map<MonthlyWorkedHoursReportViewModel>(finalReport);

            return new PdfResult(finalMonthlyReport, "MonthlyHoursReport");
        }

    }
}

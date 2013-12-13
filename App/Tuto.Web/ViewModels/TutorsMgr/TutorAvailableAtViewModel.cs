using Tuto.DataLayer.Models;

namespace Tuto.Web.ViewModels.TutorsMgr
{
    public class TutorAvailableAtViewModel
    {
        public WeekDay weekDay { get; set; }
        public int startTime { get; set; }
        public int endTime { get; set; }
    }
}
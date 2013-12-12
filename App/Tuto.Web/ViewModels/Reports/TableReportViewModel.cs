using System.Collections.Generic;

namespace Tuto.Web.ViewModels.Reports
{
    public class TableReportViewModel : AbstractReportViewModel
    {
        public string[] columnsTitle { get; set; }
        public List<string[]> tableData { get; set; }
    }
}
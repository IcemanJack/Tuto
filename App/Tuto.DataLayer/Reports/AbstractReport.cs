using System;
using Tuto.DataLayer.Enums;

namespace Tuto.DataLayer.Reports
{
    public abstract class AbstractReport<T>
    {
        protected AbstractReport(ReportsType theType)
        {
            this.type = theType;
        }

        public string title { get; set; }
        public DateTime generatedAt { get; set; }
        
        public ReportsType type { get; set; }
        public T innerData { get; set; }
    }
}
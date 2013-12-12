using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Tuto.DataLayer.Enums;

namespace Tuto.DataLayer.Reports
{
    public abstract class TableAbstractReport<T> : AbstractReport<List<T>>, IEnumerable<List<string>> where T : AbstractTableEntry
    {
        protected TableAbstractReport(ReportsType type) : base(type)
        {
            this.innerData = new List<T>();
        }

        protected List<List<string>> columnsTitle { get; set; }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return this.GetEnumerator(); }

        public IEnumerator<List<string>>  GetEnumerator()
        {
            return new TableStringEntryEnumerator(this.innerData);
        }

        public string[] getColumnsTitle()
        {
            var titleList = new List<string>();
            
            var entryType = typeof (T) as Type;
            foreach (PropertyInfo prop in entryType.GetProperties())
            {
                var columnTitle = prop.Name;

                var displayAttr = ((DisplayAttribute)prop.GetCustomAttributes(typeof (DisplayAttribute), false).FirstOrDefault());
                if (displayAttr != null)
                {
                    columnTitle = displayAttr.Name;
                }

                titleList.Add(columnTitle);
            }

            return titleList.ToArray();
        }

        public void addDataRow(T rowToAdd)
        {
            this.innerData.Add(rowToAdd);
        }

        private class TableStringEntryEnumerator : IEnumerator<List<string>>
        {
            private readonly IEnumerator<T> entryEnum; 
 
            public TableStringEntryEnumerator(List<T> tableData)
            {
                this.entryEnum = tableData.GetEnumerator();
            }

            public void Dispose()
            {
                this.entryEnum.Dispose();
            }

            public bool MoveNext()
            {
                return this.entryEnum.MoveNext();
            }

            public void Reset()
            {
                this.entryEnum.Reset();
            }

            public List<string> Current {
                get
                {
                    var returnEntryValues = new List<string>();
                    var entryType = typeof(T) as Type;
                    var entry = this.entryEnum.Current;

                    foreach (PropertyInfo prop in entryType.GetProperties())
                    {
                        object entryFieldValue = prop.GetValue(entry, null);
                        string entryFieldValueStr = "";

                        if (entryFieldValue != null)
                        {
                            entryFieldValueStr = entryFieldValue.ToString();
                        }

                        returnEntryValues.Add(entryFieldValueStr);
                    }

                    return returnEntryValues;
                }

                private set { }
            }

            object IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }
        }
    }

    public abstract class AbstractTableEntry
    { }
}
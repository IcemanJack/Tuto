using System.Collections.Generic;

namespace Tuto.DataLayer.Models
{
    public class Department : Entity
    {
        public string name { get; set; }

        public virtual List<Course> courses { get; set; }

        public override bool Equals(object other)
        {
            var otherDepartement = other as Department;
            if (otherDepartement == null)
            {
                return false;
            }

            return (this.name == otherDepartement.name);
        }

        public override int GetHashCode()
        {
            return this.name.GetHashCode();
        }
    }
}
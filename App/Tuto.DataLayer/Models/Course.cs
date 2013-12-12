using System.Collections.Generic;
using Tuto.DataLayer.Models.Users;

namespace Tuto.DataLayer.Models
{
    public class Course : Entity
    {
        public Course()
        {
            this.tutorsWithSkillsInThis = new HashSet<Tutor>();
        }

        public string courseCode { get; set; }
        public string courseName { get; set; }

        public int departmentId { get; set; }
        public virtual Department department { get; set; }

        // tutors having this course as a skill
        public virtual ICollection<Tutor> tutorsWithSkillsInThis { get; set; }
        public virtual ICollection<HelpRequest> helpRestRequests { get; set; }

        public override bool Equals(object other)
        {
            var otherCourse = other as Course;
            if (otherCourse == null)
            {
                return false;
            }

            return (otherCourse.courseCode == this.courseCode && otherCourse.courseName == this.courseName/* && otherCourse.department == this.department*/);
        }

        public override int GetHashCode()
        {
            return (this.courseCode + this.courseName).GetHashCode();
        }
    }
}
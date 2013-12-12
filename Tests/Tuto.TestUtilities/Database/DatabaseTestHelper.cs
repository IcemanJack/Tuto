using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuto.DataLayer;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.Repository;

namespace Tuto.TestUtilities.Database
{
    public class DatabaseTestHelper
    {
        private readonly TutoContext context;

        public DatabaseTestHelper(TutoContext context)
        {
            this.context = context;
        }

        public void cleanAllTables()
        {
            cleanTable<Course>();
            cleanTable<Department>();
        }

        public void seedTables()
        {
            cleanAllTables();

            // Departments
            var general = new Department() { name = "Général" };
            var informatique = new Department() { name = "Informatique" };

            // Courses
            var francais = new Course() { courseName = "Français 1" };
            var anglais = new Course() { courseName = "Anglais 1" };
            francais.department = general;
            anglais.department = general;

            var programmation = new Course() { courseName = "Programmation 1" };
            var algorithmie = new Course() { courseName = "Algorithmie 1" };
            programmation.department = informatique;
            algorithmie.department = informatique;

            var repository = new EntityRepository();
            repository.addAll(francais, anglais, programmation, algorithmie);
        }

        private void cleanTable<T>() where T : class
        {
            var entities = context.Set<T>();
            foreach (var entity in entities)
            {
               context.Set<T>().Remove(entity);
            }
            context.SaveChanges();
        }
    }

}

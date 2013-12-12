using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.Models.Notifications;
using Tuto.DataLayer.Models.Notifications.Manager;
using Tuto.DataLayer.Models.Notifications.Shared;
using Tuto.DataLayer.Models.Notifications.Tutor;
using Tuto.DataLayer.Models.Users;

namespace Tuto.DataLayer
{
    public class TutoContext : DbContext
    {
        public DbSet<HelpRequest> helpRequests { get; set; }
        public DbSet<IndividualSession> individualSessions { get; set; }

        public DbSet<Tutor> tutors { get; set; }
        public DbSet<Helped> helpeds { get; set; }
        public DbSet<Manager> managers { get; set; }
        public DbSet<Course> courses { get; set; }
        public DbSet<ScheduleBlock> scheduleBlocks { get; set; }
        public DbSet<Department> departments { get; set; }

        public DbSet<BaseNotification> baseNotifications { get; set; }
        public DbSet<SharedBaseNotification> sharedNotifications { get; set; }

        public DbSet<AssignedToHelpRequestTask> assignedToHelpRequestAlerts { get; set; }
        public DbSet<AssignedToGroupSessionAlert> assignedToGroupSessionAlerts { get; set; }
        public DbSet<TutorHasRegisteredTask> tutorHasRegisteredTasks { get; set; }
        public DbSet<HelpRequestToAssignAlert> helpRequestToAslToAssignAlerts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToOneConstraintIntroductionConvention>(); // we want to manually specify if we want cascade on delete

            modelBuilder.Entity<IndividualSession>()
                .HasKey(c => c.helpRequestId);

            modelBuilder.Entity<IndividualSession>()
                .HasRequired(c => c.helpRequest)
                .WithOptional(r => r.individualSession)
                .WillCascadeOnDelete();

            // Tutors < - > ScheduleBlocks
            modelBuilder.Entity<Tutor>()
                .HasMany(c => c.scheduleBlocks)
                .WithMany(r => r.tutors);

            // Helped < - > ScheduleBlocks
            modelBuilder.Entity<Helped>()
                .HasMany(c => c.scheduleBlocks)
                .WithMany(r => r.helpeds);

            // Tutor < - > HelpRequest
            modelBuilder.Entity<Tutor>()
                .HasMany(c => c.helpRequests)
                .WithOptional(r => r.tutor);

            // Helped < - > HelpRequest
            modelBuilder.Entity<Helped>()
                .HasMany(c => c.helpRequests)
                .WithRequired(r => r.helped);

            // Alerts
            modelBuilder.Entity<SharedBaseNotification>()
                .HasMany(c => c.concernedUsers)
                .WithMany(r => r.sharedNotifications);

            modelBuilder.Entity<HelpedBaseNotification>()
                .HasRequired(c => c.helpedUser)
                .WithMany(r => r.individualNotifications)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TutorBaseNotification>()
                .HasRequired(c => c.tutorUser)
                .WithMany(r => r.individualNotifications)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ManagerBaseNotification>()
                .HasRequired(c => c.managerUser)
                .WithMany(r => r.individualNotifications)
                .WillCascadeOnDelete(false);

            // AssignedToGroupSessionAlertB
            modelBuilder.Entity<AssignedToGroupSessionAlert>()
                .HasRequired(c => c.concernedGroupSession)
                .WithOptional()
                .WillCascadeOnDelete(false);

            // TutorHasRegistredTask
            modelBuilder.Entity<TutorHasRegisteredTask>()
                .HasRequired(c => c.registeredTutor)
                .WithOptional()
                .WillCascadeOnDelete(false);

            // HelpRequestToAssignAlert
            modelBuilder.Entity<HelpRequestToAssignAlert>()
                .HasRequired(c => c.helpRequest)
                .WithMany()
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}

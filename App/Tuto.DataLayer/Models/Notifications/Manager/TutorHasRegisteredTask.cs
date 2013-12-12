using Tuto.DataLayer.Enums;
using Tuto.DataLayer.Models.Notifications.Interfaces;

namespace Tuto.DataLayer.Models.Notifications.Manager
{
    public class TutorHasRegisteredTask : ManagerBaseNotification, IBuildableAlert<TutorHasRegisteredTask.TutorHasRegistredNotificationBuilder>
    {
        public TutorHasRegisteredTask() : base(NotificationTypes.TASK)
        { }

        public int registeredTutorId { get; set; }
        
        // ---
        public virtual Users.Tutor registeredTutor { get; set; }
        // --

        public TutorHasRegistredNotificationBuilder getBuilder()
        {
            return new TutorHasRegistredNotificationBuilder(this);
        }

        public class TutorHasRegistredNotificationBuilder : AbstractManagerBaseNotificationBuilder<TutorHasRegisteredTask>
        {
            public TutorHasRegistredNotificationBuilder(TutorHasRegisteredTask buildInstance) : base(buildInstance)
            { }

            public TutorHasRegistredNotificationBuilder setNewlyRegistredTutor(Users.Tutor registeredTutor)
            {
                this.buildInstance.registeredTutor = registeredTutor;
                return this;
            }
        }
    }
}
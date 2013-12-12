using Tuto.DataLayer.Enums;

namespace Tuto.DataLayer.Models.Notifications
{
    public abstract class TutorBaseNotification : BaseNotification
    {
        protected TutorBaseNotification(NotificationTypes type) : base(type)
        { }

        public int tutorUserId { get; set; }

        // navigation properties
        public virtual Users.Tutor tutorUser { get; set; }

        public override bool isASharedAlert()
        {
            return false;
        }

        public abstract class AbstractTutorBaseNotificationBuilder<T> : AbstractNotificationBuilder<T> where T : TutorBaseNotification
        {
            protected AbstractTutorBaseNotificationBuilder(T buildInstance) : base(buildInstance)
            { }

            public dynamic setConcernedTutor(Users.Tutor concernedTutor)
            {
                this.buildInstance.tutorUser = concernedTutor;
                return this;
            }
        }
    }
}

using Tuto.DataLayer.Enums;
using Tuto.DataLayer.Models.GroupSessions;
using Tuto.DataLayer.Models.Notifications.Interfaces;

namespace Tuto.DataLayer.Models.Notifications.Tutor
{
    public class AssignedToGroupSessionAlert : TutorBaseNotification, IBuildableAlert<AssignedToGroupSessionAlert.AssignedToGroupSessionNotificationBuilder>
    {
        public AssignedToGroupSessionAlert() : base(NotificationTypes.ALERT)
        { }

        public int concernedGroupSessionId { get; set; }

        // ---
        public virtual AssignedGroupSession concernedGroupSession { get; set; }

        // ---

        public AssignedToGroupSessionNotificationBuilder getBuilder()
        {
            return new AssignedToGroupSessionNotificationBuilder(this);
        }


        public class AssignedToGroupSessionNotificationBuilder : AbstractTutorBaseNotificationBuilder<AssignedToGroupSessionAlert> 
        {
            public AssignedToGroupSessionNotificationBuilder(AssignedToGroupSessionAlert buildInstance) : base(buildInstance)
            { }

            public AssignedToGroupSessionNotificationBuilder setConcernedGroupSession(AssignedGroupSession concernedGroupSession)
            {
                this.buildInstance.concernedGroupSession = concernedGroupSession;
                this.setConcernedTutor(concernedGroupSession.tutor);
                return this;
            }
        }
    }
}
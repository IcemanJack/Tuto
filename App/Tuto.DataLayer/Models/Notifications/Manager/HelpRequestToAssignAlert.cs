using Tuto.DataLayer.Enums;
using Tuto.DataLayer.Models.Notifications.Interfaces;

namespace Tuto.DataLayer.Models.Notifications.Manager
{
    public class HelpRequestToAssignAlert : ManagerBaseNotification, IBuildableAlert<HelpRequestToAssignAlert.HelpRequestToAssignAlertBuilder>
    {
        public HelpRequestToAssignAlert() : base(NotificationTypes.ALERT)
        { }

        public int helpRequestId { get; set; }

        // ---
        public virtual HelpRequest helpRequest { get; set; }
        // ---

        public HelpRequestToAssignAlertBuilder getBuilder()
        {
            return new HelpRequestToAssignAlertBuilder(this);
        }

        public class HelpRequestToAssignAlertBuilder : AbstractManagerBaseNotificationBuilder<HelpRequestToAssignAlert>
        {
            public HelpRequestToAssignAlertBuilder(HelpRequestToAssignAlert buildInstance) : base(buildInstance)
            { }

            public HelpRequestToAssignAlertBuilder setConcernedHelpRequest(HelpRequest concernedHelpRequest)
            {
                this.buildInstance.helpRequest = concernedHelpRequest;
                return this;
            }
        }
    }
}
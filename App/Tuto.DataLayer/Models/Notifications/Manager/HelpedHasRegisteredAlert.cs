using Tuto.DataLayer.Enums;
using Tuto.DataLayer.Models.Notifications.Interfaces;
using Tuto.DataLayer.Models.Users;

namespace Tuto.DataLayer.Models.Notifications.Manager
{
    public class HelpedHasRegisteredAlert : ManagerBaseNotification, IBuildableAlert<HelpedHasRegisteredAlert.HelpedHasRegisteredAlertBuilder>
    {
        public HelpedHasRegisteredAlert() : base(NotificationTypes.ALERT)
        { }

        public int helpedUserId { get; set; }
        
        // --
        public virtual Helped helpedUser { get; set; }
        // --

        public HelpedHasRegisteredAlertBuilder getBuilder()
        {
            return new HelpedHasRegisteredAlertBuilder(this);
        }

        public class HelpedHasRegisteredAlertBuilder : AbstractManagerBaseNotificationBuilder<HelpedHasRegisteredAlert>
        {
            public HelpedHasRegisteredAlertBuilder(HelpedHasRegisteredAlert buildInstance) : base(buildInstance)
            { }

            public HelpedHasRegisteredAlertBuilder setConcernedHelped(Helped concernedHelped)
            {
                this.buildInstance.helpedUser = concernedHelped;
                return this;
            }

        }

    }
}
using System;
using Tuto.DataLayer.Enums;
using Tuto.DataLayer.Models.Notifications.Interfaces;

namespace Tuto.DataLayer.Models.Notifications.Shared
{
    public class AssignedToHelpRequestTask : SharedBaseNotification, IBuildableAlert<AssignedToHelpRequestTask.AssignedToHelpRequestBuilder>
    {
        public AssignedToHelpRequestTask() : base(NotificationTypes.TASK)
        { }

        public int concernedHelpRequestId { get; set; }

        // navigation properties
        public virtual HelpRequest concernedHelpRequest { get; set; }

        public AssignedToHelpRequestBuilder getBuilder()
        {
            return new AssignedToHelpRequestBuilder(this);
        }

        //
        // builder
        //
        public class AssignedToHelpRequestBuilder : AbstractSharedBaseNotificationBuilder<AssignedToHelpRequestTask>
        {
            public AssignedToHelpRequestBuilder(AssignedToHelpRequestTask taskInstance) : base(taskInstance)
            {}

            public AssignedToHelpRequestBuilder setConcernedHelpRequest(HelpRequest concernedHelpRequest)
            {
                this.buildInstance.concernedHelpRequest = concernedHelpRequest;
                if (concernedHelpRequest.getState() != HelpRequestState.TO_BE_CONFIRMED)
                {
                    throw new NotAnAssignedHelpRequestType();
                }

                this.addConcernedUser(concernedHelpRequest.helped);
                this.addConcernedUser(concernedHelpRequest.tutor);

                return this;
            }

            public class NotAnAssignedHelpRequestType : Exception { }
        }
    }
}
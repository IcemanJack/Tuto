namespace Tuto.DataLayer.Enums
{
    public enum HelpRequestState : int
    {
        NOT_ASSIGNED = 0,  // When there's no date and place assigned yet
        TO_BE_CONFIRMED = 1, // When both tutor and helped need to confirm if the date and place is ok
        CONFIRMED = 2,  // Both tutor and helped have confirmed
        FINISHED = 3, // The session is finished
        BILAN = 4 // Both reports were made
    }

    public static class HelpRequestStateString
    {
        public static string getString(this HelpRequestState state)
        {
            switch (state)
            {
                case HelpRequestState.NOT_ASSIGNED:
                    return Resources.Resources.StateStringNotAssigned;
                case HelpRequestState.TO_BE_CONFIRMED:
                    return Resources.Resources.StateStringToBeConfirmed;
                case HelpRequestState.CONFIRMED:
                    return Resources.Resources.StateStringConfirmed;
                case HelpRequestState.FINISHED:
                    return Resources.Resources.StateStringFinished;
                case HelpRequestState.BILAN:
                    return Resources.Resources.StateStringBilan;
                default:
                    return "";
            }
        }
    }

}
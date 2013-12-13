using Tuto.DataLayer.Enums;

namespace Tuto.DataLayer.Exceptions.HelpRequest
{
    public class InvalidHelpRequestStateException : AbstractModelRuntimeException
    {
        public InvalidHelpRequestStateException(HelpRequestState expectedState)
            : base("Invalid HelpRequest State ... expecting type : " + expectedState.ToString())
        {  }
    }
}
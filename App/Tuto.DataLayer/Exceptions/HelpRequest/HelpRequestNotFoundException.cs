namespace Tuto.DataLayer.Exceptions.HelpRequest
{
    public class HelpRequestNotFoundException : AbstractModelRuntimeException
    {
        public HelpRequestNotFoundException() 
            : base("This HelpRequest is either invalid or does not exists in database ...")
        { }
    }
}
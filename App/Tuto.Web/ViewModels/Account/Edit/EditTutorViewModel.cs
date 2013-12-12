namespace Tuto.Web.ViewModels.Account.Edit
{
    public class EditTutorViewModel : EditStudentViewModel
    {
        public override bool isTutor()
        {
            return true;
        }
    }
}
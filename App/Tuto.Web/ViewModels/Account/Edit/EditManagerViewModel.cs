namespace Tuto.Web.ViewModels.Account.Edit
{
    public class EditManagerViewModel : EditStudentViewModel
    {
        public override bool isManager()
        {
            return true;
        }
    }
}
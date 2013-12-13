
namespace Tuto.Web.ViewModels.Account.Edit
{
    public class EditHelpedViewModel : EditStudentViewModel
    {
        public override bool isHelped()
        {
            return true;
        }
    }
}
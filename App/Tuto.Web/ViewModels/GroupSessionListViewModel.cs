using System.Collections.Generic;
using Tuto.DataLayer.Models.GroupSessions;

namespace Tuto.Web.ViewModels
{
    public class GroupSessionListViewModel
    {
        public ICollection<AssignedGroupSession> assignedGroupSessions { get; set; }
    }
}
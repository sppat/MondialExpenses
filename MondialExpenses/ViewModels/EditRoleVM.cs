using Microsoft.AspNetCore.Identity;

namespace MondialExpenses.ViewModels
{
    public class EditRoleVM
    {
        public IdentityRole Role { get; set; }
        public IList<IdentityUser> Members { get; set; }
        public IEnumerable<IdentityUser> NonMembers { get; set; }
    }
}

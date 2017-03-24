using DentiCare.Models.UserSettings;
using System.Collections.Generic;

namespace DentiCare.Models.Admin
{
    public class DisplayUsersViewModel
    {
        public List<UserModel> UserViews { get; set; }

        public DisplayDetailsViewModel Details { get; set; }
    }
}
using DentiCare.Models.Admin;
using System.Collections.Generic;

namespace DentiCare.Models.SendMails
{
    public class MailViewModel
    {
        public List<UserModel> UserViews { get; set; }

        public Mail Message { get; set; }
    }
}
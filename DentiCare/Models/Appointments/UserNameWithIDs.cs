using System.ComponentModel.DataAnnotations;

namespace DentiCare.Models.Appointments
{
    public class UserNameWithIDs
    {
        public int UserInfoID { get; set; }

        public string FullName { get; set; }
    }
}
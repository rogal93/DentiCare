using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DentiCare.Repository.Entities
{
    /// <summary>
    /// Użytkownik aplikacji.
    /// </summary>
    [Table("AspNetUsers")]
    public class User : IdentityUser
    {
        /// <summary>
        /// ID danych użytkownika.
        /// </summary>
        [ForeignKey("UserInfo")]
        public int user_info_id { get; set; }

        /// <summary>
        /// ID danych lekarza.
        /// </summary>
        [ForeignKey("MedicalExperience")]
        public int? medical_exp_id { get; set; }

        /// <summary>
        /// Dane użytkownika.
        /// </summary>
        public virtual UserInformation UserInfo { get; set; }

        /// <summary>
        /// Dane stomatologa.
        /// </summary>
        public virtual MedicalExperience MedicalExperience { get; set; }
        
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}
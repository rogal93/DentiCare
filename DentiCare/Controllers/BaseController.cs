using DentiCare.Repository;
using DentiCare.Repository.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DentiCare.Controllers
{
    public class BaseController : Controller
    {
        private readonly DentiCareDBContext _context = new DentiCareDBContext();

        public BaseController() { }

        #region SignInManager, UserManager, RoleManager

        protected ApplicationSignInManager _signInManager;
        protected ApplicationUserManager _userManager;
        protected RoleManager<IdentityRole> _roleManager;

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        public RoleManager<IdentityRole> RoleManager
        {
            get { return _roleManager ?? new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context)); }
            private set { _roleManager = value; }
        }

        #endregion

        #region CurrentUser fields

        private User _currentUser;

        public User CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    _currentUser = UserManager.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
                }
                return _currentUser;
            }
            set { _currentUser = value; }
        }
        public UserInformation CurrentUserInfo
        {
            get { return CurrentUser.UserInfo; }
        }
        public Address CurrentUserAddress
        {
            get { return CurrentUserInfo.Address; }
        }
        public MedicalExperience CurrentUserMedicalExperience
        {
            get { return CurrentUser.MedicalExperience; }
        }

        public string CurrentUserPhotoURL
        {
            get
            {
                if (CurrentUser == null)
                {
                    return Url.Content("/Images/Photos/profile-photo.jpg");
                }
                else if (!CurrentUserInfo.photo_id.HasValue && CurrentUserInfo.gender == "Mężczyzna")
                {
                    return Url.Content("/Images/Photos/profile-photo.jpg");
                }
                else if (!CurrentUserInfo.photo_id.HasValue && CurrentUserInfo.gender == "Kobieta")
                {
                    return Url.Content("/Images/Photos/profile-photo-female.jpg");
                }
                else
                {
                    return CurrentUserInfo.Photo.url;
                }
            }
        }
        public string CurrentUserName
        {
            get
            {
                if (CurrentUser != null)
                {
                    return CurrentUser.UserInfo.first_name + " " + CurrentUser.UserInfo.last_name;
                }
                else
                {
                    return "";
                }
            }
        }

        #endregion

        #region Roles

        public bool IsAssistant
        {
            get
            {
                if (CurrentUser != null)
                {
                    return UserManager.IsInRole(CurrentUser.Id, "Asystentka");
                }
                return false;
            }
        }
        public bool IsDoctor
        {
            get
            {
                if (CurrentUser != null)
                {
                    return UserManager.IsInRole(CurrentUser.Id, "Stomatolog");
                }
                return false;
            }
        }
        public bool IsAdmin
        {
            get
            {
                if (CurrentUser != null)
                {
                    return UserManager.IsInRole(CurrentUser.Id, "Administrator");
                }
                return false;
            }
        }

        public bool LoggedOn { get { return User.Identity.IsAuthenticated; } }

        #endregion
        
        #region OnAction methods

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SetRolesToViewBag();
            SetPersonalInfoToViewBag();

            base.OnActionExecuting(filterContext);
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        #endregion

        #region ViewBag methods

        public void SetRolesToViewBag()
        {
            ViewBag.IsDoctor = IsDoctor;
            ViewBag.IsAdmin = IsAdmin;
            ViewBag.IsAssistant = IsAssistant;
        }
        public void SetPersonalInfoToViewBag()
        {
            string myName = CurrentUserName;
            if (IsDoctor)
            {
                myName = "lek. stom. " + myName;
            }
            ViewBag.MyName = myName;
            ViewBag.MyPhoto = CurrentUserPhotoURL;
        }

        #endregion

        #region Dispose method

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
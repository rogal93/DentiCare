using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using DentiCare.Controllers;
using DentiCare.Repository.Abstract;
using DentiCare.Repository.Concrete;
using System.Data.Entity;
using DentiCare.Repository;
using Microsoft.AspNet.Identity;
using DentiCare.Repository.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web;
using Microsoft.Owin.Security;

namespace DentiCare.App_Start
{
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });
        
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<DbContext, DentiCareDBContext>(new PerRequestLifetimeManager());
            container.RegisterType<IUserStore<User>, UserStore<User>>(new PerRequestLifetimeManager());
            
            container.RegisterType<AccountController>(new InjectionConstructor());

            container.RegisterType<ApplicationUserManager>(new PerRequestLifetimeManager());
            container.RegisterType<ApplicationSignInManager>(new PerRequestLifetimeManager());

            container.RegisterType<IAuthenticationManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));

            container.RegisterType<IPhotosRepository, PhotosRepository>(new PerRequestLifetimeManager());
            container.RegisterType<IAddressesRepository, AddressesRepository>(new PerRequestLifetimeManager());
            container.RegisterType<IUserInformationsRepository, UserInformationsRepository>(new PerRequestLifetimeManager());
            container.RegisterType<IAppointmentsRepository, AppointmentsRepository>(new PerRequestLifetimeManager());
            container.RegisterType<ITreatmentsRepository, TreatmentsRepository>(new PerRequestLifetimeManager());
            container.RegisterType<IMedicalExperienceRepository, MedicalExperienceRepository>(new PerRequestLifetimeManager());
            container.RegisterType<ISchedulesRepository, SchedulesRepository>(new PerRequestLifetimeManager());
            container.RegisterType<IMedicalDocumentsRepository, MedicalDocumentsRepository>(new PerRequestLifetimeManager());
        }
    }
}

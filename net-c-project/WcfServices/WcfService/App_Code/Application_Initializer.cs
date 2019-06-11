using DSPrima.UserSessionStore;
using DSPrima.WcfUserSession.SecurityHandlers;
using PCHI.BusinessLogic;
using PCHI.BusinessLogic.Security;
using PCHI.Model.Users;
using System.Data.Entity;

namespace PCHI.WcfServices.API.PCHIServices.WcfService.App_Code
{
    public class Application_Initializer
    {
        public static void AppInitialize()
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<PCHI.DataAccessLibrary.Context.MainDatabaseContext, PCHI.DataAccessLibrary.Migrations.Configuration>());
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<DSPrima.UserSessionStore.Context.SessionContext, DSPrima.UserSessionStore.Migrations.Configuration>());

            new PCHI.DataAccessLibrary.Context.MainDatabaseContext().Users.Load();
            WcfUserSessionSecurity.SessionUpdated += WcfUserSessionUserManager.WcfUserSessionSecurity_SessionUpdated;            
            WcfUserSessionSecurity.MultiStepVerification = true;
            WcfUserSessionSecurity.SessionTimeout = 240;
            WcfUserSessionSecurity.SessionStore = new UserSessionStore();
            WcfUserSessionSecurity.UserManager = new WcfUserSessionUserManager();
        }
    }
}
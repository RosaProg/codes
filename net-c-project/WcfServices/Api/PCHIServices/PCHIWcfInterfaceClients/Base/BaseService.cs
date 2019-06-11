using DSPrima.WcfUserSession.SecurityHandlers;
using PCHI.BusinessLogic;
using PCHI.BusinessLogic.Security;
using PCHI.Model.Users;
using PCHI.WcfServices.API.PCHIServices.InterfaceContracts.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PCHI.WcfServices.API.PCHIServices.InterfaceClients.Base
{
    /// <summary>
    /// The base service class that hold the logice each class should implement
    /// Implement the IBaseService class for that
    /// </summary>
    public abstract class BaseService : IBaseService
    {
        /// <summary>
        /// Contains the instance of the <see cref="ManagerHandler"/> class
        /// </summary>
        protected ManagerHandler handler = new ManagerHandler();
        
        /// <summary>
        /// Returns a string with some identifying features of this service.
        /// </summary>
        /// <returns>A string detailing certain the status of certain elements of the service</returns>
        public string Ping()
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement docElement = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, docElement);

            XmlElement root = doc.CreateElement("PCHI_WcfService_settings");
            doc.AppendChild(root);

            XmlElement name = doc.CreateElement("Name");
            name.InnerText = ConfigurationManager.AppSettings["ServiceName"];
            root.AppendChild(name);

            XmlElement version = doc.CreateElement("Version");
            version.InnerText = ConfigurationManager.AppSettings["ServiceVersion"];
            root.AppendChild(version);

            XmlElement instance = doc.CreateElement("Instance");
            instance.InnerText = ConfigurationManager.AppSettings["ServiceInstance"];
            root.AppendChild(instance);

            XmlElement uptime = doc.CreateElement("Uptime");
            uptime.InnerText = (DateTime.Now - System.Diagnostics.Process.GetCurrentProcess().StartTime).ToString();
            root.AppendChild(uptime);

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MainContext"].ConnectionString);
            XmlElement mainCon = doc.CreateElement("MainConnectionString");
            mainCon.InnerText = "DataSource: " + con.DataSource + " => Database " + con.Database;
            root.AppendChild(mainCon);

            con = new SqlConnection(ConfigurationManager.ConnectionStrings["DSPrima.SessionContext"].ConnectionString);
            XmlElement sessionCon = doc.CreateElement("DSPrima.SessionContext");
            sessionCon.InnerText = "DataSource: " + con.DataSource + " => Database " + con.Database;
            root.AppendChild(sessionCon);

            XmlElement maxFailedAccessAttemptsBeforeLockout = doc.CreateElement("MaxFailedAccessAttemptsBeforeLockout");
            maxFailedAccessAttemptsBeforeLockout.InnerText = BusinessLogic.Properties.Settings.Default.MaxFailedAccessAttemptsBeforeLockout.ToString();
            root.AppendChild(maxFailedAccessAttemptsBeforeLockout);

            XmlElement lockoutTimeSpan = doc.CreateElement("LockoutTimeSpan");
            lockoutTimeSpan.InnerText = BusinessLogic.Properties.Settings.Default.LockoutTimeSpan.ToString();
            root.AppendChild(lockoutTimeSpan);

            return doc.InnerXml;
        }
    }
}

using PCHI.WcfServices.API.PCHIServices.InterfaceProxies.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PCHISchedulerService
{
    class PCHISchedulerService : ServiceBase
    {
        /// <summary>
        /// The token source to use for stopping the Task
        /// </summary>        
        private CancellationTokenSource tokenSource;

        /// <summary>
        /// The token used by the task
        /// </summary>        
        private CancellationToken token;

        /// <summary>
        /// The Task for scheduling questionnaires
        /// </summary>
        private Task QuestionnaireScheduler;



        #region Actions
        public void QuestionnaireSchedule()
        {
            while (!this.token.IsCancellationRequested)
            {
                try
                {
                    ServiceCallsClient scc = new ServiceCallsClient();
                    scc.ScheduleQuestionnaires();
                    scc.SendNotifications();

                }
                catch (Exception)
                {
                }
                    
                int sleepCount = 0;
                while (sleepCount < int.Parse(ConfigurationManager.AppSettings["ScheduleIntervalInSeconds"]) && !this.token.IsCancellationRequested)
                {
                    sleepCount++;
                    Thread.Sleep(1000);
                }
            }
        }

        #endregion

        #region Service Functions
        /// <summary>
        /// Starts the service
        /// </summary>
        /// <param name="args">Any arguments</param>
        protected override void OnStart(string[] args)
        {
            this.tokenSource = new CancellationTokenSource();
            this.token = this.tokenSource.Token;

            this.QuestionnaireScheduler = new Task(this.QuestionnaireSchedule);
            this.QuestionnaireScheduler.Start();
            base.OnStart(args);
        }

        /// <summary>
        /// Stops the service
        /// </summary>
        protected override void OnStop()
        {
            this.tokenSource.Cancel();
            this.QuestionnaireScheduler.Wait();
            base.OnStop();
        }

        /// <summary>
        /// Shuts down the service
        /// </summary>
        protected override void OnShutdown()
        {
            this.OnStop();
            base.OnShutdown();
        }
        #endregion

        /// <summary>
        /// Allows installing and starting of the service
        /// </summary>
        /// <param name="args">Any arguments</param>
        public static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "/i")
            {
                System.Configuration.Install.ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
            }

            else if (args.Length > 0 && args[0] == "/u")
            {
                System.Configuration.Install.ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
            }
            else if (!Environment.UserInteractive)
            {
                System.IO.Directory.SetCurrentDirectory(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.ToString());
                ServiceBase.Run(new PCHISchedulerService());
            }
            else
            {

                PCHISchedulerService service = new PCHISchedulerService();
                service.OnStart(null);

                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();

                service.OnStop();
            }
        }
    }
}

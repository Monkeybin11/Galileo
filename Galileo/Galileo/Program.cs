using System;
using System.ServiceModel;
using System.ServiceModel.Description;

using Galileo.Connection;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

using NLog;

namespace Galileo
{
    internal class Program
    {
        #region Static Fields

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Public Methods and Operators

        public static void Main()
        {
            Logger.Info("========================================================================================");
            Logger.Info("==========================     Starting  Galileo  Service  =============================");
            Logger.Info("========================================================================================");
            bool exceptionOccured = false;
            try
            {
                var container = new UnityContainer();
                container.LoadConfiguration();

                ServiceHost host = new ServiceHost(container.Resolve<IGalileo>());
                Logger.Info("Service configured on {0} endpoints", host.Description.Endpoints.Count);
                foreach (ServiceEndpoint endpoint in host.Description.Endpoints)
                {
                    Logger.Info(endpoint.Address);
                }

                Logger.Info("Starting service host");

                host.Open();

                Logger.Info("Service started.");

                Logger.Info("Press enter for exit");
                Console.Read();
            }
            catch (Exception e)
            {
                Logger.Error("Exeption in the service {0}", e);
                exceptionOccured = true;
            }
            finally
            {
                if (exceptionOccured)
                {
                    Logger.Info("Press enter for exit");
                    Console.Read();
                }
            }
        }

        #endregion
    }
}
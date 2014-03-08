using System;
using System.ServiceModel;
using Galileo.Connection;
using Galileo.Properties;
using GalileoDriver;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using NLog;

namespace Galileo
{
    class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        
        public static void Main()
        {
            logger.Info("========================================================================================");
            logger.Info("==========================     Starting Curiosity service  =============================");
            logger.Info("========================================================================================");
            ServiceHost host = null;
            var exceptionOccured = false;
            try
            {
                var container = new UnityContainer();
                container.LoadConfiguration();
                
                host = new ServiceHost(container.Resolve<IGalileo>());
                logger.Info("Service configured on {0} endpoints", host.Description.Endpoints.Count);
                foreach (var endpoint in host.Description.Endpoints)
                {
                    logger.Info(endpoint.Address);
                }
                logger.Info("Starting service host");

                host.Open();

                logger.Info("Service setarted.");

                var driver = new Driver();
                driver.Initialize();

                logger.Info("Press enter for exit");
                Console.Read();
            }
            catch (Exception e)
            {
                logger.Error("Exeption in the service {0}", e);
                exceptionOccured = true;
            }
            finally
            {
                if (exceptionOccured)
                {
                    logger.Info("Press enter for exit");
                    Console.Read();
                }
            }
        }
    }
}

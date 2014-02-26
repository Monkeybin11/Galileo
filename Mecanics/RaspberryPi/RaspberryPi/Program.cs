using System;
using System.ServiceModel;
using System.Threading;
using RaspberryPi.Connection;
using NLog;

namespace RaspberryPi
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void Main()
        {
            logger.Info("\n\n Starting Curiosity service");

            try
            {
                var host = new ServiceHost(typeof(Galileo), new Uri(@"http://192.168.1.5:10000") );
                logger.Info("Service configured on {0} endpoints", host.Description.Endpoints.Count);
                foreach (var endpoint in host.Description.Endpoints)
                {
                    logger.Info(endpoint.Address);
                }
                logger.Info("Starting host");
                host.Open();
                logger.Info("Service setarted.");
            }
            catch (Exception e)
            {
                logger.Error("Exeption in the service", e);
            }
            
            logger.Info("Press enter for exit");
            Console.Read();
        }
    }
}

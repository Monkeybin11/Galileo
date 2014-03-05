using System;
using System.ServiceModel;

namespace Galileo.Connection
{
    /// <summary>
    /// Represent base communication interface to the Galileo
    /// </summary>
    [ServiceContract]
    public interface IGalileo
    {
        /// <summary>
        /// Initilize all systems. Prepare to the work
        /// </summary>
        /// <returns>True if sucsesfully</returns>
        [OperationContract]
        bool Start();

        /// <summary>
        /// Stop all systems
        /// </summary>
        /// <param name="immeadeatly">halt all systems without waiting  if True</param>
        /// <returns>True if sucsesfully</returns>
        [OperationContract]
        bool Stop(bool immeadeatly );

        /// <summary>
        /// Stop and Poweroff
        /// </summary>
        /// <returns>True if sucsesfully</returns>
        [OperationContract]
        bool Shutdown();

        /// <summary>
        /// Restart Galileo, including OS
        /// </summary>
        /// <returns>True if sucsesfully</returns>
        [OperationContract]
        bool Restart();

        /// <summary>
        /// Receive DriverConfiguration and apply it
        /// </summary>
        [OperationContract]
        void ApplyFirmware();
    }
}

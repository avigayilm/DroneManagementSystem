using System;
using System.Runtime.Serialization;

namespace DAL
{
    [Serializable]
    internal class DroneException : Exception
    {
        public DroneException():base
        {
        }

        public DroneException(string message) : base("Drone exception: "+message)
        {
        }

        public DroneException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DroneException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
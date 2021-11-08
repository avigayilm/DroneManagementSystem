using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace IDAL
{
    namespace DO
    {
        //class Exceptions
        //{
        //}
        [Serializable]
        internal class CustomerException : Exception
        {
            public CustomerException() : base()
            {
            }

            public CustomerException(string message) : base("parcel exception: " + message)
            {
            }

            public CustomerException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected CustomerException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }

        }

        [Serializable]
        internal class DroneException : Exception
        {
            public DroneException() : base()

            {
            }

            public DroneException(string message) : base("Drone exception: " + message)
            {
            }

            public DroneException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected DroneException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        [Serializable]
        internal class ParcelException : Exception
        {
            public ParcelException() : base()
            {
            }

            public ParcelException(string message) : base("Parcel Exception: " + message)
            {
            }

            public ParcelException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected ParcelException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        internal class StationException : Exception
        {
            public StationException() : base()
            {
            }

            public StationException(string message) : base("station exception: " + message)
            {
            }

            public StationException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected StationException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }     }
}


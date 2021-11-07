using System;
using System.Runtime.Serialization;

namespace IDAL
{
    namespace DO
    {
        [Serializable]
        internal class ParcelException : Exception
        {
            public ParcelException():base()
            {
            }

            public ParcelException(string message) : base("Parcel Exception: "+message)
            {
            }

            public ParcelException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected ParcelException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }
}
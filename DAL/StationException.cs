using System;
using System.Runtime.Serialization;

namespace DAL
{
    [Serializable]
    internal class StationException : Exception
    {
        public StationException():base()
        {
        }

        public StationException(string message) : base("station exception: "+ message)
        {
        }

        public StationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
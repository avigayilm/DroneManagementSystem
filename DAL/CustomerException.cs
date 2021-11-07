using System;
using System.Runtime.Serialization;

namespace DAL
{
    [Serializable]
    internal class CustomerException : Exception
    {
        public CustomerException():base()
        {
        }

        public CustomerException(string message) : base("parcel exception: "+message)
        {
        }

        public CustomerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CustomerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
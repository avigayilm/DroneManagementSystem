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
        /// <summary>
        /// class Exceptions
        /// </summary>

        [Serializable]
        internal class DuplicateIdException : Exception
        {
            public DuplicateIdException() : base()
            {
            }

            public DuplicateIdException(string message) : base("Duplicate Id Exception: " + message)
            {
            }

            public DuplicateIdException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected DuplicateIdException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
        [Serializable]
        internal class MissingIdException : Exception
        {
            public MissingIdException() : base()
            {
            }

            public MissingIdException(string message) : base("Missing id exception: " + message)
            {
            }

            public MissingIdException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected MissingIdException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }
}


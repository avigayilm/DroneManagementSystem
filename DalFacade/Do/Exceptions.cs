using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

    namespace DO
    {
        /// <summary>
        /// class Exceptions
        /// </summary>

        [Serializable]
        public class DuplicateIdException : Exception
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
        public class MissingIdException : Exception
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

    public class LoginException : Exception
    {
        public LoginException() : base()
        {
        }

        public LoginException(string message) : base("LoginException: " + message)
        {
        }

        public LoginException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LoginException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }

    public class XMLFileLoadCreateException : Exception
    {
        public string xmlFilePath;
        public XMLFileLoadCreateException(string xmlPath) : base() { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message) :
            base(message)
        { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message, Exception innerException) :
            base(message, innerException)
        { xmlFilePath = xmlPath; }

        public override string ToString()
        {
            return base.ToString() + $", fail to load or create xml file: {xmlFilePath}";
        }
    }
}



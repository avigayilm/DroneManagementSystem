using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace IBL
{
    namespace BO
    {
        /// <summary>
        /// Exception for all the update funcitons
        /// </summary>
        [Serializable]
        internal class UpdateIssueException : Exception
        {
            public UpdateIssueException() : base()
            {
            }

            public UpdateIssueException(string message) : base("Update Issue Exception: " + message)
            {
            }

            public UpdateIssueException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected UpdateIssueException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
        /// <summary>
        ///Exception for all the getFuncitons
        /// </summary>
        [Serializable]
       
        internal class RetrievalException : Exception
        {
            public RetrievalException() : base()
            {
            }

            public RetrievalException(string message) : base("Retrieval Exception: " + message)
            {
            }

            public RetrievalException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected RetrievalException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
        /// <summary>
        /// Exception for all the invalid inputs
        /// </summary>

            [Serializable]
            internal class InvalidInputException : Exception
            {
                public InvalidInputException() : base()
                {
                }

                public InvalidInputException(string message) : base("Invalid input: " + message)
                {
                }

                public InvalidInputException(string message, Exception innerException) : base(message, innerException)
                {
                }

                protected InvalidInputException(SerializationInfo info, StreamingContext context) : base(info, context)
                {
                }
            }

        /// <summary>
        /// Exception for all teh delivery issues
        /// </summary>
        [Serializable]
        internal class DeliveryIssueException : Exception
        {
            public DeliveryIssueException() : base()
            {
            }

            public DeliveryIssueException(string message) : base("Delivery Issue: " + message)
            {
            }

            public DeliveryIssueException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected DeliveryIssueException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
        /// <summary>
        /// Exception for all the batteryissues
        /// </summary>
        [Serializable]
        internal class BatteryIssueException : Exception
        {
            public BatteryIssueException() : base()
            {
            }

            public BatteryIssueException(string message) : base("Battery Issue: " + message)
            {
            }

            public BatteryIssueException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected BatteryIssueException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        /// <summary>
        /// exception for all the adding funcitons
        /// </summary>
        [Serializable]
        internal class AddingException : Exception
        {
            public AddingException() : base()
            {
            }

            public AddingException(string message) : base("Adding Issue: " + message)
            {
            }

            public AddingException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected AddingException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

    }
}

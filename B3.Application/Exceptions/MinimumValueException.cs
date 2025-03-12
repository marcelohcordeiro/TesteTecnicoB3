using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace B3.Application.Exceptions
{
    [Serializable]
    public class MinimumValueException : Exception
    {
        protected MinimumValueException()
        {
            
        }

        public MinimumValueException(string? message) : base(message)
        {
            
        }

        public MinimumValueException(string? message, Exception? innerException) : base(message, innerException)
        {
            
        }

        protected MinimumValueException(SerializationInfo info, StreamingContext context) : base(info, context) { }
       
    }
}

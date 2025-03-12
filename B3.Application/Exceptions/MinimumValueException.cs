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
        
        public MinimumValueException(string? message) : base(message)
        {
            
        }               

        protected MinimumValueException(SerializationInfo info, StreamingContext context) : base(info, context) { }
       
    }
}

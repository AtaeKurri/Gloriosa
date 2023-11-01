using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gloriosa.Exceptions
{
    public class UnknownWorldException : Exception
    {
        public UnknownWorldException()
        {
        }

        public UnknownWorldException(string message)
            : base(message)
        {
        }

        public UnknownWorldException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

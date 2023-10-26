using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gloriosa.Utility
{
    public class InvalidScopeException : Exception
    {
        public InvalidScopeException()
        {
        }

        public InvalidScopeException(string message)
            : base(message)
        {
        }

        public InvalidScopeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

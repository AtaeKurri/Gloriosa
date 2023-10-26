using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gloriosa.Exceptions
{
    public class CannotFindResourceException : Exception
    {
        public CannotFindResourceException()
        {
        }

        public CannotFindResourceException(string message)
            : base(message)
        {
        }

        public CannotFindResourceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

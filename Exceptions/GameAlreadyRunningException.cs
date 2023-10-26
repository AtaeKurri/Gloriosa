using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gloriosa.Utility
{
    public class GameAlreadyRunningException : Exception
    {
        public GameAlreadyRunningException()
        {
        }

        public GameAlreadyRunningException(string message)
            : base(message)
        {
        }

        public GameAlreadyRunningException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

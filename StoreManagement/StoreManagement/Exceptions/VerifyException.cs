using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagement.Exceptions
{
    public class VerifyException : Exception
    {
        public VerifyException() : base()
        {

        }

        public VerifyException(string message) : base(message)
        {

        }
    }
}
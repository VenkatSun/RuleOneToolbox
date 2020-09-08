using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RuleOneToolbox.CompanyFinancialsApi
{
    /// <summary>  
    /// This class will allow to generate the custom exception message.  
    /// </summary>  
    public class RuleOneToolBoxCustomException : Exception
    {
        public RuleOneToolBoxCustomException()
        {
        }

        public RuleOneToolBoxCustomException(string message) : base(message)
        {
        }

        public RuleOneToolBoxCustomException(string message, string responseModel) : base(message)
        {
        }

        public RuleOneToolBoxCustomException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
    /// <summary>  
    /// Different types of exceptions.  
    /// </summary>  
    public enum Exceptions
    {
        NullReferenceException = 1,
        FileNotFoundException = 2,
        OverflowException = 3,
        OutOfMemoryException = 4,
        InvalidCastException = 5,
        ObjectDisposedException = 6,
        UnauthorizedAccessException = 7,
        NotImplementedException = 8,
        NotSupportedException = 9,
        InvalidOperationException = 10,
        TimeoutException = 11,
        ArgumentException = 12,
        FormatException = 13,
        StackOverflowException = 14,
        SqlException = 15,
        IndexOutOfRangeException = 16,
        IOException = 17
    }
}

using System;
using System.Collections.Generic;

namespace KeyCounter
{
    /// <summary>
    /// For now it just is the equivalent to having inner exceptions to an exception
    /// will get more features on a later update
    /// </summary>
    class ChainingException : Exception
    {
        private readonly List<string> _exceptionChain;

        public ChainingException(string initialError)
        {
            _exceptionChain = new List<string> {initialError};
        }

        public void AddErrorToChain(string newError)
        {
            _exceptionChain.Add(newError);
        }

        public List<string> GetExceptionChain()
        {
            _exceptionChain.Reverse();
            return _exceptionChain;
        }
    }
}

using System;

namespace Tuto.DataLayer.Exceptions
{
    public abstract class AbstractModelRuntimeException : Exception
    {
        protected AbstractModelRuntimeException(string message) : base(message)
        { }
    }
}